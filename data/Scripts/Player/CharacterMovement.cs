using Godot;
using System;

public partial class CharacterMovement : CharacterBody3D
{
	public const float Speed = 4.0f;
	public const float RunSpeed = 8.0f;
	public const float JumpVelocity = 6.5f;
	public static bool enabled = true;


	private float WishSpeed;
	private float CurrentSpeed;


	[ExportGroup("References")]
	[Export] private Node3D VIEWMODEL_HANDLER;
	[Export] private Node3D CAMERA_HANDLER;
	[Export] private Camera3D CAMERA;
	[Export] private Timer TIMERFOOTSTEP;
	[Export] private AudioStreamPlayer Footstep;
	[Export] private AudioStreamMP3[] FootstepsSounds;
	[Export] private AudioStreamMP3 LastFootstepSound;

	[ExportGroup("Values")]
	[Export] public float swayAmount = 0.05f;
	[Export] public float swaySpeed = 8.0f;
	[Export] private float RUNNING_FIELD_OF_VIEW;
	[Export] private float WALKING_FIELD_OF_VIEW;
	
	private float wishFov = 90f;
	private Vector2 _mouseDelta;
	Vector3 camRot;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		wishFov = WALKING_FIELD_OF_VIEW;
		Input.MouseMode = Input.MouseModeEnum.Captured;

		TIMERFOOTSTEP.Timeout += footstep;
	}
	public void footstep() {
			AudioStreamMP3 stream = FootstepsSounds[new Random().Next(0, FootstepsSounds.Length)];
			if(stream == LastFootstepSound) {
				footstep();
				return;
			}

			Footstep.Stream = stream;
			Footstep.Play();
			LastFootstepSound = stream;
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseButton && enabled) {
			RotateY(-eventMouseButton.Relative.X * PlayerPreferences.Sensibility);

			CAMERA_HANDLER.RotateX(eventMouseButton.Relative.Y * PlayerPreferences.Sensibility);

			
			_mouseDelta = eventMouseButton.Relative;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		Vector2 moveRawInput = Vector2.Zero;
		moveRawInput.X = Input.GetActionStrength("MoveLeft") - Input.GetActionStrength("MoveRight");
		moveRawInput.Y = Input.GetActionStrength("MoveUp") - Input.GetActionStrength("MoveDown");
		
		RotateY((Input.GetActionStrength("LookLeft") - Input.GetActionStrength("LookRight")) * (PlayerPreferences.Sensibility * 4));
		CAMERA_HANDLER.RotateX((Input.GetActionStrength("LookDown") - Input.GetActionStrength("LookUp")) * (PlayerPreferences.Sensibility * 4));
		
		Vector3 camRot = CAMERA_HANDLER.Rotation;
		camRot.X = Mathf.Clamp(camRot.X, -Mathf.DegToRad(90), Mathf.DegToRad(90));
		CAMERA_HANDLER.Rotation = camRot;
		GD.Print(moveRawInput.X);
		Vector3 dir = (moveRawInput.X * Transform.Basis.X) + (moveRawInput.Y * Transform.Basis.Z).Normalized();

		if(dir != Vector3.Zero && enabled) {
			wishFov = Input.IsActionPressed("Running") && IsOnFloor() ? RUNNING_FIELD_OF_VIEW : WALKING_FIELD_OF_VIEW;
			WishSpeed = Input.IsActionPressed("Running") && IsOnFloor() ? RunSpeed : Speed;
			double waittime = Input.IsActionPressed("Running") ? .4 : .7;
			TIMERFOOTSTEP.WaitTime = waittime;
			velocity.X = dir.X * CurrentSpeed;
			velocity.Z = dir.Z * CurrentSpeed;
			if (TIMERFOOTSTEP.TimeLeft == 0) {
				TIMERFOOTSTEP.Start();
			}
		} else {
			TIMERFOOTSTEP.Stop();
			WishSpeed = 0;
			wishFov = WALKING_FIELD_OF_VIEW;
			velocity.X = Mathf.MoveToward(velocity.X, 0, CurrentSpeed);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, CurrentSpeed);
		}

		CAMERA.Fov = (float)Mathf.Lerp(CAMERA.Fov, wishFov, 3 * delta);

		_mouseDelta = _mouseDelta.Lerp(Vector2.Zero, swaySpeed * (float) delta);		

		CAMERA.Rotation = new Vector3(CAMERA.Rotation.X, CAMERA.Rotation.Y, Mathf.Lerp(CAMERA.Rotation.Z, (moveRawInput.X + Math.Clamp(_mouseDelta.X,-1,1)) * 0.1f, 3 * (float)delta));
		CurrentSpeed = (float)Mathf.Lerp(CurrentSpeed, WishSpeed, 3 * delta);

		VIEWMODEL_HANDLER.Rotation = VIEWMODEL_HANDLER.Rotation.Lerp(new Vector3(_mouseDelta.Y * swayAmount, _mouseDelta.X * swayAmount, (moveRawInput.X + Math.Clamp(-_mouseDelta.X,-3,3)) * 0.1f), swaySpeed * (float)delta);

		Velocity = velocity;

		MoveAndSlide();
	}
}
