using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewRules;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace MicroscopicContentExpansion.NewComponents {
    [TypeId("73cfbad567f948ae87712cb76d59d879")]
    public class AbilityCustomStartossComet : AbilityCustomLogic {
        [SerializeField]
        public BlueprintFeatureReference m_MythicBlueprint;
        [SerializeField]
        public BlueprintFeatureReference m_RowdyFeature;
        [SerializeField]
        public BlueprintFeatureReference m_StartossShower;
        [SerializeField]
        public BlueprintFeatureReference m_VitalStrike;
        [SerializeField]
        public BlueprintFeatureReference m_VitalStrikeImproved;
        [SerializeField]
        public BlueprintFeatureReference m_VitalStrikeGreater;
        public BlueprintFeature MythicBlueprint => this.m_MythicBlueprint?.Get();

        public BlueprintFeature RowdyFeature => this.m_RowdyFeature?.Get();

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            var caster = context.MaybeCaster;
            var previousTarget = target.Unit;
            if (caster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                yield break;
            }
            var threatHand = caster.GetThreatHandRanged();
            if (threatHand == null) {
                PFLog.Default.Error("Caster can't attack", Array.Empty<object>());
                yield break;
            }
            var originalTarget = target.Unit;
            if (originalTarget == null) {
                PFLog.Default.Error("Can't be applied to point", Array.Empty<object>());
                yield break;
            }
            //first we attack our main target

            UnitEntityData maybeCaster = context.MaybeCaster;
            int attackBonusPenalty = 0;
            EventHandlers eventHandlers = null;
            var vitalStrikeMod = 1;
            if (maybeCaster.HasFact(m_VitalStrike)) {
                vitalStrikeMod = 2;
            }
            if (maybeCaster.HasFact(m_VitalStrikeImproved)) {
                vitalStrikeMod = 3;
            }
            if (maybeCaster.HasFact(m_VitalStrikeGreater)) {
                vitalStrikeMod = 4;
            }
            if (vitalStrikeMod > 1) {
                eventHandlers = new EventHandlers();
                eventHandlers.Add(new VitalStrikeEventHandler(caster, vitalStrikeMod, maybeCaster.HasFact(MythicBlueprint),
                    maybeCaster.HasFact(RowdyFeature), context.Ability.Fact));
            }
            RuleAttackWithWeapon rule = new RuleAttackWithWeapon(maybeCaster, originalTarget, caster.GetFirstWeapon(), attackBonusPenalty);
            RuleAttackWithWeapon firstAttack;

            //collect adjacent targets (collect now instead of after, because if target dies too early (i.e. Rogue Master Strike), thing can fail.
            previousTarget = originalTarget;
            var targetList = new List<UnitEntityData> { };
            foreach (UnitGroupMemory.UnitInfo unitInfo in caster.Memory.Enemies) {
                UnitEntityData unit = unitInfo.Unit;
                if (unit != originalTarget && unit.Descriptor.State.IsConscious && originalTarget.IsReach(unit, threatHand)) {
                    targetList.Add(unit);
                }
            }
            targetList.Sort((UnitEntityData u1, UnitEntityData u2) => u1.DistanceTo(originalTarget).CompareTo(u2.DistanceTo(originalTarget)));

            if (eventHandlers != null) {
                using (eventHandlers.Activate()) {
                    firstAttack = context.TriggerRule(rule);
                }
            } else {
                firstAttack = context.TriggerRule(rule);
            }
            yield return new AbilityDeliveryTarget(target);

            //then we chain if we hit
            if (firstAttack.AttackRoll.IsHit) {
                var addAttacks = 1;
                if (maybeCaster.HasFact(this.m_StartossShower)) {
                    addAttacks = 1 + caster.Stats.BaseAttackBonus.ModifiedValue / 5;
                }
                List<UnitEntityData> hitTargets = new List<UnitEntityData>();
                List<UnitEntityData> validTargets = new List<UnitEntityData>(targetList);
                var weaponRange = threatHand.Weapon.AttackRange.Meters;

                //wait 0.2 seconds before starting chaining
                while (firstAttack.LaunchedProjectiles.Any(t => !t.IsHit)) {
                    if (firstAttack.LaunchedProjectiles.Any(t => t.Cleared)) {
                        yield break;
                    }
                    yield return null;
                }
                var initialStartTime = Game.Instance.TimeController.GameTime;
                while (Game.Instance.TimeController.GameTime - initialStartTime < ((float)0.2f).Seconds())
                    yield return null;


                while (validTargets.Count > 0) {
                    validTargets
                        .Sort((UnitEntityData u1, UnitEntityData u2) => u1.DistanceTo(previousTarget).CompareTo(u2.DistanceTo(previousTarget)));
                    validTargets = validTargets
                        .Where(t => t.Descriptor.State.IsConscious)
                        .Where(t => t.DistanceTo(previousTarget) <= (t.View.Corpulence + previousTarget.View.Corpulence + weaponRange))
                        .ToList();
                    var currentTarget = validTargets.FirstOrDefault();
                    if (currentTarget == null) { break; }
                    var chainSource = previousTarget != currentTarget ? previousTarget : null;
                    var res = context.TriggerRule(new RuleAttackWithWeaponChaining(caster, chainSource, currentTarget, threatHand.Weapon, 0) {
                        IsFirstAttack = hitTargets.Any()
                    });

                    if (!res.AttackRoll.IsHit) {
                        break;
                    }
                    //wait for projectile to hit and then wait additional 0.2s 
                    while (res.LaunchedProjectiles.Any(t => !t.IsHit)) {
                        if (res.LaunchedProjectiles.Any(t => t.Cleared)) {
                            yield break;
                        }
                        yield return null;
                    }
                    var startTime = Game.Instance.TimeController.GameTime;
                    while (Game.Instance.TimeController.GameTime - startTime < ((float)0.2f).Seconds())
                        yield return null;

                    hitTargets.Add(currentTarget);
                    previousTarget = currentTarget;
                    yield return new AbilityDeliveryTarget(currentTarget);
                    if (hitTargets.Count >= addAttacks) {
                        break;
                    }
                    validTargets = targetList.Where(t => !hitTargets.Contains(t)).ToList();
                }
                yield break;
            }
        }

        public override void Cleanup(AbilityExecutionContext context) {
        }   

        public override bool IsEngageUnit => true;

        private class EventHandlers : IDisposable {
            private readonly List<object> m_Handlers = new List<object>();

            public void Add(object handler) => this.m_Handlers.Add(handler);

            public EventHandlers Activate() {
                foreach (object handler in this.m_Handlers)
                    EventBus.Subscribe(handler);
                return this;
            }

            public void Dispose() {
                foreach (object handler in this.m_Handlers)
                    EventBus.Unsubscribe(handler);
            }
        }
        /**
         * Copyright (c) 2021 Sean Petrie (Vek17)
         */
        private class VitalStrikeEventHandler : IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
                IRulebookHandler<RuleCalculateWeaponStats>,
                IInitiatorRulebookHandler<RulePrepareDamage>,
                IRulebookHandler<RulePrepareDamage>,
                IInitiatorRulebookHandler<RuleAttackWithWeapon>,
                IRulebookHandler<RuleAttackWithWeapon>,
                ISubscriber, IInitiatorRulebookSubscriber {

            public VitalStrikeEventHandler(UnitEntityData unit, int damageMod, bool mythic, bool rowdy, EntityFact fact) {
                this.m_Unit = unit;
                this.m_DamageMod = damageMod;
                this.m_Mythic = mythic;
                this.m_Rowdy = rowdy;
                this.m_Fact = fact;
            }

            public UnitEntityData GetSubscribingUnit() {
                return this.m_Unit;
            }

            public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            }

            public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
                DamageDescription damageDescription = evt.DamageDescription.FirstItem();
                if (damageDescription != null && damageDescription.TypeDescription.Type == DamageType.Physical) {
                    if (!evt.DoNotScaleDamage
                            && (evt.WeaponDamageDice.HasModifications
                                || !evt.Weapon.Blueprint.IsDamageDiceOverridden
                                || (evt.Initiator.IsPlayerFaction && !evt.Initiator.Body.IsPolymorphed)
                                || evt.IsDefaultUnit)) {
                        //DiceFormula diceFormula = WeaponDamageScaleTable.Scale(evt.WeaponDamageDice.ModifiedValue, evt.WeaponSize, Size.Medium, evt.Weapon.Blueprint);
                        //if (diceFormula != evt.WeaponDamageDice.ModifiedValue) {
                        //    evt.WeaponDamageDice.Modify(diceFormula, ModifierDescriptor.Size);
                        //}
                    }
                    var vitalDamage = CalculateVitalDamage(evt);
                    //new DamageDescription() {
                    //Dice = new DiceFormula(damageDescription.Dice.Rolls * Math.Max(1, this.m_DamageMod - 1), damageDescription.Mo.Dice.Dice),
                    //Bonus = this.m_Mythic ? damageDescription.Bonus * Math.Max(1, this.m_DamageMod - 1) : 0,
                    //TypeDescription = damageDescription.TypeDescription,
                    //IgnoreReduction = damageDescription.IgnoreReduction,
                    //IgnoreImmunities = damageDescription.IgnoreImmunities,
                    //SourceFact = this.m_Fact,
                    //CausedByCheckFail = damageDescription.CausedByCheckFail,
                    //m_BonusWithSource = 0
                    //};
                    evt.DamageDescription.Insert(1, vitalDamage);
                }
            }

            private DamageDescription CalculateVitalDamage(RuleCalculateWeaponStats evt) {
                var WeaponDice = new ModifiableDiceFormula(evt.WeaponDamageDice.ModifiedValue);
                //var WeaponDice = new ModifiableDiceFormula(evt.WeaponDamageDice.BaseFormula);
                //WeaponDice.m_Modifications = evt.WeaponDamageDice.Modifications.ToList();
                WeaponDice.Modify(new DiceFormula(WeaponDice.ModifiedValue.Rolls * Math.Max(1, this.m_DamageMod - 1), WeaponDice.ModifiedValue.Dice), m_Fact);

                DamageDescription damageDescriptor = evt.Weapon.Blueprint.DamageType.GetDamageDescriptor(WeaponDice, evt.Initiator.Stats.AdditionalDamage.BaseValue);
                damageDescriptor.TemporaryContext(dd => {
                    dd.TypeDescription.Physical.Enhancement = evt.Enhancement;
                    dd.TypeDescription.Physical.EnhancementTotal = evt.EnhancementTotal + evt.Weapon.EnchantmentValue;
                    if (this.m_Mythic) {
                        dd.AddModifier(new Modifier(evt.DamageDescription.FirstItem().Bonus * Math.Max(1, this.m_DamageMod - 1), evt.Initiator.GetFact(m_Fact.Blueprint.GetComponent<AbilityCustomStartossComet>().MythicBlueprint), ModifierDescriptor.UntypedStackable));
                    }
                    dd.TypeDescription.Common.Alignment = evt.Alignment;
                    dd.SourceFact = m_Fact;
                });
                return damageDescriptor;
            }

            public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
            }

            //For Ranged - Handling of damage calcs does not occur the same due to projectiles
            public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
                if (!m_Rowdy) { return; }
                var RowdyFact = evt.Initiator.GetFact(m_Fact.Blueprint.GetComponent<AbilityCustomStartossComet>().RowdyFeature);
                RuleAttackRoll ruleAttackRoll = evt.AttackRoll;
                if (ruleAttackRoll == null) { return; }
                if (evt.Initiator.Stats.SneakAttack < 1) { return; }
                if (!ruleAttackRoll.TargetUseFortification) {
                    var FortificationCheck = Rulebook.Trigger<RuleFortificationCheck>(new RuleFortificationCheck(ruleAttackRoll));
                    if (FortificationCheck.UseFortification) {
                        ruleAttackRoll.FortificationChance = FortificationCheck.FortificationChance;
                        ruleAttackRoll.FortificationRoll = FortificationCheck.Roll;
                    }
                }
                if (!ruleAttackRoll.TargetUseFortification || ruleAttackRoll.FortificationOvercomed) {
                    DamageTypeDescription damageTypeDescription = evt.ResolveRules
                        .Select(e => e.Damage).First()
                        .DamageBundle.First<BaseDamage>().CreateTypeDescription();
                    var rowdyDice = new ModifiableDiceFormula(new DiceFormula(evt.Initiator.Stats.SneakAttack * 2, DiceType.D6));
                    var RowdyDamage = damageTypeDescription.GetDamageDescriptor(rowdyDice, 0);
                    RowdyDamage.SourceFact = RowdyFact;
                    BaseDamage baseDamage = RowdyDamage.CreateDamage();
                    baseDamage.Precision = true;
                    evt.ResolveRules.Select(e => e.Damage)
                        .ForEach(e => e.Add(baseDamage));
                }
            }

            //For Melee
            public void OnEventAboutToTrigger(RulePrepareDamage evt) {
                if (!m_Rowdy) { return; }
                var RowdyFact = evt.Initiator.GetFact(m_Fact.Blueprint.GetComponent<AbilityCustomStartossComet>().RowdyFeature);
                RuleAttackRoll ruleAttackRoll = evt.ParentRule.AttackRoll;
                if (ruleAttackRoll == null) { return; }
                if (evt.Initiator.Stats.SneakAttack < 1) { return; }
                if (!ruleAttackRoll.TargetUseFortification) {
                    var FortificationCheck = Rulebook.Trigger<RuleFortificationCheck>(new RuleFortificationCheck(ruleAttackRoll));
                    if (FortificationCheck.UseFortification) {
                        ruleAttackRoll.FortificationChance = FortificationCheck.FortificationChance;
                        ruleAttackRoll.FortificationRoll = FortificationCheck.Roll;
                    }
                }
                if (!ruleAttackRoll.TargetUseFortification || ruleAttackRoll.FortificationOvercomed) {
                    DamageTypeDescription damageTypeDescription = evt.DamageBundle
                        .First()
                        .CreateTypeDescription();
                    var rowdyDice = new ModifiableDiceFormula(new DiceFormula(evt.Initiator.Stats.SneakAttack * 2, DiceType.D6));
                    var RowdyDamage = damageTypeDescription.GetDamageDescriptor(rowdyDice, 0);
                    RowdyDamage.SourceFact = RowdyFact;
                    BaseDamage baseDamage = RowdyDamage.CreateDamage();
                    baseDamage.Precision = true;
                    evt.Add(baseDamage);
                }
            }

            public void OnEventDidTrigger(RulePrepareDamage evt) {
            }

            private readonly UnitEntityData m_Unit;
            private readonly EntityFact m_Fact;
            private int m_DamageMod;
            private bool m_Mythic;
            private bool m_Rowdy;
        }
    }
}
