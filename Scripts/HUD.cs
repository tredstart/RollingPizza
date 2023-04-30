using Godot;
using System;

public partial class HUD : CanvasLayer
{
	private Label _money;

	private Label _pizzas;

	private VBoxContainer _pauseBox;
	private VBoxContainer _gameOverBox;
	private AudioStreamPlayer2D _menuSound;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_money = GetNode<Label>("Money");
		_pizzas = GetNode<Label>("Pizzas");
		_pauseBox = GetNode<VBoxContainer>("PauseBox");
		_gameOverBox = GetNode<VBoxContainer>("GameOver");
		_menuSound = GetNode<AudioStreamPlayer2D>("MenuSound");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed("esc")) return;
		_menuSound.Play();
		GetTree().Paused = true;
		_pauseBox.Visible = true;
	}

	public void UpdateMoney(int money)
	{
		_money.Text = $"{money}$";
	}
	public void UpdatePizzas(int pizzas)
	{
		_pizzas.Text = pizzas.ToString();
	}

	public void GameOver()
	{
		GetTree().Paused = true;
		_gameOverBox.Visible = true;
	}

	private void OnResumePressed()
	{
		_menuSound.Play();
		GetTree().Paused = false;
		_pauseBox.Visible = false;
	}
	private void OnMainMenuPressed()
	{
		_menuSound.Play();
		GetTree().Paused = false;
		_pauseBox.Visible = false;
		GetTree().ChangeSceneToFile("res://Scenes/StartingScene.tscn");
	}
	private void OnQuitGamePressed()
	{
		_menuSound.Play();
		GetTree().Quit();
	}
	
	private void OnRestartPressed()
	{
		_menuSound.Play();
		GetTree().Paused = false;
		_pauseBox.Visible = false;
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}
}
