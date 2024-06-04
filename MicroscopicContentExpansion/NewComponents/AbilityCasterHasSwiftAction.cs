using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace MicroscopicContentExpansion.NewComponents;
[AllowedOn(typeof(BlueprintAbility), false)]
[AllowMultipleComponents]
[TypeId("08a2ac736acf45fa82b484593d88cab9")]
internal class AbilityCasterHasSwiftAction : BlueprintComponent, IAbilityCasterRestriction
{
    public string GetAbilityCasterRestrictionUIText() => "Caster doesn't have swift action available";

    public bool IsCasterRestrictionPassed(UnitEntityData caster) => caster.HasSwiftAction();
}
