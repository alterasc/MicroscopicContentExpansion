using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.View.Animation;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.Visual.Animation.Kingmaker.Actions;

namespace MicroscopicContentExpansion.NewComponents;

[ComponentName("SpecialAnimationStateAny")]
[AllowedOn(typeof(BlueprintBuff), false)]
[TypeId("494e9bf94233487f9284c94ba51d94f3")]
public class SpecialAnimationMonkKick : UnitBuffComponentDelegate<SpecialAnimationStateAnyData>,
    IBuffFxComponent {

    public UnitAnimationActionHandAttack Animation;

    public override void OnDeactivate() {
        base.OnDeactivate();
        if (this.Data.Handle == null)
            return;
        this.Data.Handle.Release();
        this.Data.Handle = null;
    }

    void IBuffFxComponent.OnSpawnFx() => this.StartAnimation();

    public void StartAnimation() {
        UnitAnimationManager animationManager = this.Owner.View.AnimationManager;
        if (animationManager == null)
            return;

        var handle = animationManager.CreateHandle(UnitAnimationType.MainHandAttack, true);
        handle.AttackWeaponStyle = WeaponAnimationStyle.MartialArts;
        handle.Variant = 2;
        handle.m_SpeedScale = 0.66f;
        this.Data.Handle = handle;
        animationManager.Execute(Data.Handle);
    }
}

public class SpecialAnimationStateAnyData {
    public UnitAnimationActionHandle Handle;
}