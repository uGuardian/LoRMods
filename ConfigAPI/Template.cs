using System;

// Your namespace here
namespace Template {
	public class ConfigInitializer : ModInitializer {
		public override void OnInitializeMod() {
			// Put the name of your config file, and then the instance of the config class
			ConfigAPI.Init("Template", TemplateConfig.Instance);
		}
	}

	[Serializable]
	public class TemplateConfig {
		// Change TemplateConfig to whatever the name of your config class is
		public static TemplateConfig Instance = new TemplateConfig();
	}
}