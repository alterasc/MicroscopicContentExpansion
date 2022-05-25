using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Logging;
using UnityEngine;


namespace MicroscopicContentExpansion.NewComponents {
    internal class AbilityCustomStartossComet : AbilityCustomLogic {
        public int VitalStrikeMod;
        [SerializeField]
        private BlueprintFeatureReference m_MythicBlueprint;
        [SerializeField]
        private BlueprintFeatureReference m_RowdyFeature;

        public BlueprintFeature MythicBlueprint => this.m_MythicBlueprint?.Get();

        public BlueprintFeature RowdyFeature => this.m_RowdyFeature?.Get();

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            AbilityCustomStartossComet ctx = this;
            UnitEntityData maybeCaster = context.MaybeCaster;
            if (maybeCaster == (UnitDescriptor)null) {
                PFLog.Default.Error((ICanBeLogContext)ctx, "Caster is missing");
            } else {
                ItemEntityWeapon firstWeapon = maybeCaster.GetFirstWeapon();
                if (firstWeapon == null) {
                    PFLog.Default.Error("Caster can't attack");
                } else {
                    UnitEntityData unit = target.Unit;
                    if (unit == (UnitDescriptor)null) {
                        PFLog.Default.Error("Can't be applied to point");
                    } else {
                        int attackBonusPenalty = 0;
                        AbilityCustomStartossComet.EventHandlers eventHandlers = new AbilityCustomStartossComet.EventHandlers();
                        eventHandlers.Add((object)new AbilityCustomStartossComet.VitalStrike(maybeCaster, ctx.VitalStrikeMod, maybeCaster.HasFact((BlueprintFact)ctx.MythicBlueprint), maybeCaster.HasFact((BlueprintFact)ctx.RowdyFeature)));
                        RuleAttackWithWeapon rule = new RuleAttackWithWeapon(maybeCaster, unit, firstWeapon, attackBonusPenalty);
                        using (eventHandlers.Activate())
                            context.TriggerRule<RuleAttackWithWeapon>(rule);
                        yield return new AbilityDeliveryTarget(target);
                    }
                }
            }
        }

        public override void Cleanup(AbilityExecutionContext context) {
        }

        private class EventHandlers : IDisposable {
            private readonly List<object> m_Handlers = new List<object>();

            public void Add(object handler) => this.m_Handlers.Add(handler);

            public AbilityCustomStartossComet.EventHandlers Activate() {
                foreach (object handler in this.m_Handlers)
                    EventBus.Subscribe(handler);
                return this;
            }

            public void Dispose() {
                foreach (object handler in this.m_Handlers)
                    EventBus.Unsubscribe(handler);
            }
        }

        public class VitalStrike :
          IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
          IRulebookHandler<RuleCalculateWeaponStats>,
          ISubscriber,
          IInitiatorRulebookSubscriber {
            private readonly UnitEntityData m_Unit;
            private int m_DamageMod;
            private bool m_Mythic;
            private bool m_Rowdy;

            public VitalStrike(UnitEntityData unit, int damageMod, bool mythic, bool rowdy) {
                this.m_Unit = unit;
                this.m_DamageMod = damageMod;
                this.m_Mythic = mythic;
                this.m_Rowdy = rowdy;
            }

            public UnitEntityData GetSubscribingUnit() => this.m_Unit;

            public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            }

            public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
                DamageDescription damageDescription1 = evt.DamageDescription.FirstItem<DamageDescription>();
                if (damageDescription1 == null || damageDescription1.TypeDescription.Type != DamageType.Physical)
                    return;
                damageDescription1.Dice = new DiceFormula(damageDescription1.Dice.Rolls * this.m_DamageMod, damageDescription1.Dice.Dice);
                if (this.m_Mythic)
                    damageDescription1.Bonus *= this.m_DamageMod;
                if (!this.m_Rowdy || evt.Initiator.Descriptor.Stats.SneakAttack.ModifiedValue <= 0)
                    return;
                DamageDescription damageDescription2 = new DamageDescription();
                DamageTypeDescription typeDescription = evt.DamageDescription.FirstItem<DamageDescription>().TypeDescription;
                damageDescription2.TypeDescription = new DamageTypeDescription() {
                    Common = new DamageTypeDescription.CommomData() {
                        Alignment = typeDescription.Common.Alignment,
                        Precision = true,
                        Reality = typeDescription.Common.Reality
                    },
                    Energy = typeDescription.Energy,
                    Physical = new DamageTypeDescription.PhysicalData() {
                        Enhancement = typeDescription.Physical.Enhancement,
                        EnhancementTotal = typeDescription.Physical.EnhancementTotal,
                        Form = typeDescription.Physical.Form,
                        Material = typeDescription.Physical.Material
                    },
                    Type = typeDescription.Type
                };
                damageDescription2.Dice = new DiceFormula(2 * evt.Initiator.Descriptor.Stats.SneakAttack.ModifiedValue, DiceType.D6);
                evt.DamageDescription.Add(damageDescription2);
            }
        }
    }
}
