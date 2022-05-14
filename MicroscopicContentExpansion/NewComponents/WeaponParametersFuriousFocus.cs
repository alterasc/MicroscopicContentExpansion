using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using System;

namespace MicroscopicContentExpansion.NewComponents {
    public class WeaponParametersFuriousFocus :
      UnitFactComponentDelegate,
      IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
      IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
      ISubscriber,
      IInitiatorRulebookSubscriber {

        private Guid powerAttackBuff = Guid.Parse("5898bcf75a0942449a5dc16adc97b279");
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
            if (evt.Weapon == null) {
                return;
            }
            RulebookEvent rule = evt.Reason.Rule;
            if (rule == null || rule is not RuleAttackWithWeapon) {
                return;
            }
            RuleAttackWithWeapon attackRule = rule as RuleAttackWithWeapon;
            if (attackRule.IsAttackOfOpportunity || !attackRule.IsFirstAttack) {
                return;
            }

            var furiousFocusBonusValue = base.Owner.Stats.BaseAttackBonus.ModifiedValue / 4 + 1;

            foreach (Buff buff in evt.Initiator.Descriptor.Buffs) {
                if (buff.Blueprint.AssetGuid.m_Guid.Equals(powerAttackBuff)) {
                    evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalAttackBonus.AddModifier(furiousFocusBonusValue, Fact, ModifierDescriptor.UntypedStackable));
                    return;
                }
            }

        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) {
        }
    }
}
