using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using MicroscopicContentExpansion.Utils;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Utils.ActionFlow;


namespace MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures;
internal class BeaconOfEvil
{
    const string NAME = "Beacon of Evil";

    const string DESCRIPTION = @"A dread vanguard unleashes the powers of his vile masters to strengthen both himself and his allies.
At 4th level and every 4 level thereafter, a dread vanguard gains one additional use of his touch of corruption ability per day. As a standard action, he can spend a use of his touch of corruption ability to manifest the darkness in his soul as an area of flickering shadows with a 30-foot radius centered on him. These shadows don’t affect visibility. The antipaladin and all allies in the area gain a +1 morale bonus to AC and on attack rolls, damage rolls, and saving throws against fear. This lasts for 1 minute, as long as the dread vanguard is conscious.
At 8th level, the aura grants fast healing 3 to the dread vanguard as well as to his allies while they remain within it. Additionally, while this aura is active, the antipaladin can use his touch of corruption ability against any targets within its radius by making a ranged touch attack.
At 12th level, when he activates this ability, a dread vanguard can choose to increase the radius of one antipaladin aura he possesses to 30 feet. Also, the morale bonus granted to AC and on attack rolls, damage rolls, and saving throws against fear increases to +2.
At 16th level, the fast healing granted by this ability increases to 5. Additionally, the antipaladin’s weapons and those of his allies within the aura’s radius are considered evil for the purpose of overcoming damage reduction.
At 20th level, the beacon of evil’s radius increases to 50 feet, and the morale bonus granted to AC and on attack rolls, damage rolls, and saving throws against fear increases to +4. Lastly, attacks made by the dread vanguard and his allies within the aura’s radius are infused with pure unholy power, and deal an additional 1d6 points of damage.";

    public static BlueprintFeatureReference AddBeaconOfEvil()
    {
        var AntipaladinClass = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
        var TouchOfCorruptionResource = MCEContext.GetModBlueprintReference<BlueprintAbilityResourceReference>("AntipaladinTouchOfCorruptionResource");
        var allTouchesOfCorruption = new List<BlueprintAbilityReference> {
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionUnmodified"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionBlinded"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionCursed"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionDazed"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionDiseased"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionExhausted"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionFatigued"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionFrightened"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionNauseated"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionParalyzed"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionPoisoned"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionShaken"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionSickened"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionStaggered"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionStunned")
                };

        var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("a02cf51787df937489ef5d4cf5970335").Icon;

        var beaconOfEvilAreaEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DreadVanguardBeaconOfEvilAreaEffectBuff", bp =>
        {
            bp.SetName(MCEContext, NAME);
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.m_Icon = icon;
            bp.Stacking = StackingType.Replace;
            bp.IsClassFeature = true;
            bp.Frequency = DurationRate.Rounds;

            bp.AddComponent<SavingThrowContextBonusAgainstDescriptor>(c =>
            {
                c.SpellDescriptor = SpellDescriptor.Fear;
                c.ModifierDescriptor = ModifierDescriptor.Morale;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageDice,
                };
            });

            bp.AddComponent<AddContextStatBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.Morale;
                c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageDice,
                };
            });

            bp.AddComponent<AddContextStatBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.Morale;
                c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageDice,
                };
            });

            bp.AddComponent<AddContextStatBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.Morale;
                c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalDamage;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageDice,
                };
            });

            bp.AddComponent<AddEffectContextFastHealing>(c =>
            {
                c.Bonus = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus,
                };
            });
            /*
            bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                c.OnlyHit = true;
                c.CriticalHit = false;
                c.Action = DoSingle<ContextActionDealDamage>(a => {
                    a.Value = new ContextDiceValue() {
                        DiceType = DiceType.D6,
                        DiceCountValue = 1,
                        BonusValue = 0
                    };
                    a.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Unholy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData()
                    };
                    a.Duration = new ContextDurationValue();
                });
            });*/

            bp.AddComponent<ContextRankConfig>(c =>
            {
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.Custom;
                c.m_Type = AbilityRankType.DamageDice;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClass };
                c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 11,
                        ProgressionValue = 1
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 19,
                        ProgressionValue = 2
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 100,
                        ProgressionValue = 4
                    }
                };
            });

            bp.AddComponent<ContextRankConfig>(c =>
            {
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.Custom;
                c.m_Type = AbilityRankType.DamageBonus;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClass };
                c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 7,
                        ProgressionValue = 0
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 15,
                        ProgressionValue = 3
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 100,
                        ProgressionValue = 5
                    }
                };
            });

            bp.IsClassFeature = true;
            bp.FxOnRemove = new PrefabLink();
            bp.FxOnStart = new PrefabLink();
        });

        var inspireGreatnessFx = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("23ddd38738bd1d84595f3cdbb8512873").Fx;
        var inspireCourageFx = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("5d4308fa344af0243b2dd3b1e500b2cc").Fx;

        var beaconOfEvilAreaEffect = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "DreadVanguardBeaconOfEvilAreaEffect", bp =>
        {
            bp.Shape = AreaEffectShape.Cylinder;
            bp.AggroEnemies = false;
            bp.Size = 30.Feet();
            bp.Fx = inspireGreatnessFx;
            bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
            bp.AddComponent(AuraUtils.CreateUnconditionalAuraEffect(beaconOfEvilAreaEffectBuff.ToReference<BlueprintBuffReference>()));
        });

        var beaconOfEvilAreaEffect20 = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "DreadVanguardBeaconOfEvilAreaEffect20", bp =>
        {
            bp.m_TargetType = beaconOfEvilAreaEffect.m_TargetType;
            bp.Shape = beaconOfEvilAreaEffect.Shape;
            bp.Size = 50.Feet();
            bp.Fx = inspireCourageFx;
            bp.AddComponent(AuraUtils.CreateUnconditionalAuraEffect(beaconOfEvilAreaEffectBuff.ToReference<BlueprintBuffReference>()));
        });

        var beaconOfEvilBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DreadVanguardBeaconOfEvilBuff", bp =>
        {
            bp.SetName(MCEContext, $"{NAME}");
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.IsClassFeature = true;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.Stacking = StackingType.Prolong;
            bp.Frequency = DurationRate.Rounds;
            bp.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = beaconOfEvilAreaEffect.ToReference<BlueprintAbilityAreaEffectReference>();
            });
            bp.AddComponent<AutoMetamagic>(c =>
            {
                c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
                c.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach;
                c.Abilities = allTouchesOfCorruption;
            });
            bp.FxOnRemove = new PrefabLink();
        });


        var beaconOfEvilBuff20 = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DreadVanguardBeaconOfEvilBuff20", bp =>
        {
            bp.SetName(MCEContext, $"{NAME}");
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.IsClassFeature = true;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.Stacking = StackingType.Summ;
            bp.Frequency = DurationRate.Rounds;
            bp.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = beaconOfEvilAreaEffect20.ToReference<BlueprintAbilityAreaEffectReference>();
            });
            bp.AddComponent<AutoMetamagic>(c =>
            {
                c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
                c.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Reach;
                c.Abilities = allTouchesOfCorruption;
            });
            bp.FxOnRemove = new PrefabLink();
        });

        var beacon20 = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature20");

        var beaconOfEvilAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DreadVanguardBeaconOfEvilAbility", bp =>
        {
            bp.SetName(MCEContext, $"{NAME}");
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.m_Icon = icon;
            bp.Type = AbilityType.Supernatural;
            bp.Range = AbilityRange.Personal;
            bp.CustomRange = 30.Feet();
            bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            bp.EffectOnEnemy = AbilityEffectOnUnit.None;
            bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self;
            bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.AddComponent<AbilityResourceLogic>(c =>
            {
                c.m_RequiredResource = TouchOfCorruptionResource;
                c.m_IsSpendResource = true;
            });
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = DoSingle<Conditional>(ac =>
                {
                    ac.ConditionsChecker = IfSingle<ContextConditionCasterHasFact>(cond =>
                    {
                        cond.m_Fact = beacon20.ToReference<BlueprintUnitFactReference>();
                    });
                    ac.IfTrue = DoSingle<ContextActionApplyBuff>(cc =>
                    {
                        cc.m_Buff = beaconOfEvilBuff20.ToReference<BlueprintBuffReference>();
                        cc.ToCaster = true;
                        cc.DurationValue = Create1MinDuration();
                    });
                    ac.IfFalse = DoSingle<ContextActionApplyBuff>(cc =>
                    {
                        cc.m_Buff = beaconOfEvilBuff.ToReference<BlueprintBuffReference>();
                        cc.ToCaster = true;
                        cc.DurationValue = Create1MinDuration();
                    });
                });
            });
        });

        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature", bp =>
        {
            bp.SetName(MCEContext, $"{NAME}");
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.IsClassFeature = true;
            bp.m_Icon = icon;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[] { beaconOfEvilAbility.ToReference<BlueprintUnitFactReference>() };
            });
        }).ToReference<BlueprintFeatureReference>();
    }

    private static ContextDurationValue Create1MinDuration()
    {
        return new ContextDurationValue()
        {
            Rate = DurationRate.Minutes,
            DiceType = DiceType.Zero,
            DiceCountValue = 0,
            BonusValue = 1
        };
    }
}
