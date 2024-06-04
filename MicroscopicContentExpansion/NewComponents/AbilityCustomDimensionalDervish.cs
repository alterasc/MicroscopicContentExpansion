using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MicroscopicContentExpansion.NewComponents;
[TypeId("3eda209f2af8431ab8093f6ceb4e73e8")]
internal class AbilityCustomDimensionalDervish : AbilityCustomLogic
{
    public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
    {
        var caster = context.MaybeCaster;

        if (caster == null)
        {
            PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
            yield break;
        }
        var threatHand = caster.GetThreatHandMelee();
        if (threatHand == null)
        {
            PFLog.Default.Error("Caster can't attack", Array.Empty<object>());
            yield break;
        }
        var originalTarget = target.Unit;
        if (originalTarget == null)
        {
            PFLog.Default.Error("Can't be applied to point", Array.Empty<object>());
            yield break;
        }
        caster.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, true, 0);
        //first we attack our main target
        var totalRange = caster.CombatSpeedMps * 6f;
        UnitEntityData maybeCaster = context.MaybeCaster;
        if (!maybeCaster.IsReach(originalTarget, threatHand))
        {
            Vector3 target1 = originalTarget.Position;
            var casterPos = caster.Position;

            var handRange = caster.GetThreatHandMelee().Weapon.AttackRange.Meters;

            var dist = Vector3.Distance(casterPos, target1) - handRange / 2;
            totalRange -= dist;
            var tpTarget = Vector3.MoveTowards(casterPos, target1, dist);

            caster.ForceLookAt(target1);
            caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
            caster.Position = tpTarget;
        }
        // then setup TeleportAfterAttackToNextTarget handler that will teleport to closest enemy if no targets in reach
        // and perform full attack
        var eventHandlers = new EventHandlers();
        eventHandlers.Add(new TeleportAfterAttackToNextTarget(caster, totalRange));

        using (eventHandlers.Activate())
        {
            UnitAttack cmd = new UnitAttack(target.Unit)
            {
                ForceFullAttack = true
            };
            cmd.Init(context.Caster);
            cmd.Start();

            while (!cmd.IsFinished)
            {
                bool wasActed = cmd.IsActed;
                if (!cmd.IsFinished)
                    cmd.Tick();
                if ((wasActed ? 0 : (cmd.IsActed ? 1 : 0)) != 0)
                {
                    yield return new AbilityDeliveryTarget(target);
                }
                yield return null;
            }
        }
    }

    public override void Cleanup(AbilityExecutionContext context)
    {
    }

    private class EventHandlers : IDisposable
    {
        private readonly List<object> m_Handlers = new List<object>();

        public void Add(object handler) => this.m_Handlers.Add(handler);

        public EventHandlers Activate()
        {
            foreach (object handler in this.m_Handlers)
                EventBus.Subscribe(handler);
            return this;
        }

        public void Dispose()
        {
            foreach (object handler in this.m_Handlers)
                EventBus.Unsubscribe(handler);
        }
    }

    internal static void TeleportToTarget(UnitEntityData caster, UnitEntityData target)
    {
        Vector3 target1 = target.Position;
        var casterPos = caster.Position;

        var handRange = caster.GetThreatHandMelee().Weapon.AttackRange.Meters;

        var dist = Vector3.Distance(casterPos, target1) - handRange / 2;
        var tpTarget = Vector3.MoveTowards(casterPos, target1, dist);

        caster.ForceLookAt(target1);
        caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
        caster.Position = tpTarget;
    }

    public override bool IsEngageUnit => true;
}




[TypeId("461bb5f8eb0b418da9a4b1bd4003c92c")]
public class TeleportAfterAttackToNextTarget :
    IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>,
    IRulebookHandler<RuleAttackWithWeaponResolve>,
    ISubscriber,
    IInitiatorRulebookSubscriber
{
    public UnitEntityData GetSubscribingUnit()
    {
        return this.m_Unit;
    }

    private readonly UnitEntityData m_Unit;
    private float distance;

    public TeleportAfterAttackToNextTarget(UnitEntityData unit, float distance)
    {
        m_Unit = unit;
        this.distance = distance;
    }

    public void OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
    {

    }


    public void OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
    {
        WeaponSlot threatHand = evt.Initiator.GetThreatHandMelee();
        var list = new List<UnitEntityData> { };
        foreach (UnitGroupMemory.UnitInfo enemy in evt.Initiator.Memory.Enemies)
        {
            UnitEntityData unit = enemy.Unit;
            if (unit.HPLeft > 0 && evt.Initiator.IsReach(unit, threatHand)) return;
        }
        foreach (UnitGroupMemory.UnitInfo unitInfo in evt.Initiator.Memory.Enemies)
        {
            if (unitInfo.Unit.HPLeft > 0 && unitInfo.Unit.Descriptor.State.IsConscious)
                list.Add(unitInfo.Unit);
        }
        list.Sort((u1, u2) => evt.Initiator.DistanceTo(u1).CompareTo(evt.Initiator.DistanceTo(u2)));
        if (list.Count == 0)
            return;
        var closest = list.First();

        Vector3 target1 = closest.Position;
        var casterPos = evt.Initiator.Position;

        var handRange = evt.Initiator.GetThreatHandMelee().Weapon.AttackRange.Meters;

        var dist = Vector3.Distance(casterPos, target1) - handRange / 2;
        if (dist < this.distance)
        {
            this.distance -= dist;
            var tpTarget = Vector3.MoveTowards(casterPos, target1, dist);

            evt.Initiator.ForceLookAt(target1);
            evt.Initiator.CombatState.PreventAttacksOfOpporunityNextFrame = true;
            evt.Initiator.Position = tpTarget;
        }
    }
}
