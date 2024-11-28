using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Localization.Shared;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Classes;
internal class CrusaderLegionBlessing
{
    internal static void Add()
    {
        var clericClass = GetBPRef<BlueprintCharacterClassReference>("67819271767a9dd4fbfd4ae700befea0");
        var crusaderArchetype = GetBP<BlueprintArchetype>("6bfb7e74b530f3749b590286dd2b9b30");

        var icon = GetBP<BlueprintAbility>("2427f2e3ca22ae54ea7337bbab555b16").m_Icon;


        List<BlueprintBuffReference> buffs = new();
        var buffDescription = Helpers.CreateString(MCEContext, "LegionBlessingBuff.Description",
            "Next time you cast on ally spell of this level, it also gets applied on all allies around you in 10 ft radius.", Locale.enGB);
        for (int i = 1; i <= 6; i++)
        {
            var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, $"LegionBlessingBuff{i}", bp =>
            {
                bp.SetName(MCEContext, $"Legion's Blessing - Spell Level {i}");
                bp.m_Description = buffDescription;
                bp.m_Icon = icon;
                bp.m_Flags = BlueprintBuff.Flags.RemoveOnRest;
                bp.AddComponent<LegionBlessingApplySpell>(c =>
                {
                    c.SpellbookReference = crusaderArchetype.m_ReplaceSpellbook;
                    c.SpellLevel = i;
                });
            });
            buffs.Add(buff.ToReference<BlueprintBuffReference>());
        };


        var sacrificeSpellAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "LegionBlessingSacrificeSpellAbility", bp =>
        {
            bp.SetName(MCEContext, "Legion's Blessing - Sacrifice Spell");
            bp.SetDescription(MCEContext, "You sacrifice prepared spell. Next beneficial spell with " +
                "a range of touch prepared in a slot 3 spell levels lower will be applied not only to target, " +
                "but also to all allies around in 10 ft radius.");
            bp.m_Icon = icon;
            bp.AddComponent<LegionBlessingSacrificeSpellAbility>(c =>
            {
                c.LegionBlessingBuffs = buffs.ToArray();
            });
            bp.Type = AbilityType.Supernatural;
            bp.Range = AbilityRange.Personal;
            bp.CanTargetSelf = true;
            bp.LocalizedSavingThrow = new();
            bp.LocalizedDuration = new();
            bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
            bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
        });



        var legionBlessing = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "LegionBlessingFeature", bp =>
        {
            bp.SetName(MCEContext, "Legion's Blessing");
            bp.SetDescription(MCEContext, "At 8th level, a crusader gains the ability to confer beneficial " +
                "spells quickly to a large group of allies. The crusader may confer " +
                "the effects of a single harmless spell with a range of touch to all allies around in 10 ft radius. " +
                "Using the legion’s blessing expends the prepared spell, but it also requires the crusader to " +
                "sacrifice another prepared spell three levels higher, as when spontaneously using a cure " +
                "or inflict spell. The higher-level spell is not cast but is simply lost, its magical " +
                "energy used to power the legion’s blessing.");
            bp.m_Icon = icon;
            bp.AddComponent<SpontaneousSpellConversion>(c =>
            {
                c.m_CharacterClass = clericClass;
                var nullRef = new BlueprintAbilityReference() { deserializedGuid = BlueprintGuid.Empty };
                var abilityRef = sacrificeSpellAbility.ToReference<BlueprintAbilityReference>();
                c.m_SpellsByLevel = [
                    nullRef, //0
                    nullRef, //1
                    nullRef, //2
                    nullRef, //3
                    abilityRef, //4
                    abilityRef, //5
                    abilityRef, //6
                    abilityRef, //7
                    abilityRef, //8
                    abilityRef  //9
                ];
            });
        });

        if (MCEContext.AddedContent.NewClasses.IsDisabled("CrusaderLegionBlessing")) return;

        var features = crusaderArchetype.AddFeatures;
        var lvl8Feature = features.Where(x => x.Level == 8).FirstOrDefault();
        if (lvl8Feature != null)
        {
            lvl8Feature.m_Features.Add(legionBlessing.ToReference<BlueprintFeatureBaseReference>());
        }
        else
        {
            lvl8Feature = Helpers.CreateLevelEntry(8, legionBlessing);
            crusaderArchetype.AddFeatures = features.AddToArray(lvl8Feature);
        }
    }
}
