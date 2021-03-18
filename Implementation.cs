using MelonLoader;
using UnityEngine;
using Harmony;

namespace RopeLuck
{
    public class Implementation : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            Debug.Log($"[{Info.Name}] Version {Info.Version} loaded!");
        }
    }

    /*[HarmonyPatch(typeof(GearItem), "Awake")]
    internal class DestroyRopeGearAwake
    {
        private static void Postfix(GearItem __instance)
        {
            if (__instance.name == "GEAR_Rope(Clone)")
            {
                //MelonLogger.Log("Rope destroyed in Awake");
                GameObject.Destroy(__instance.gameObject);
            }
        }
    }*/

    [HarmonyPatch(typeof(RopeAnchorPoint), "Deserialize")]
    internal class MaybeChangeActiveState
    {
        private static void Postfix(RopeAnchorPoint __instance)
        {
            if (Utils.RollChance(10f))
            {
                //MelonLogger.Log("Rope Anchor Point before: {0}", __instance.m_RopeDeployed);
                __instance.SetRopeActiveState(!__instance.m_RopeDeployed, false);
                __instance.m_RopeDeployed = !__instance.m_RopeDeployed;
                //MelonLogger.Log("Rope Anchor Point after: {0}", __instance.m_RopeDeployed);
            }
        }
    }

    [HarmonyPatch(typeof(RopeAnchorPoint), "ActionStarted")]
    [HarmonyPriority(Priority.Last)]
    internal class CantInteractWithAnchorPoint
    {
        private static bool Prefix()
        {
            GameAudioManager.PlayGUIError();
            return false;
        }
    }
}
