using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MicroscopicContentExpansion.NewComponents {
    [TypeId("b0880fc55e33413db74fe74d0f4ef3aa")]
    public class ContextActionArmorEnchantPoolMCE : ContextAction {
        public EnchantPoolType EnchantPool;
        public ActivatableAbilityGroup Group;
        [SerializeField]
        [FormerlySerializedAs("DefaultEnchantments")]
        public BlueprintItemEnchantmentReference[] m_DefaultEnchantments = new BlueprintItemEnchantmentReference[5];
        public ContextDurationValue DurationValue;

        public override string GetCaption() => string.Format("Add enchants from pool to caster's armor (for {0})", DurationValue);

        public override void RunAction() {
            UnitEntityData maybeCaster = this.Context.MaybeCaster;
            if (maybeCaster == null) {
                LogChannel.Default.Error(this, "ContextActionArmorEnchantPool: target is null");
            } else {
                UnitPartEnchantPoolData partEnchantPoolData = maybeCaster.Ensure<UnitPartEnchantPoolData>();
                partEnchantPoolData.ClearEnchantPool(this.EnchantPool);
                ItemEntityArmor maybeArmor = maybeCaster.Body.Armor.MaybeArmor;
                if (maybeArmor == null)
                    return;
                int num1 = 0;
                int groupSize = maybeCaster.Ensure<UnitPartActivatableAbility>().GetGroupSize(this.Group);
                if (maybeArmor.Enchantments.Any()) {
                    num1 = maybeArmor.Enchantments.SelectMany(e => e.SelectComponents<ArmorEnhancementBonus>())
                        .Select(e => e.EnhancementValue)
                        .DefaultIfEmpty(0)
                        .Max();
                }
                Rounds duration = this.DurationValue.Calculate(this.Context);
                foreach (AddBondProperty selectFactComponent in maybeCaster.Buffs.SelectFactComponents<AddBondProperty>()) {
                    if (selectFactComponent.EnchantPool == this.EnchantPool && !maybeArmor.HasEnchantment(selectFactComponent.Enchant)) {
                        groupSize -= selectFactComponent.Enchant.EnchantmentCost;
                        partEnchantPoolData.AddEnchant(maybeArmor, this.EnchantPool, selectFactComponent.Enchant, this.Context, duration);
                    }
                }
                int num2 = Math.Min(Math.Max(groupSize, 0) + num1, 5);
                if (num2 <= 0)
                    return;
                BlueprintItemEnchantment defaultEnchantment = this.m_DefaultEnchantments[num2 - 1];
                partEnchantPoolData.AddEnchant(maybeArmor, this.EnchantPool, defaultEnchantment, this.Context, duration);
            }
        }
    }
}
