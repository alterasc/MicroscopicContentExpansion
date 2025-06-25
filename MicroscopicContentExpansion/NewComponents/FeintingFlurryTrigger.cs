using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace MicroscopicContentExpansion.NewComponents;
internal class FeintingFlurryTrigger : EntityFactComponentDelegate,
    IInitiatorRulebookHandler<RuleAttackWithWeapon>,
    IRulebookHandler<RuleAttackWithWeapon>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public BlueprintUnitFactReference flurryReference;

    public BlueprintArchetypeReference zenArcherArchetype;

    public BlueprintUnitFactReference feint;

    public BlueprintUnitFactReference rangedFeint;

    public BlueprintCharacterClassReference monkClass;

    public ActionList action;

    public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
    {
        if (!evt.IsFullAttack || !evt.IsFirstAttack)
        {
            return;
        }
        if (!evt.Initiator.HasFact(flurryReference))
        {
            return;
        }
        if (!evt.Initiator.HasFact(feint))
        {
            return;
        }
        var attackType = evt.Weapon.Blueprint.AttackType;
        if (attackType.IsMelee())
        {
            RunActions(this, evt, base.Context, base.Fact);
        }
        else if (attackType.IsRanged())
        {
            if (!evt.Initiator.HasFact(rangedFeint))
            {
                return;
            }
            var isZenArcher = evt.Initiator.Progression.IsArchetype(zenArcherArchetype);
            if (isZenArcher)
            {
                var level = evt.Initiator.Progression.GetClassLevel(monkClass);
                if (level >= 7)
                {
                    RunActions(this, evt, base.Context, base.Fact);
                }
            }
        }
    }

    public void OnEventDidTrigger(RuleAttackWithWeapon evt)
    {

    }

    private static void RunActions(FeintingFlurryTrigger c, RuleAttackWithWeapon rule, MechanicsContext context, EntityFact fact)
    {
        UnitEntityData unit = rule.Target;
        if (!fact.IsDisposed)
        {
            fact.RunActionInContext(c.action, unit);
        }
        else
        {
            using (context.GetDataScope(unit))
            {
                c.action.Run();
            }
        }
    }
}