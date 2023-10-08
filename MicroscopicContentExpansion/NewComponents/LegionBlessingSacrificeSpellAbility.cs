using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;

namespace MicroscopicContentExpansion.NewComponents {
    [TypeId("e626812134a646d2bbd1be80b7fad21d")]
    public class LegionBlessingSacrificeSpellAbility : AbilityApplyEffect {

        public BlueprintBuffReference[] LegionBlessingBuffs;
        public override void Apply(AbilityExecutionContext context, TargetWrapper target) {
            var spellLevel = context.Ability.SpellLevel;
            if (spellLevel >= 4 && spellLevel <= 9) {
                var buff = LegionBlessingBuffs[spellLevel - 4];
                context.MaybeCaster?.AddBuff(buff, null, null);
            }
        }
    }
}
