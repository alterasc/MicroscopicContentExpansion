using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;

namespace MicroscopicContentExpansion.NewComponents;

[ComponentName("Add feature depending on class level")]
[AllowMultipleComponents]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[TypeId("1981eee498aa42ce92459d460b9681b9")]
public class AddUnitFactDependingOnClassLevel :
    UnitFactComponentDelegate<AddFeatureDependingOnClassLevelData>,
    IOwnerGainLevelHandler,
    IUnitSubscriber,
    ISubscriber
{

    [SerializeField]
    public BlueprintCharacterClassReference m_Class;

    [SerializeField]
    public BlueprintUnitFactReference[] unitFactArray;

    [SerializeField]
    public BlueprintCharacterClassReference[] m_AdditionalClasses;

    [SerializeField]
    public BlueprintArchetypeReference[] m_Archetypes;

    public BlueprintCharacterClass Class => this.m_Class?.Get();

    public ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> AdditionalClasses => (ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference>)this.m_AdditionalClasses;

    public ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference> Archetypes => (ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference>)this.m_Archetypes;

    public override void OnActivate() => this.Apply();

    public override void OnDeactivate()
    {
        Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Fact not null {this.Data.AppliedFact != null}");
        this.Owner.RemoveFact(this.Data.AppliedFact);
        this.Data.AppliedFact = null;
        this.Data.AppliedFactReference = null;
    }

    public void HandleUnitGainLevel() => this.Apply();

    private void Apply()
    {
        var featureToApply = IsFeatureShouldBeApplied();

        Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Will add guid {featureToApply.Guid}");
        if (this.Data.AppliedFact != null)
        {
            Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Applied Fact present");
            if (this.Data.AppliedFactReference.Equals(featureToApply))
            {
                Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Applied Fact match");
                return;
            }
            else
            {
                this.Owner.RemoveFact(this.Data.AppliedFact);
                this.Data.AppliedFact = null;
                this.Data.AppliedFactReference = null;
                Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Removed Fact");
            }
        }
        var feature = featureToApply.Get();
        Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Feature not null {feature != null}");
        UnitFact fact2 = feature.CreateFact(null, this.Owner.Descriptor, new TimeSpan?());
        Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: fact2 not null {fact2 != null}");
        UnitFact fact3 = this.Owner.Facts.Add(fact2);
        this.Data.AppliedFact = fact3;

        this.Data.AppliedFactReference = featureToApply;
        Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: Added new Fact {this.Data.AppliedFact != null}");
    }

    private BlueprintUnitFactReference IsFeatureShouldBeApplied()
    {
        int classLevel = ReplaceCasterLevelOfAbility.CalculateClassLevel(this.Class, this.AdditionalClasses.ToArray(), (UnitDescriptor)this.Owner, this.Archetypes.ToArray());
        var idx = classLevel / 3;
        Main.MCEContext.Logger.Log($"IsFeatureShouldBeApplied: {idx}");
        if (idx > unitFactArray.Length)
        {
            return unitFactArray[unitFactArray.Length - 1];
        }
        return unitFactArray[idx - 1];

    }
}

public class AddFeatureDependingOnClassLevelData
{
    [JsonProperty]
    public EntityFact AppliedFact;
    [JsonProperty]
    public BlueprintUnitFactReference AppliedFactReference;
}