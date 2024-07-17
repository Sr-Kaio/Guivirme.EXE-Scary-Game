using Godot;
using System;

public partial class SettingsHandling : Control
{
	[ExportGroup("Settings")]
	[Export]
	public CheckButton VSync;
	[Export]
	public Slider Sensibility, Master_Vol;
	[Export]
	public Label SensbilityLabel;
	[Export]
	public OptionButton Graphics;

	public override void _Ready()
	{
		VSync.ButtonPressed = PlayerPreferences.VSync;
		Sensibility.Value = PlayerPreferences.Sensibility*1000;
		Graphics.Selected = PlayerPreferences.GraphicsQuality;
		Master_Vol.Value = PlayerPreferences.Master_Vol;
		int Master_bus = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(Master_bus, Mathf.LinearToDb((float)(Master_Vol.Value/100)));
		(Graphics as GraphicsQualityHandler).Set();
	}
	public override void _Process(double delta)
	{
		SensbilityLabel.Text = "Sensbility - " + Sensibility.Value.ToString();
	}
	public void OnRequestSaveAndApply() {
		(Graphics as GraphicsQualityHandler).Set();
		PlayerPreferences.VSync = VSync.ButtonPressed;
		PlayerPreferences.Sensibility = (float)Sensibility.Value/1000;
		PlayerPreferences.GraphicsQuality = Graphics.Selected;		
		PlayerPreferences.Master_Vol = (int)Master_Vol.Value;
		int Master_bus = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(Master_bus, Mathf.LinearToDb((float)(Master_Vol.Value/100)));
		PlayerPreferences.Apply();
		PlayerPreferences.SaveSettings();
	}
}

