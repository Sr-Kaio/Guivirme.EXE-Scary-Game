using Godot;
using System;

public partial class MainMenuManager : Control
{
	[Export] public AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Input(InputEvent @event)
	{
		if (@event.IsPressed())
			animationPlayer.Play("end");
	}

	public void goToGame() {
		GetTree().ChangeSceneToFile("res://data/Scenes/game.tscn");
	}
}
