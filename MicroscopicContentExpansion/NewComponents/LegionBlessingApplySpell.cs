using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using System.Collections.Generic;

namespace MicroscopicContentExpansion.NewComponents {
    [TypeId("c184044ab8934085bc7ed8eded0ddb4c")]
    public class LegionBlessingApplySpell :
    UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCastSpell>,
    IRulebookHandler<RuleCastSpell>,
    ISubscriber,
    IInitiatorRulebookSubscriber {

        public int SpellLevel;
        public BlueprintSpellbookReference SpellbookReference;

        void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(
          RuleCastSpell evt) {
        }

        void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt) {
            if (evt.IsDuplicateSpellApplied
                || !evt.Success
                || evt.Spell.Blueprint.Type != AbilityType.Spell
                || evt.Spell.Range != AbilityRange.Touch
                || evt.Spell.SpellLevel != SpellLevel
                || evt.Spell.SpellbookBlueprint != SpellbookReference.Get()
                || !evt.SpellTarget.Unit.IsAlly(Owner))
                return;
            AbilityData spell = evt.Spell;
            List<UnitEntityData> alliesAround = GetTargetsAroundYou(spell, evt.SpellTarget.Unit);
            var touchAbility = spell.Blueprint.GetComponent<AbilityEffectStickyTouch>();
            if (touchAbility != null) {
                spell = new AbilityData(evt.Spell, touchAbility.TouchDeliveryAbility);
            }
            foreach (var target in alliesAround) {
                Rulebook.Trigger(new RuleCastSpell(spell, target) {
                    IsDuplicateSpellApplied = true
                });
            }
            Owner.RemoveFact(Fact);
        }

        private List<UnitEntityData> GetTargetsAroundYou(AbilityData data, UnitEntityData baseTarget) {
            List<UnitEntityData> unitsInRange = EntityBoundsHelper.FindUnitsInRange(Owner.Position, 13.Feet().Meters);
            unitsInRange.Remove(baseTarget);
            unitsInRange.RemoveAll(x => x.Faction != baseTarget.Faction || !data.CanTarget((TargetWrapper)x));
            return unitsInRange;
        }
    }
}
