using Godot;
using Godot.Collections;
using System;

public partial class GraphicsQualityHandler : OptionButton {
	[Export] public WorldEnvironment worldEnviroment;
	[Export] public DirectionalLight3D light3D;
	struct Preset {
		public string PresetName;
		public bool Shadows;

		// Best = 2
		// Good = 1
		// Fast = 0
		public DirectionalLight3D.ShadowMode ShadowQuality;

		public bool SSAO;

		public bool SDFI;

		public bool SCREEN_SPACE_REFLECTIONS;

		public float RENDERING_MULTIPLIER;
	}

	Preset SceneDefaults;
	
	Preset[] GraphicsPresets = {new Preset(),new Preset(),new Preset()};
	Preset CurrentPreset;

	public override void _Ready()
	{
			SceneDefaults.SSAO = worldEnviroment.Environment.SsaoEnabled;
			SceneDefaults.SCREEN_SPACE_REFLECTIONS = worldEnviroment.Environment.SsilEnabled;
			SceneDefaults.SDFI = worldEnviroment.Environment.SdfgiEnabled;
		

		Preset HIGH = new Preset();
		HIGH.PresetName = "Ultra";

		HIGH.Shadows = true;
		HIGH.ShadowQuality = DirectionalLight3D.ShadowMode.Parallel4Splits;

		HIGH.SSAO = true;
		
		HIGH.SDFI = true;

		HIGH.SCREEN_SPACE_REFLECTIONS = true;

		HIGH.RENDERING_MULTIPLIER = 1;

		GraphicsPresets[2] = HIGH;

		Preset MID = new Preset();
		MID.PresetName = "Mid";

		MID.Shadows = true;
		MID.ShadowQuality = DirectionalLight3D.ShadowMode.Parallel2Splits;

		MID.SSAO = false;
		
		MID.SDFI = false;

		MID.SCREEN_SPACE_REFLECTIONS = true;

		MID.RENDERING_MULTIPLIER = 1;

		GraphicsPresets[1] = MID;
		
		Preset LOW = new Preset();
		LOW.PresetName = "Low";

		LOW.Shadows = false;
		LOW.ShadowQuality = DirectionalLight3D.ShadowMode.Orthogonal;

		LOW.SSAO = false;
		
		LOW.SDFI = false;

		LOW.SCREEN_SPACE_REFLECTIONS = false;

		LOW.RENDERING_MULTIPLIER = 0.5f;

		GraphicsPresets[0] = LOW;

	}

	public void Set() {
		CurrentPreset = GraphicsPresets[Selected];
		Godot.Collections.Array arguments = new Godot.Collections.Array();
		arguments.Add(CurrentPreset.Shadows);
		GetTree().Root.PropagateCall("set_shadow", arguments);
		
	
				
				worldEnviroment.Environment.SsaoEnabled = CurrentPreset.SSAO;
				worldEnviroment.Environment.SsilEnabled = CurrentPreset.SCREEN_SPACE_REFLECTIONS;
				
				worldEnviroment.Environment.SdfgiEnabled = CurrentPreset.SDFI;
			
	}
}
