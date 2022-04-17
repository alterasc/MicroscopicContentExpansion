using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace MicroscopicContentExpansion.Utils {
    internal class AuraUtils {

        public static AbilityAreaEffectRunAction CreateUnconditionalAuraEffect(BlueprintBuffReference buff) {
            return new AbilityAreaEffectRunAction() {
                UnitEnter = ActionFlow.DoSingle<ContextActionApplyBuff>(b => {
                    b.m_Buff = buff;
                    b.Permanent = true;
                    b.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Rounds,
                        DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 0,
                            ValueRank = AbilityRankType.Default,
                            ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage,
                            Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.None
                        },
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 1,
                            ValueRank = AbilityRankType.Default,
                            ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage,
                            Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.None
                        }
                    };
                }),
                UnitExit = ActionFlow.DoSingle<ContextActionRemoveBuff>(b => {
                    b.m_Buff = buff;
                    b.RemoveRank = false;
                    b.ToCaster = false;
                }),
                UnitMove = ActionFlow.DoNothing(),
                Round = ActionFlow.DoNothing()
            };
        }
    }
}
