using Kingmaker.Blueprints;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.Utils {
    internal class AuraUtils {

        public static AbilityAreaEffectRunAction CreateUnconditionalAuraEffect(BlueprintBuffReference buff) {
            return new AbilityAreaEffectRunAction() {
                UnitEnter = ActionFlow.DoSingle<ContextActionApplyBuff>(b => {
                    b.m_Buff = buff;
                    b.Permanent = true;
                    b.DurationValue = new ContextDurationValue();
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

        public static BlueprintAbilityAreaEffect CreateUnconditionalHostileAuraEffect(TabletopTweaks.Core.ModLogic.ModContextBase modContext, string bpName, int size, BlueprintBuffReference buff) {
            return Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(modContext, bpName, bp => {
                bp.AggroEnemies = true;
                bp.AffectEnemies = true;
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = size.Feet();
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.Condition = new();
                    c.m_Buff = buff;
                });
            });
        }
    }
}
