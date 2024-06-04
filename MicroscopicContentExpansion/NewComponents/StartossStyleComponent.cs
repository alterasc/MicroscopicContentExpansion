using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace MicroscopicContentExpansion.NewComponents;


[AllowMultipleComponents]
[AllowedOn(typeof(BlueprintUnitFact))]
[TypeId("8b13837d88a24b63bd3ce04b76b2664e")]
public class StartossStyleComponent : UnitFactComponentDelegate,
    IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
    IRulebookHandler<RuleCalculateWeaponStats>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{

    public BlueprintParametrizedFeatureReference ChosenWeaponFeature;
    public BlueprintFeatureReference StartossComet;
    public BlueprintFeatureReference StartossShower;
    public WeaponFighterGroup WeaponGroup;
    public BlueprintFeatureReference WeaponGroupReference;

    public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
    {
        if (IsSuitable(evt))
        {
            var addDmg = 2;
            if (Owner.HasFact(StartossComet))
            {
                addDmg += 2;
            }
            if (Owner.HasFact(StartossShower))
            {
                addDmg += 2;
            }
            evt.AddDamageModifier(addDmg, base.Fact, ModifierDescriptor.UntypedStackable);
        }
    }

    public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
    {
    }

    private bool HasSuitableWeapon(WeaponSlot slot)
    {
        return slot.MaybeWeapon != null
            && slot.Owner.GetFeature((BlueprintFeature)this.ChosenWeaponFeature, (FeatureParam)slot.MaybeWeapon.Blueprint.Category) != null;
    }

    private bool IsSuitable(RuleCalculateWeaponStats evt)
    {
        var weapon = evt.Weapon;
        var ruleCalculateAttackBonus = new RuleCalculateAttackBonusWithoutTarget(evt.Initiator, weapon, 0);
        ruleCalculateAttackBonus.WeaponStats.m_Triggered = true;
        Rulebook.Trigger(ruleCalculateAttackBonus);

        return weapon.Blueprint.IsRanged
            && weapon.Blueprint.FighterGroup.Contains(WeaponGroup)
            && (HasSuitableWeapon(Owner.Body.PrimaryHand) || Owner.HasFact(WeaponGroupReference))
            && IsOffHandNotHoldingAnotherWeapon();
    }

    private bool IsOffHandNotHoldingAnotherWeapon()
    {
        var secondaryHand = Owner.Body.CurrentHandsEquipmentSet.SecondaryHand;
        bool hasFreeHand = true;
        if (secondaryHand.HasShield)
        {
            var maybeShield = secondaryHand.MaybeShield;
            hasFreeHand = maybeShield.Blueprint.Type.ProficiencyGroup == ArmorProficiencyGroup.Buckler;
        }
        else if (secondaryHand.HasWeapon && secondaryHand.MaybeWeapon != Owner.Body.EmptyHandWeapon)
        {
            hasFreeHand = false;
        }
        return hasFreeHand;
    }
}
