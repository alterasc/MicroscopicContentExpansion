using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;

namespace MicroscopicContentExpansion.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("ac67128448634c5b88bfc44d3009bb8d")]
    public class SnakeFangOnMissHandler :
        UnitFactComponentDelegate,
        ITargetRulebookHandler<RuleAttackWithWeapon>,
        IRulebookHandler<RuleAttackWithWeapon>,
        ISubscriber,
        ITargetRulebookSubscriber {

        public BlueprintUnitFactReference m_Fact;
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
            if (evt.AttackRoll.IsHit || !evt.Target.HasFact(m_Fact) || !evt.Target.GetFirstWeapon().IsMonkUnarmedStrike)
                return;
            if (this.Fact is IFactContextOwner) {
                Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(evt.Target, evt.Initiator);
            }
        }

    }
}