using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;

namespace MicroscopicContentExpansion.RebalancedContent.MythicArmor;

/// <summary>
/// Replacer component for heavy and medium mythic armor offence feats.
/// Uses armor enhancement value as enhancement value of damage
/// </summary>
[AllowMultipleComponents]
[TypeId("5d9f3651218e44cc91f5d7a0092abed7")]
public class ArmorACAsDamage :
    EntityFactComponentDelegate,
    IInitiatorRulebookHandler<RulePrepareDamage>,
    IRulebookHandler<RulePrepareDamage>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public WeaponRangeType RangeType;

    public ContextDiceValue Value;

    public void OnEventAboutToTrigger(RulePrepareDamage evt)
    {
        if (!CheckCondition(evt))
            return;
        UnitPartDisableBonusForDamage disableBonusForDamage = evt.Initiator.Parts.Get<UnitPartDisableBonusForDamage>();
        if (disableBonusForDamage != null && disableBonusForDamage.DisableAdditionalDice)
            return;
        DamageTypeDescription damageTypeDescription = evt.DamageBundle.First.CreateTypeDescription();
        var enhancementBonus = GameHelper.GetItemEnhancementBonus(evt.Initiator.Body.Armor.Armor);
        damageTypeDescription.Physical.Enhancement = enhancementBonus;
        damageTypeDescription.Physical.EnhancementTotal = enhancementBonus;
        int rollsCount = this.Value.DiceCountValue.Calculate(this.Context);
        DamageDescription damageDescription = new()
        {
            TypeDescription = damageTypeDescription,
            Dice = new DiceFormula(rollsCount, this.Value.DiceType),
            Bonus = this.Value.BonusValue.Calculate(this.Context),
            SourceFact = this.Fact
        };
        BaseDamage damage = damageDescription.CreateDamage();
        evt.Add(damage);
    }


    public void OnEventDidTrigger(RulePrepareDamage evt)
    {
    }

    private bool CheckCondition(RulePrepareDamage evt)
    {
        RuleAttackWithWeapon attackWithWeapon = evt.ParentRule.AttackRoll?.RuleAttackWithWeapon;
        if (attackWithWeapon != null && CheckCondition(attackWithWeapon))
            return true;
        return false;
    }

    private bool CheckCondition(RuleAttackWithWeapon evt)
    {
        ItemEntity itemEntity = this.Fact is ItemEnchantment fact ? fact.Owner : null;
        if (itemEntity != null && itemEntity != evt.Weapon
            || !evt.AttackRoll.IsHit
            || !this.RangeType.IsSuitableWeapon(evt.Weapon))
            return false;
        return true;
    }
}

