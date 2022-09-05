using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                MCEContext.Logger.LogHeader("Patching Monk");

                AddOldMaster();

            }
            static void AddOldMaster() {
                var YAFoBAttack = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MonkOldMasterFoB", bp => {
                    bp.AddComponent<BuffExtraAttack>(c => {
                        c.Number = 1;
                        c.Haste = false;
                    });
                    bp.HideInUI = true;
                    bp.IsClassFeature = true;
                    bp.HideInCharacterSheetAndLevelUp = true;
                });

                var AddDodgeBonusBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "MonkOldMasterACBuff", bp => {
                    bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                    bp.SetName(MCEContext, "Old Master");
                    bp.IsClassFeature = true;
                    bp.AddComponent<AddStatBonus>(c => {
                        c.Stat = StatType.AC;
                        c.Descriptor = ModifierDescriptor.Dodge;
                        c.Value = 2;
                    });
                });

                var OldMaster_ACBonus = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MonkOldMasterACFeature", bp => {
                    bp.HideInUI = true;
                    bp.IsClassFeature = true;
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { AddDodgeBonusBuff.ToReference<BlueprintUnitFactReference>() };
                    });
                });

                BlueprintFeature CreateOMForArchetype(string Name, MonkNoArmorAndMonkWeaponFeatureUnlock FlurryUnlock, bool IsSensei = false) {
                    return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, Name, bp => {
                        bp.SetName(MCEContext, "Old Master");
                        bp.SetDescription(MCEContext, "At 20th level, the monk has reached the highest levels of his martial arts school. " +
                                "The monk gains one additional attack at his highest base attack bonus when using flurry of blows, " +
                                "and his dodge bonus to AC increases by 2.");
                        bp.IsClassFeature = true;
                        if (!IsSensei) {
                            bp.AddComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>(c => {
                                c.m_NewFact = YAFoBAttack.ToReference<BlueprintUnitFactReference>();
                                c.IsZenArcher = FlurryUnlock.IsZenArcher;
                                c.m_BowWeaponTypes = FlurryUnlock.m_BowWeaponTypes;
                                c.m_RapidshotBuff = FlurryUnlock.m_RapidshotBuff;
                                c.IsSohei = FlurryUnlock.IsSohei;
                            });
                        }
                        bp.AddComponent<MonkNoArmorFeatureUnlock>(c => {
                            c.m_NewFact = OldMaster_ACBonus.ToReference<BlueprintUnitFactReference>();
                        });
                    });
                }
                var MonkPerfectSelf = BlueprintTools.GetBlueprint<BlueprintFeature>("3854f693180168a4980646aee9494c72");


                BlueprintFeatureSelection CreateCapstoneSelection(string Name, BlueprintFeature OldMaster) {
                    return Helpers.CreateBlueprint<BlueprintFeatureSelection>
                        (MCEContext, Name, bp => {
                            bp.SetName(MCEContext, "Monk Capstone");
                            bp.SetDescription(MCEContext, "At 20th level, the monk gains a powerful class feature");
                            bp.m_AllFeatures = new BlueprintFeatureReference[] {
                                OldMaster.ToReference<BlueprintFeatureReference>(),
                                MonkPerfectSelf.ToReference<BlueprintFeatureReference>()
                            };
                            bp.Mode = SelectionMode.Default;
                            bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                            bp.IsClassFeature = true;
                        });
                }

                var MonkFoBUnlockComponent = BlueprintTools.GetBlueprint<BlueprintFeature>("fd99770e6bd240a4aab70f7af103e56a")
                    .GetComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>();

                var OldMaster = CreateOMForArchetype("MonkOldMasterFeature", MonkFoBUnlockComponent);
                var MonkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");
                var Lvl20 = MonkProgression.LevelEntries.Where(l => l.Level == 20).First();
                BlueprintFeatureSelection MonkCapstoneSelection = CreateCapstoneSelection("MonkCapstoneSelection", OldMaster);

                var Sohei = BlueprintTools.GetBlueprint<BlueprintArchetype>("fad7c56737ed12e42aacc330acc86428");
                var SoheiFoBUnlockComponent = BlueprintTools.GetBlueprint<BlueprintFeature>("cd4381b73b6709146bbcc0a528a6f471")
                    .GetComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>();
                var OldMasterSohei = CreateOMForArchetype("MonkSoheiOldMasterFeature", SoheiFoBUnlockComponent);
                var MonkSoheiCapstoneSelection = CreateCapstoneSelection("MonkSoheiCapstoneSelection", OldMasterSohei);

                var ZenArcher = BlueprintTools.GetBlueprint<BlueprintArchetype>("2b1a58a7917084f49b097e86271df21c");
                var ZenArcherFoBUnlockComponent = BlueprintTools.GetBlueprint<BlueprintFeature>("3e470edc8a733b641bcbbbb5b9527ff6")
                    .GetComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>();
                var OldMasterZenArcher = CreateOMForArchetype("MonkZenArcherOldMasterFeature", ZenArcherFoBUnlockComponent);
                var MonkZenArcherCapstoneSelection = CreateCapstoneSelection("MonkZenArcherCapstoneSelection", OldMasterZenArcher);

                var Sensei = BlueprintTools.GetBlueprint<BlueprintArchetype>("f8767821ec805bf479706392fcc3394c");
                var OldMasterSensei = CreateOMForArchetype("MonkSenseiOldMasterFeature", MonkFoBUnlockComponent, IsSensei: true);
                var MonkSenseiCapstoneSelection = CreateCapstoneSelection("MonkSenseiCapstoneSelection", OldMasterSensei);

                var QuarterstaffMaster = BlueprintTools.GetBlueprint<BlueprintArchetype>("dde7724382ae4f63aa9786cb9b3b64b2");
                var QMFoBUnlockComponent = BlueprintTools.GetBlueprint<BlueprintFeature>("44b0f313ec56481eb447019fbe714330")
                    .GetComponent<MonkNoArmorAndMonkWeaponFeatureUnlock>();
                var OldMasterQM = CreateOMForArchetype("MonkQMOldMasterFeature", QMFoBUnlockComponent);
                var MonkQMCapstoneSelection = CreateCapstoneSelection("MonkQMCapstoneSelection", OldMasterQM);                
            }
        }


        private static void RemoveFeatureFromArchetype(BlueprintArchetype Archetype, int Level, BlueprintFeatureSelection BlueprintFeatureSelection) {
            RemoveFeatureFromArchetype(Archetype, Level, BlueprintFeatureSelection.ToReference<BlueprintFeatureBaseReference>());
        }

        private static void RemoveFeatureFromArchetype(BlueprintArchetype Archetype, int Level, BlueprintFeatureBaseReference Reference) {
            var Lvl20Entry = Archetype.RemoveFeatures.FirstOrDefault(l => l.Level == Level);
            if (Lvl20Entry != null) {
                Lvl20Entry.m_Features.Add(Reference);
            } else {
                Archetype.RemoveFeatures = Archetype.RemoveFeatures.AddToArray(Helpers.CreateLevelEntry(Level, Reference));
            }
        }

        private static void AddFeatureToArchetype(BlueprintArchetype Archetype, int Level, BlueprintFeatureSelection BlueprintFeatureSelection) {
            AddFeatureToArchetype(Archetype, Level, BlueprintFeatureSelection.ToReference<BlueprintFeatureBaseReference>());
        }

        private static void AddFeatureToArchetype(BlueprintArchetype Archetype, int Level, BlueprintFeatureBaseReference Reference) {
            var Lvl20Entry = Archetype.AddFeatures.FirstOrDefault(l => l.Level == Level);
            if (Lvl20Entry != null) {
                Lvl20Entry.m_Features.Add(Reference);
            } else {
                Archetype.AddFeatures = Archetype.AddFeatures.AddToArray(Helpers.CreateLevelEntry(Level, Reference));
            }
        }
    }
}
