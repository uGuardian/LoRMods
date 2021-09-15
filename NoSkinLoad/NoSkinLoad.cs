using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HarmonyLib;
using BepInEx;
using BepInEx.Harmony;

namespace NoSkinLoad {
    [BepInPlugin("LoR.uGuardian.NoSkinLoad", "NoSkinLoad", "1.0")]
    public class NoSkinLoad : BaseUnityPlugin {
        void Awake() {
            Harmony harmony = new Harmony("LoR.uGuardian.NoSkinLoad");
            harmony.PatchAll();
        }
    }
    [HarmonyPatch(typeof(CustomizingResourceLoader))]
    [HarmonyPatch("LoadWorkshopCustomSkinData")]
    class Patch {
        static bool Prefix() {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Contains("-noskins")) {
                UnityEngine.Debug.Log("NoSkinLoad: Skipping Workshop Skins");
                return false;
            } else {
                return true;
            }
        }
    }
}