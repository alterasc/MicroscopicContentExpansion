using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using System.Collections.Generic;
using System.Linq;

namespace MicroscopicContentExpansion.RebalancedContent.DragonbloodShifterArchetype;

[TypeId("268eb90086c44966b5eae821e050c555")]
public class DragonbloodWingController : UnitFactComponentDelegate<WingData>, IActivatableAbilityWillStopHandler, IGlobalSubscriber, ISubscriber, IActivatableAbilityStartHandler
{
    public Dictionary<BlueprintGuid, BlueprintBuffReference> MatchingWingsDictionary;

    public override void OnActivate()
    {
        UpdateWings();
    }

    public override void OnTurnOn()
    {
        UpdateWings();
    }

    public override void OnDeactivate()
    {
        if (this.Data.WingBuff != null)
        {
            this.Owner.RemoveFact(this.Data.WingBuff);
            this.Data.WingBuff = null;
        }
    }

    public override void OnTurnOff()
    {
        if (this.Data.WingBuff != null)
        {
            this.Owner.RemoveFact(this.Data.WingBuff);
            this.Data.WingBuff = null;
        }
    }

    public void HandleActivatableAbilityStart(ActivatableAbility ability)
    {
        this.TryRunActions(ability);
    }

    public void HandleActivatableAbilityWillStop(ActivatableAbility ability)
    {
        this.TryRunActions(ability);
    }

    private void TryRunActions(ActivatableAbility ability)
    {
        if (ability.Blueprint.Group != ActivatableAbilityGroup.ShifterAspect)
        {
            return;
        }
        if (ability.Owner != base.Owner)
        {
            return;
        }

        UpdateWings();
    }

    private void UpdateWings()
    {
        ActivatableAbility lastActivatedAspect = base.Owner.ActivatableAbilities.Enumerable
                    .Where(a => a.Blueprint.Group == ActivatableAbilityGroup.ShifterAspect && a.IsOn)
                    .OrderByDescending(x => x.m_TurnOnTime)
                    .FirstOrDefault();

        if (lastActivatedAspect == default)
        {
            if (this.Data.WingBuff != null)
            {
                Owner.RemoveFact(this.Data.WingBuff);
                this.Data.WingBuff = null;
            }
        }
        else
        {
            var abilityBlueprintGuid = lastActivatedAspect.Blueprint.AssetGuid;
            var matchingBuff = MatchingWingsDictionary[abilityBlueprintGuid];
            if (this.Data.WingBuff != null)
            {
                if (this.Data.WingBuff.Blueprint.AssetGuid != matchingBuff.Guid)
                {
                    Owner.RemoveFact(this.Data.WingBuff);
                    this.Data.WingBuff = null;
                }
                else
                {
                    return;
                }
            }
            this.Data.WingBuff = Owner.AddBuff(matchingBuff.Get(), Context);
        }
    }
}

public class WingData
{
    public Buff WingBuff;
}