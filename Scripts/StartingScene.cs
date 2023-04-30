using Godot;
using System;

public partial class StartingScene : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	private AudioStreamPlayer2D _menuSound;
	public override void _Ready()
	{
		_menuSound = GetNode<AudioStreamPlayer2D>("MenuSound");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnStartGamePressed()
	{
		_menuSound.Play();
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}
	
	private void OnQuitGamePressed()
	{
		_menuSound.Play();
		GetTree().Quit();
	}
}
