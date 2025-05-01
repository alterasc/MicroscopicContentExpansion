using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization.Shared;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.RebalancedContent.MythicFeats;
internal class MythicDodge
{
    internal static void Rework()
    {
        var mythicDodgeFeat = GetBP<BlueprintFeature>("3812d9ca1377c014c97fb3ac421ba9e6");

        var mythicDodgeEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DodgeMythicEffectBuff", a =>
        {
            a.m_DisplayName = mythicDodgeFeat.m_DisplayName;
            a.m_Icon = mythicDodgeFeat.m_Icon;
            a.m_Description = new();
            a.AddComponent<ModifyD20>(x =>
            {
                x.Rule = RuleType.AttackRoll;
                x.RollsAmount = 1;
                x.RollCondition = ModifyD20.RollConditionType.Equal;
                x.ValueToCompareRoll = 20;
            });
            a.AddComponent<AddInitiatorAttackWithWeaponTrigger>(x =>
            {
                x.TriggerBeforeAttack = true;
                x.OnlyHit = true;
                x.Action = ActionFlow.DoSingle<Conditional>(x =>
                {
                    x.ConditionsChecker = ActionFlow.IfSingle<ContextConditionHasFact>(c =>
                    {
                        c.Not = true;
                        c.m_Fact = mythicDodgeFeat.ToReference<BlueprintUnitFactReference>();
                    });
                    x.IfTrue = ActionFlow.DoSingle<ContextActionRemoveSelf>();
                    x.IfFalse = new();
                });
            });
            a.AddComponent<TargetChangedDuringRound>(x =>
            {
                x.Actions = ActionFlow.DoSingle<ContextActionRemoveSelf>();
            });
            a.m_Flags = BlueprintBuff.Flags.HiddenInUi;
        }).ToReference<BlueprintBuffReference>();

        var reworkDesc = Helpers.CreateString(MCEContext, "MythicDodgeRework.Description", "When focused, you become nearly impossible to strike.\r\nBenefit: Whenever you are targeted by an effect that requires an {g|Encyclopedia:Attack}attack roll{/g}, including weapon attacks, if attacker {g|Encyclopedia:Dice}rolls{/g} a natural 20 on an {g|Encyclopedia:Attack}attack roll{/g}, they must reroll the die once.", Locale.enGB, shouldProcess: true);

        if (MCEContext.Homebrew.MythicFeats.IsDisabled("MythicDodgeRework"))
        {
            return;
        }

        mythicDodgeFeat.RemoveComponents<BlueprintComponent>(x => x is not PrerequisiteFeature);
        mythicDodgeFeat.AddComponent<AddTargetBeforeAttackRollTrigger>(x =>
        {
            x.ActionsOnAttacker = ActionFlow.DoSingle<ContextActionApplyBuff>(x =>
            {
                x.m_Buff = mythicDodgeEffectBuff;
                x.DurationValue = new()
                {
                    Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds,
                    DiceCountValue = 0,
                    BonusValue = 1,
                    m_IsExtendable = true
                };
            });
            x.ActionOnSelf = new();
        });
        mythicDodgeFeat.AddComponent<AddAbilityUseTargetTrigger>(x =>
        {
            x.DontCheckType = true;
            x.ToCaster = true;
            x.Action = ActionFlow.DoSingle<Conditional>(c =>
            {
                c.ConditionsChecker = ActionFlow.IfSingle<ContextConditionIsEnemy>();
                c.IfTrue = ActionFlow.DoSingle<ContextActionApplyBuff>(x =>
                {
                    x.m_Buff = mythicDodgeEffectBuff;
                    x.DurationValue = new()
                    {
                        Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds,
                        DiceCountValue = 0,
                        BonusValue = 1,
                        m_IsExtendable = true
                    };
                });
            });
        });
        mythicDodgeFeat.m_Description = reworkDesc;
    }
}
