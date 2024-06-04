using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using System.Linq;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.Classes;
internal class MonkFlawlessMind
{
    internal static void AddFlawlessMind()
    {
        var flawlessMind = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MonkFlawlessMind", a =>
        {
            a.SetName(MCEContext, "Flawless Mind");
            a.SetDescription(MCEContext, "At 19th level, a monk gains total control over his mental faculties. Whenever he attempts a Will save, he can roll twice and take the better result.");
            a.AddComponent<ModifyD20>(c =>
            {
                c.Rule = RuleType.SavingThrow;
                c.RollsAmount = 1;
                c.TakeBest = true;
                c.m_SavingThrowType = Kingmaker.RuleSystem.Rules.FlaggedSavingThrowType.Will;
            });
        });
        if (MCEContext.AddedContent.Feats.IsEnabled("MonkFlawlessMind"))
        {
            var monkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");
            var lvlEntries = monkProgression.LevelEntries;
            var lvl19Entry = lvlEntries.FirstOrDefault(x => x.Level == 19);
            if (lvl19Entry != null)
            {
                lvl19Entry.m_Features.Add(flawlessMind.ToReference<BlueprintFeatureBaseReference>());
            }
        }
    }
}
