using Godot;
using System;

public partial class GameManager : Node {
    public static bool hasKey;

    [Export] public Area3D area3D;
    public override void _Ready()
    {
        hasKey = false;    
    }

    public override void _Process(double delta)
    {
        var bodies = area3D.GetOverlappingBodies();
		foreach (Node3D body in bodies)
		{
			if (body is CharacterMovement && hasKey) {
                GetTree().ChangeSceneToFile("res://data/Scenes/win.tscn");
            }
		}
    }

    public void CloseGam() {
        GetTree().Quit();
    }
}