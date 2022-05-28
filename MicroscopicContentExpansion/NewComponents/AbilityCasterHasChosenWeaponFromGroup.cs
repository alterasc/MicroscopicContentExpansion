using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System.Linq;

namespace MicroscopicContentExpansion.NewComponents {
    [AllowedOn(typeof(BlueprintAbility), false)]
    [AllowMultipleComponents]
    public class AbilityCasterHasChosenWeaponFromGroup : BlueprintComponent, IAbilityCasterRestriction {
        public BlueprintParametrizedFeatureReference ChosenWeaponFeature;
        public WeaponFighterGroup WeaponGroup;
        public BlueprintFeatureReference WeaponGroupReference;

        public bool IsCasterRestrictionPassed(UnitEntityData caster) {
            if (this.HasSuitableWeapon(caster.Body.PrimaryHand))
                return true;
            return false;
        }

        private bool HasSuitableWeapon(WeaponSlot slot) {
            return slot.MaybeWeapon != null
                && slot.MaybeWeapon.Blueprint.IsRanged
                && slot.MaybeWeapon.Blueprint.FighterGroup.Contains(WeaponGroup)
                && (slot.Owner.GetFeature(ChosenWeaponFeature, (FeatureParam)slot.MaybeWeapon.Blueprint.Category) != null
                    || slot.Owner.HasFact(WeaponGroupReference))
                ;
        }
        public string GetAbilityCasterRestrictionUIText() => (string)LocalizedTexts.Instance.Reasons.ChosenWeaponRequired;
    }
}
