using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Archetypes;
internal class KnightoftheSepulcher
{
    private const string NAME = "Knight of the Sepulcher";
    private const string DESCRIPTION = "Not content with the antipaladin’s mere corruption of the soul, the knight of the sepulcher sacrifices mortality along with morality.";

    public static void AddKnightoftheSepulcher()
    {
        var antipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");

        var fiendishBoon = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinFiendishBoonSelection");
        var auraOfDespair = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfDespairFeature");
        var smiteGoodAdditionalUse = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinSmiteGoodAdditionalUse");
        var auraOfSin = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfSinFeature");
        var crueltySelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltySelection");
        var auraOfDepravity = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfDepravityFeature");
        var capstone = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCapstone");


        var touchoftheCrypt = TouchoftheCrypt.AddTouchoftheCrypt();
        var fortitudeoftheCrypt = FortitudeoftheCrypt.AddFortitudeoftheCrypt();
        var cloakoftheCrypt = CloakoftheCrypt.AddCloakoftheCrypt();
        var willoftheCrypt = WilloftheCrypt.AddWilloftheCrypt();
        var weaponsofSin = WeaponsofSin.AddWeaponsofSin();
        var cryptLord = CryptLord.AddCryptLord();
        var souloftheCrypt = SouloftheCrypt.AddSouloftheCrypt();
        var undyingChampion = UndyingChampion.AddUndyingChampion();

        var knightoftheSepulcher = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "KnightoftheSepulcherArchetype", bp =>
        {
            bp.LocalizedName = Helpers.CreateString(MCEContext, $"{bp.name}.Name", NAME);
            bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
            bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
            bp.RemoveFeatures = [
                Helpers.CreateLevelEntry(5, fiendishBoon),
                Helpers.CreateLevelEntry(8, auraOfDespair),
                Helpers.CreateLevelEntry(10, smiteGoodAdditionalUse),
                Helpers.CreateLevelEntry(14, auraOfSin),
                Helpers.CreateLevelEntry(15, crueltySelection),
                Helpers.CreateLevelEntry(17, auraOfDepravity),
                Helpers.CreateLevelEntry(20, capstone)
            ];
            bp.AddFeatures = [
                Helpers.CreateLevelEntry(5, touchoftheCrypt),
                Helpers.CreateLevelEntry(8, fortitudeoftheCrypt),
                Helpers.CreateLevelEntry(10, cloakoftheCrypt),
                Helpers.CreateLevelEntry(14, weaponsofSin),
                Helpers.CreateLevelEntry(15, cryptLord),
                Helpers.CreateLevelEntry(17, souloftheCrypt),
                Helpers.CreateLevelEntry(20, undyingChampion)
            ];
        });

        var archetypes = antipaladinClass.m_Archetypes.AppendToArray(knightoftheSepulcher.ToReference<BlueprintArchetypeReference>());
        antipaladinClass.m_Archetypes = archetypes;

        var KotSUIGroup = Helpers.CreateUIGroup(touchoftheCrypt, fortitudeoftheCrypt, cloakoftheCrypt,
            willoftheCrypt, weaponsofSin, cryptLord, souloftheCrypt, undyingChampion);
        var antipaladinProgression = BlueprintTools.GetModBlueprint<BlueprintProgression>(MCEContext, "AntipaladinProgression");
        antipaladinProgression.UIGroups = antipaladinProgression.UIGroups.AppendToArray(KotSUIGroup);

    }

}
