using Godot;
using System;

public partial class GuivirmeEXE : CharacterBody3D
{
	[Export] public NavigationAgent3D agent;
	[Export] public CharacterMovement player;
	[Export] public Area3D areaDeath;
	[Export] public Timer timer;
	public bool isOn = false;
	public const float speed = 3f;
	public const float chaseSpeed = 5.8f;
	private float curspeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer.Start(1f);
		timer.Timeout += new Action(() => {
			isOn = true;
			timer.Stop();
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var bodies = areaDeath.GetOverlappingBodies();
		foreach (Node3D body in bodies)
		{
			if (body is CharacterMovement) {
				GetTree().ChangeSceneToFile("res://data/Scenes/gameover.tscn");
			}
		}
		var space_state = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(GlobalTransform.Origin, player.GlobalTransform.Origin);
		var result = space_state.IntersectRay(query);
		if (result.Count > 0) 
			curspeed = chaseSpeed;
		else
			curspeed = speed;
		
		if(isOn) {
			Vector3 dir = new Vector3();

			agent.TargetPosition = player.GlobalPosition;

			dir = agent.GetNextPathPosition() - GlobalPosition;
			dir = dir.Normalized();

			Velocity = Velocity.Lerp(dir * curspeed, 10f * (float)delta);

			MoveAndSlide();
		}
	}
}
