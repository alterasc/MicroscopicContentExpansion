using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroscopicContentExpansion.NewComponents {
    [TypeId("29cfc39737054920bdef50529a521699")]
    internal class AbilityCustomDimensionalAssault : AbilityCustomLogic {

        public override bool IsEngageUnit => true;
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            var caster = context.MaybeCaster;

            if (caster == null) {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                yield break;
            }
            var threatHand = caster.GetThreatHandMelee();
            if (threatHand == null) {
                PFLog.Default.Error("Caster can't attack", Array.Empty<object>());
                yield break;
            }
            var originalTarget = target.Unit;
            if (originalTarget == null) {
                PFLog.Default.Error("Can't be applied to point", Array.Empty<object>());
                yield break;
            }
            if (!caster.IsReach(originalTarget, threatHand)) {
                Vector3 target1 = originalTarget.Position;
                var casterPos = caster.Position;

                var handRange = caster.GetThreatHandMelee().Weapon.AttackRange.Meters;

                var dist = Vector3.Distance(casterPos, target1) - handRange / 2;

                var tpTarget = Vector3.MoveTowards(casterPos, target1, dist);

                caster.ForceLookAt(target1);
                caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                caster.Position = tpTarget;
            }
            caster.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, context, new TimeSpan?(1.Rounds().Seconds));
            caster.Descriptor.State.IsCharging = true;

            UnitAttack attack = new UnitAttack(target.Unit) {
                IsCharge = true
            };
            attack.IgnoreCooldown();
            attack.Init(caster);
            attack.Start();
            caster.Commands.AddToQueueFirst(attack);
        }

        public override void Cleanup(AbilityExecutionContext context) {
            context.Caster.Descriptor.State.IsCharging = false;
        }
    }
}
