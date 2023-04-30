using Godot;
using System;

public partial class NPC : Area2D
{
	private AnimatedSprite2D _animation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animation = GetNode<AnimatedSprite2D>("Animation");
		_animation.Play("idle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(PhysicsBody2D body)
	{
		body.EmitSignal(Player.SignalName.HitNPC, _animation);
	}
}
