using Godot;
using System;

public partial class Interaction : RayCast3D {
    [Export] public AudioStreamPlayer sfx;
    [Export] public TextureRect tex;
    public override void _UnhandledInput(InputEvent @event)
    {
        if(Input.IsActionPressed("Interact")) {
            if(IsColliding()) {
                Node3D node3D = GetCollider() as Node3D;
                if (node3D.Name == "Key") {
                    sfx.Play();
                    tex.Modulate = new Color(1,1,1,1);
                    GameManager.hasKey = true;
                    node3D.QueueFree();
                }
            }
        }
    }
}