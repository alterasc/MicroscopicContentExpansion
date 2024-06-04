using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.View;
using Kingmaker.Visual.Animation.Kingmaker;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MicroscopicContentExpansion.NewComponents;

[TypeId("7d967f2e6e4043c2a490963bb6d40027")]
public class AbilityCustomFlyingKick : AbilityCustomLogic,
    IAbilityTargetRestriction,
    IAbilityMinRangeProvider
{

    [SerializeField]
    public BlueprintBuffReference m_AddBuffWhileRunning;

    public BlueprintBuffReference m_FlurrySuppressionBuff;

    public BlueprintBuff AddBuffWhileRunning => this.m_AddBuffWhileRunning?.Get();

    public override IEnumerator<AbilityDeliveryTarget> Deliver(
      AbilityExecutionContext context,
      TargetWrapper target)
    {
        UnitEntityData caster = context.Caster;
        Vector3 startPoint = caster.Position;
        var originalTarget = target.Unit;

        if (originalTarget == null)
        {
            PFLog.Default.Error("Can't be applied to point", []);
            yield break;
        }
        Vector3 originalTargetPosition = originalTarget.Position;

        var handRange = caster.GetThreatHandMelee().Weapon.AttackRange.Meters;

        var endPoint = GetLandingEndpoint(caster, originalTarget, handRange);

        caster.View.StopMoving();

        if (caster.View.AnimationManager?.CurrentAction is UnitAnimationActionHandle currentAction)
            currentAction.DoesNotPreventMovement = true;
        else
            PFLog.Default.Error("No animation handle found");

        caster.Descriptor.AddBuff(this.AddBuffWhileRunning, context);

        var startTime = Game.Instance.TimeController.GameTime;
        float duration = 0.75f;
        float d = 0.0f;

        while (d <= duration)
        {
            if (Game.Instance.TurnBasedCombatController.WaitingForUI)
            {
                yield return null;
            }
            else
            {
                d = (float)(Game.Instance.TimeController.GameTime - startTime).TotalSeconds;
                var passed = d / duration;

                if (originalTarget.Position != originalTargetPosition)
                {
                    endPoint = GetLandingEndpoint(caster, originalTarget, handRange);
                }

                var position = Vector3.Lerp(startPoint, endPoint, passed);
                caster.Position = position;
                caster.FlyHeight = (passed - passed * passed) * 4f;
                yield return null;
            }
        }
        caster.FlyHeight = 0f;
        caster.Position = endPoint;

        context.Caster.Descriptor.RemoveFact(AddBuffWhileRunning);

        caster.FlyHeight = 0f;
        UnitPlaceOnGroundController.ForcedTick(caster);

        if (!originalTarget.State.IsDead)
        {
            RuleAttackWithWeapon attackWithWeapon = new RuleAttackWithWeapon(caster, originalTarget, caster.GetThreatHandMelee().Weapon, 0);
            context.TriggerRule(attackWithWeapon);
        }
        caster.Descriptor.AddBuff(m_FlurrySuppressionBuff, context, TimeSpan.FromSeconds(1));
        var flurryTarget = originalTarget;
        if (!flurryTarget.State.IsDead)
        {
            var threatHand = caster.GetThreatHandMelee();
            var list = new List<UnitEntityData> { };

            foreach (UnitGroupMemory.UnitInfo enemy in caster.Memory.Enemies)
            {
                UnitEntityData unit = enemy.Unit;
                if (unit.HPLeft > 0 && caster.IsReach(unit, threatHand))
                {
                    flurryTarget = unit;
                    break;
                }
            }
        }
        if (flurryTarget != null)
        {
            UnitAttack cmd = new UnitAttack(target.Unit)
            {
                ForceFullAttack = true
            };
            cmd.IgnoreCooldown();
            cmd.Init(caster);
            caster.Commands.AddToQueueFirst(cmd);
        }
    }

    public override void Cleanup(AbilityExecutionContext context)
    {
        context.Caster.Descriptor.RemoveFact(AddBuffWhileRunning);
    }

    private static Vector3 GetLandingEndpoint(UnitEntityData caster, UnitEntityData target, float handRange)
    {

        Vector3 startPoint = caster.Position;
        Vector3 targetP = target.Position;
        var dist = Vector3.Distance(startPoint, targetP) - handRange - caster.Corpulence - target.Corpulence;
        return Vector3.MoveTowards(startPoint, targetP, dist);
    }

    public float GetMinRangeMeters(UnitEntityData caster) => 5.Feet().Meters + caster.View.Corpulence;

    public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper targetWrapper) => this.CheckTargetRestriction(caster, targetWrapper, out LocalizedString _);

    public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
    {
        CheckTargetRestriction(caster, target, out LocalizedString failReason);
        return failReason == null ? string.Empty : (string)failReason;
    }

    private bool CheckTargetRestriction(
      UnitEntityData caster,
      TargetWrapper targetWrapper,
      [CanBeNull] out LocalizedString failReason)
    {

        float magnitude = (targetWrapper.Point - caster.Position).magnitude;
        if ((double)magnitude > (double)AbilityCustomOverrun.GetMaxRangeMeters(caster))
        {
            failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooFar;
            return false;
        }
        if (magnitude < GetMinRangeMeters(caster))
        {
            failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooClose;
            return false;
        }

        if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, targetWrapper.Point) != targetWrapper.Point)
        {
            failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
            return false;
        }
        failReason = null;
        return true;
    }

    public override bool IsEngageUnit => true;
}
