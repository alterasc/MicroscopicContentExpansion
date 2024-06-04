using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Modding;
using Kingmaker.View;
using Kingmaker.Visual.Mounts;
using Owlcat.Runtime.Core.Utils;
using System.Linq;
using UnityEngine;

namespace MicroscopicContentExpansion.NewContent.AnimalCompanions;

[HarmonyPatch(typeof(OwlcatModificationsManager), nameof(OwlcatModificationsManager.OnResourceLoaded))]
public static class PatchResourcesOnLoad
{

    [HarmonyPrefix]
    public static void Prefix(object resource, string guid)
    {
        if (guid != "ac628c52d113f9c47a923c51d25f605e") //nightmare asset id
            return;
        if (resource is UnitEntityView view)
        {
            PatchNightmareAsset(view);
        }
    }

    public static Transform CreateMountBone(Transform parent, string type, Vector3 posOffset, Vector3? rotOffset = null)
    {
        var offsetBone = new GameObject($"Saddle_{type}_parent");
        offsetBone.transform.SetParent(parent);
        offsetBone.transform.localPosition = posOffset;
        if (rotOffset.HasValue)
            offsetBone.transform.localEulerAngles = rotOffset.Value;

        var target = new GameObject($"Saddle_{type}");
        target.transform.SetParent(offsetBone.transform);

        return target.transform;
    }

    public static void PatchNightmareAsset(UnitEntityView view)
    {
        var offsets = view.gameObject.AddComponent<MountOffsets>();

        offsets.Root = view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02");
        offsets.RootBattle = view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02");

        offsets.PelvisIkTarget = CreateMountBone(view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02"),
            "Pelvis",
            new Vector3(0f, 0.361f, 0.065f),
            new Vector3(0.7602f, 180f, 0f));
        offsets.LeftFootIkTarget = CreateMountBone(view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02"),
            "LeftFoot",
            new Vector3(0.425f, -0.3506f, -0.1074f),
            new Vector3(334.9193f, 94.9215f, 322.0144f));
        offsets.RightFootIkTarget = CreateMountBone(view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02"),
            "RightFoot",
            new Vector3(-0.425f, -0.3506f, -0.1074f),
            new Vector3(11.3555f, 92.1214f, 144.7181f));
        offsets.LeftKneeIkTarget = CreateMountBone(view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02"),
            "LeftKnee",
            new Vector3(0.386f, 0.0652f, -0.275f),
            new Vector3(359.9774f, 0f, 149.1742f));
        offsets.RightKneeIkTarget = CreateMountBone(view.Pelvis.FindChildRecursive("Locator_Torso_Upper_02"),
            "RightKnee",
            new Vector3(-0.386f, 0.0652f, -0.275f),
            new Vector3(359.9774f, 0f, 337.0312f));

        offsets.Hands = CreateMountBone(view.Pelvis.FindChildRecursive("Locator_Head_00"),
            "Hands",
            new Vector3(0f, 0.5108f, -0.5856f),
            new Vector3(359.9774f, 0f, 337.0312f));

        var offsetConfig = ScriptableObject.CreateInstance<RaceMountOffsetsConfig>();
        offsetConfig.name = "Nightmare_MountConfig";

        offsetConfig.offsets = new RaceMountOffsetsConfig.MountOffsetData[] {
                new RaceMountOffsetsConfig.MountOffsetData() {
                    Races = BlueprintRoot.Instance.Progression.m_CharacterRaces.ToList(),
                    RootPosition = new Vector3(0f, 0f, 0.5f),
                    RootBattlePosition = new Vector3(0f, 0f, 0.5f),

                    SaddleRootPosition = Vector3.zero,
                    SaddleRootScale = Vector3.one,
                    SaddleRootRotation = new Vector4(0, 0, 0, 1),

                    PelvisPosition = Vector3.zero,
                    PelvisRotation = new Vector4(0, 0, 0, 1),

                    LeftFootPosition = Vector3.zero,
                    LeftFootRotation = new Vector4(0, 0, 0, 1),

                    RightFootPosition = Vector3.zero,
                    RightFootRotation = new Vector4(0, 0, 0, 1),

                    LeftKneePosition = Vector3.zero,

                    RightKneePosition = Vector3.zero,

                    HandsPosition = new Vector3(0.15f, -0.4f, 1.2f),

                    PelvisPositionWeight = 0.9f,
                    PelvisRotationWeight = 1.0f,
                    FootsPositionWeight = 1.0f,
                    FootsRotationWeight = 1.0f,
                    KneesBendWeight = 1.0f,
                    HandsPositionWeight = 1.0f,
                    HandsMappingWeight = 0.7f,
                }
            };
        offsets.OffsetsConfig = offsetConfig;
    }


}
