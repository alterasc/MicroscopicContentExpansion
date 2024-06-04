using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace MicroscopicContentExpansion.NewComponents;
[TypeId("42a0a56bec4a47df89aa6e8b7ddd8157")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AuraFeatureComponentWithWiden :
    UnitFactComponentDelegate<AuraFeatureComponentData>,
    IUnitLifeStateChanged,
    IGlobalSubscriber,
    ISubscriber,
    IUnitGainFactHandler,
    IUnitLostFactHandler
{

    public BlueprintBuffReference DefaultBuff;
    public BlueprintUnitFactReference WidenFact;
    public BlueprintBuffReference WidenBuff;
    public BlueprintBuff BuffToApply => Owner.HasFact(WidenFact) ? WidenBuff?.Get() : DefaultBuff?.Get();

    public override void OnActivate() => Data.AppliedBuff = Owner.AddBuff(BuffToApply, Fact.MaybeContext);

    public override void OnDeactivate()
    {
        Data.AppliedBuff?.Remove();
        Data.AppliedBuff = null;
    }

    public void HandleUnitLifeStateChanged(UnitEntityData unit, UnitLifeState prevLifeState)
    {
        if (unit == (UnitDescriptor)Owner && (prevLifeState == UnitLifeState.Dead || prevLifeState == UnitLifeState.Unconscious) && prevLifeState != UnitLifeState.Conscious && Data.AppliedBuff == null)
            Data.AppliedBuff = Owner.AddBuff(BuffToApply, Fact.MaybeContext);
        if (!(unit == (UnitDescriptor)Owner) || prevLifeState == UnitLifeState.Dead || prevLifeState == UnitLifeState.Unconscious || prevLifeState != UnitLifeState.Conscious)
            return;
        Data.AppliedBuff?.Remove();
        Data.AppliedBuff = null;
    }

    public void HandleUnitGainFact(EntityFact fact)
    {
        if (!WidenFact.Is(fact.Blueprint as BlueprintUnitFact)) return;
        if (Data.AppliedBuff != null)
        {
            Data.AppliedBuff?.Remove();
            Data.AppliedBuff = Owner.AddBuff(BuffToApply, Fact.MaybeContext);
        }
    }

    public void HandleUnitLostFact(EntityFact fact)
    {
        if (!WidenFact.Is(fact.Blueprint as BlueprintUnitFact)) return;
        if (Data.AppliedBuff != null)
        {
            Data.AppliedBuff?.Remove();
            Data.AppliedBuff = Owner.AddBuff(BuffToApply, Fact.MaybeContext);
        }
    }
}
