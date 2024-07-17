using Godot;
using System;
using System.Text.Json;
using System.IO;

public partial class PlayerPreferences : Node
{
	public static bool VSync;
	public static bool FullScreen;

	public static float Sensibility = 0.01f;
	public static int GraphicsQuality, Master_Vol;

	public override void _Ready()
	{
		LoadSettings();
	}

	public static void Apply()
	{

		//Yandere
		if (VSync) {
			DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
		} else {
			DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
		}

		// if (FullScreen) {
		// 	DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		// } else {
		// 	DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		// }
	}

	public static void SaveSettings()
	{
		var config = new ConfigFile();

		config.SetValue("misc", "vsync", VSync);
		config.SetValue("misc", "sens", Sensibility);
		config.SetValue("misc", "graphics_quality", GraphicsQuality);
		config.SetValue("volume", "master", Master_Vol);

		config.Save("res://config.cfg");
	}

	public static void LoadSettings()
	{
		var config = new ConfigFile();

		// Load data from a file.
		Error err = config.Load("res://config.cfg");

		// If the file didn't load, ignore it.
		if (err != Error.Ok)
		{
			GD.PrintErr("CONFIG NOT SAVED");
			return;
		}
		VSync = (bool)config.GetValue("misc", "vsync");
		// FullScreen = (bool)config.GetValue("misc", "full_screen");
		Sensibility = (float)config.GetValue("misc", "sens");
		GraphicsQuality = (int)config.GetValue("misc","graphics_quality");
		Master_Vol = (int)config.GetValue("volume","master");
		GD.Print(VSync, Sensibility, GraphicsQuality);
		Apply();
	}
}
