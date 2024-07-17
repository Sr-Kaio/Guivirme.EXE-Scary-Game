using Godot;
using System;

public partial class PlayerGUIManaging : CanvasLayer {

	public bool IsPaused;

	[ExportGroup("References")]
	[Export]
	public Control PauseControl;
	[Export]
	public CanvasLayer MainPause;
	[Export]
	public Button ResumeBtn;
	[Export]
	public CharacterMovement player;

	public override void _Ready()
	{
	   	IsPaused = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("Pause")) {
			IsPaused = !IsPaused;
			if (player != null) {
				CharacterMovement.enabled = !IsPaused;
				player.SetProcess(!IsPaused);
				player.SetProcessUnhandledInput(!IsPaused);			
			}

		if (IsPaused) {
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else { 
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		ResumeBtn.GrabFocus();
		}
	}

	public override void _Process(double delta)
	{
		PauseControl.Visible = IsPaused;

		Engine.TimeScale = IsPaused ? 0 : 1;
		Godot.Collections.Array arguments = new Godot.Collections.Array();
		arguments.Add(IsPaused);
		GetTree().Root.PropagateCall("set_stream_paused", arguments);
		
	}

	public void Resume() {
		IsPaused = false;
		if (player != null) {
			CharacterMovement.enabled = true;
			player.SetProcess(true);
			player.SetProcessUnhandledInput(true);
		}
	}

	public void BackToMenu() {
		IsPaused = false;
		Engine.TimeScale = 1;
		CharacterMovement.enabled = true;
		GetTree().ChangeSceneToFile("res://data/Scenes/mainmenu.tscn");
	}

	public void Quit() {
		IsPaused = false;
		Engine.TimeScale = 1;
		GetTree().Quit();
	}
}
