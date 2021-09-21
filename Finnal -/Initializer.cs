using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinallyBeyondTheTime {
    public class Initializer : ModInitializer {
        public override void OnInitializeMod() {
            List<String> assembly = new List<String>();
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
				assembly.Add(a.GetName().Name);
			}
			if (assembly.Contains("0Harmony")) {
				Debug.Log("Finall: Harmony Found");
				FinallHarmony.Load();
			} else {
				Debug.Log("Finall: Harmony Unavailable");
            }
            Config.instance.Load();
            Config.instance.EchoAll();
        }
    }
}