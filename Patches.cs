using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions.Must;
using UWE;

namespace RewrittenRando
{
    internal class Patches
    {
        [HarmonyPatch(typeof(LootSpawner))]
        public static class LootSpawnerPatch
        {
            [HarmonyPatch(nameof(LootSpawner.GetEscapePodStorageTechTypes))]
            [HarmonyPostfix]
            public static void Postfix(ref TechType[] __result)
            {
                var list = __result.Append(TechType.Knife);
                __result = list.ToArray();
                KnownTech.UnlockAll();
            }
        }
        [HarmonyPatch(typeof(BreakableResource))]
        public static class BreakableResource_Patch
        {
            [HarmonyPatch(nameof(BreakableResource.SpawnResourceFromPrefab), new Type[] { typeof(AssetReferenceGameObject), typeof(Vector3), typeof(Vector3) })]
            [HarmonyPrefix]
            public static bool Prefix(AssetReferenceGameObject breakPrefab, Vector3 position, Vector3 up)
            {
                CoroutineHost.StartCoroutine(dostuff(breakPrefab, position, up));
                return false;
            }
            public static IEnumerator dostuff(AssetReferenceGameObject breakPrefab, Vector3 position, Vector3 up)
            {
                var prefab_ = Addressables.LoadAssetAsync<GameObject>(breakPrefab.RuntimeKey);
                yield return prefab_;
                var prefab = prefab_.Result;
                var TT = CraftData.GetTechType(prefab);
                var newTT = RewrittenRando.currentrandompool[TT];
                var newprefab_ = CraftData.GetPrefabForTechTypeAsync(newTT);
                yield return newprefab_;
                var newprefab = newprefab_.GetResult();
                var thing = GameObject.Instantiate(newprefab, position, default);
                thing.EnsureComponent<AlreadyRandom>();

            }
        }
        [HarmonyPatch(typeof(CraftData))]
        public static class CraftData_Patch
        {
            [HarmonyPatch(nameof(CraftData.AddToInventory))]
            [HarmonyPrefix]
            public static void Prefix(ref TechType techType)
            {

                techType = RewrittenRando.currentrandompool[techType];
            }
        }
        [HarmonyPatch(typeof(Inventory))]
        public static class Inventory_Patch
        {
            [HarmonyPatch(nameof(Inventory.Pickup))]
            [HarmonyPrefix]
            public static bool Prefix(Pickupable pickupable, ref bool __result)
            {
                var trace = new StackTrace();
                if (pickupable.GetComponent<AlreadyRandom>() || trace.GetFrame(2).GetMethod().Name.ToLower().Contains("movenext"))
                    return true;
                CoroutineHost.StartCoroutine(dostuff(pickupable));
                __result = true;
                return false;
            }
            public static IEnumerator dostuff(Pickupable pickup)
            {
                var TT = pickup.GetTechType();
                if (TT.ToString().ToLower().Contains("undiscover"))
                {
                    TT = (TechType)Enum.Parse(typeof(TechType), TT.ToString().Replace("Undiscovered", ""));
                }
                var newTT = RewrittenRando.currentrandompool[TT];
                var go = new TaskResult<GameObject>();
                var asyncdata = CraftData.AddToInventoryAsync(newTT, go);
                yield return asyncdata;
                go.value.EnsureComponent<AlreadyRandom>();
                GameObject.Destroy(pickup.gameObject);
            }
        }
        [HarmonyPatch(typeof(PickPrefab))]
        public static class PickPrefabPatch
        {
            [HarmonyPatch(nameof(PickPrefab.Start))]
            [HarmonyPostfix]
            public static void Postfix(PickPrefab __instance)
            {
                __instance.pickTech = RewrittenRando.currentrandompool[__instance.pickTech];
            }
        }
        [HarmonyPatch(typeof(ConstructorInput))]
        public static class ConstructorInputPatch
        {
            [HarmonyPatch(nameof(ConstructorInput.Craft))]
            [HarmonyPrefix]
            public static void Prefix(ref TechType techType)
            {
                techType = RewrittenRando.vehiclepool[techType];
            }
        }
    }
}
