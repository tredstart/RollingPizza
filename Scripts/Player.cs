using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HitNPCEventHandler(AnimatedSprite2D hitAnimation);

	[Signal]
	public delegate void GetPizzaEventHandler();
	
	public const float Speed = 370.0f;
	private const float Acceleration = 5.0f;
	private const float JumpVelocity = -290.0f;
	private RayCast2D _edge; 
	[Export]
	public int Pizzas { get; private set; }
	private float _lastCollision;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private RayCast2D _head;
	private AnimatedSprite2D _bike;
	private AnimatedSprite2D _body;
	private AnimatedSprite2D _leg;
	private bool _falling;

	private AudioStreamPlayer2D _moneySound;
	private AudioStreamPlayer2D _hitSound;
	private AudioStreamPlayer2D _jumpSound;
	private AudioStreamPlayer2D _pizzaSound;
	private AudioStreamPlayer2D _rideSound;

	public override void _Ready()
	{
		_edge = GetNode<RayCast2D>("Edge");
		_head = GetNode<RayCast2D>("Head");
		_bike = GetNode<AnimatedSprite2D>("Bike");
		_body = GetNode<AnimatedSprite2D>("Body");
		_leg = GetNode<AnimatedSprite2D>("Leg");

		_moneySound = GetNode<AudioStreamPlayer2D>("MoneySound");
		_hitSound = GetNode<AudioStreamPlayer2D>("HitSound");
		_jumpSound = GetNode<AudioStreamPlayer2D>("JumpSound");
		_pizzaSound = GetNode<AudioStreamPlayer2D>("PizzaSound");
		_rideSound = GetNode<AudioStreamPlayer2D>("RideSound");
		PlayIdle();
	}
	public override void _PhysicsProcess(double delta)
	{
		if(_falling)
		{
			MoveAndSlide();
			return;
		}
		if (Velocity.X == 0)
		{
			PlayIdle();
		}
		else if(IsOnFloor())
		{
			PlayRide();
		}
		var velocity = Velocity;
		var direction = 1;
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += _gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionPressed("mouse_left") && IsOnFloor())
			if (GetGlobalMousePosition().X > Position.X)
			{
				velocity.Y = JumpVelocity;
				PlayJump();
			}
			else
			{
				direction = -3;
			}
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		velocity.X = Mathf.Clamp(velocity.X + direction * Acceleration, 0, Speed);
		Velocity = velocity;
		MoveAndSlide();
		if (_edge.IsColliding() && Math.Abs(Position.X - _lastCollision) > 10)
		{
			Collided();
		}

		if (!_head.IsColliding() || Velocity.X != 0) return;
		if (_head.GetCollider() is not Area2D collider) return;
		if (collider.Name.ToString().Contains("House"))
		{
			MakeDelivery();
		}
		else
		{
			StopAtPizzeria();
		}
		collider.SetCollisionLayerValue(3, false);
	}

	private void Collided()
	{
		PlayFall();
		_falling = true;
		Velocity = new Vector2(0, 1000);
		Pizzas -= 1;
		GD.Print(Pizzas);
		_lastCollision = Position.X;
	}

	private void OnHitNPC(AnimatedSprite2D hitAnimation)
	{
		GD.Print(Velocity.X);
		if (!(Velocity.X >= Speed * 0.5)) return;
		hitAnimation.Play("hit");
		Collided();
	}

	private void MakeDelivery()
	{
		GD.Print("Delivered!");
		Pizzas -= 1;
		Main.Money += 32;
		_moneySound.Play();
	}
	private void StopAtPizzeria()
	{
		GD.Print("Stop at the pizzeria!");
		Pizzas += 1;
		_pizzaSound.Play();
	}

	private void PlayIdle()
	{
		_rideSound.Stop();
		_bike.Play("idle");
		_body.Play("idle");
		_leg.Play("idle");
	}
	private void PlayRide()
	{
		if (!_rideSound.Playing)
		{
			_rideSound.Play();
		}
		
		_bike.Play("ride");
		_body.Play("ride");
		_leg.Play("ride");
	}
	private void PlayJump()
	{
		_rideSound.Stop();
		_jumpSound.Play();
		_bike.Play("jump");
		_body.Play("jump");
		_leg.Play("jump");
	}
	private void PlayFall()
	{
		_rideSound.Stop();
		_hitSound.Play();
		_bike.Play("fall");
		_body.Play("fall");
		_leg.Play("fall");

	}

	private void OnBikeFinishedFalling()
	{
		_falling = false;
	}
}
