using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	private const int BushMinDistance = 400;
	[Export] public PackedScene BushScene;
	[Export] public PackedScene NpcScene;
	[Export] public PackedScene KerbScene;
	[Export] public PackedScene HouseScene;
	[Export] public PackedScene PizzeriaScene;
	private const int NpcMinDistance = 450;
	private const int KerbMinDistance = 300;

	private const byte QSize = 6;

	private Queue<Node2D> _queue;
	// Called when the node enters the scene tree for the first time.
	private float _prevObstaclePosition;
	private float _prevHousePosition;
	private float _prevPizzeriaPosition;
	private int _houseModifier;
	private int _pizzeriaModifier;
	private Player _player;
	private HUD _hud;
	public static int Money;

	public override void _Ready()
	{
		_player = GetNode<Player>("Player");
		_prevObstaclePosition = _player.Position.X + 400;
		_prevHousePosition = _player.Position.X;
		_prevPizzeriaPosition = GetNode<Area2D>("Pizzeria").Position.X;
		_queue = new Queue<Node2D>();
		_houseModifier = (int)(GD.Randi() % 10 + 11);
		_pizzeriaModifier = (int)(GD.Randi() % 10 + 16);
		_hud = GetNode<HUD>("HUD");
		// LoadGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_hud.UpdateMoney(Money);
		_hud.UpdatePizzas(_player.Pizzas);
		if (_player.Pizzas <= 0)
		{
			// Save();
			_hud.GameOver();
		}
		if (_player.Position.X - _prevHousePosition > Player.Speed * _houseModifier)
		{
			var house = HouseScene.Instantiate<House>();
			GenerateNode(600, house);
			_prevHousePosition = house.Position.X;
		}
		if (_player.Position.X - _prevPizzeriaPosition > Player.Speed * _pizzeriaModifier)
		{
			var pizzeria = PizzeriaScene.Instantiate<Pizzeria>();
			GenerateNode(600, pizzeria);
			_prevPizzeriaPosition = pizzeria.Position.X;
		}
		var chance = GD.Randi() % 3;
		switch (chance)
		{
			case 0:
				var npc = NpcScene.Instantiate<NPC>();
				GenerateNode(NpcMinDistance, npc);	
				break;
			case 1:
				var bush = BushScene.Instantiate<Bush>();
				GenerateNode(BushMinDistance, bush);
				break;
			case 2:
				var kerb = KerbScene.Instantiate<Kerb>();
				GenerateNode(KerbMinDistance, kerb);
				break;
		}

		if (_queue.Count <= 0) return;
		if (!(_queue.Peek().Position.X < _player.Position.X - 500)) return;
		var node = _queue.Dequeue();
		node.QueueFree();

	}

	private void GenerateNode(int minDistance, Node2D node)
	{
		if (minDistance < _player.Position.X - _prevObstaclePosition) return;
		if (_queue.Count >= QSize) return;
		node.Position = new Vector2(_prevObstaclePosition + minDistance, 0);
		_prevObstaclePosition = node.Position.X;
		AddChild(node);
		_queue.Enqueue(node);
	}
	
	// private void Save()
	// {
	// 	var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
	// 	
	// 	var data = new Godot.Collections.Dictionary<string, Variant>
	// 	{
	// 		["Money"] = Money
	// 	};
	// 	var jsonString = Json.Stringify(data);
 //
	// 	// Store the save dictionary as a new line in the save file.
	// 	saveGame.StoreLine(jsonString);
	// }
 //
	// private void LoadGame()
	// {
	// 	if (!FileAccess.FileExists("user://savegame.save"))
	// 	{
	// 		return; // Error! We don't have a save to load.
	// 	}
	// 	var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
	// 	
	// 	while (saveGame.GetPosition() < saveGame.GetLength())
	// 	{
	// 		var jsonString = saveGame.GetLine();
 //
	// 		// Creates the helper class to interact with JSON
	// 		var json = new Json();
	// 		var parseResult = json.Parse(jsonString);
	// 		if (parseResult != Error.Ok)
	// 		{
	// 			GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
	// 			continue;
	// 		}
 //
	// 		// Get the data from the JSON object
	// 		var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
 //
	// 		// Firstly, we need to create the object and add it to the tree and set its position.
	// 		var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
	// 		if (newObjectScene.Instantiate<Node>() is not Main newObject) return;
	// 		Money = newObject.GetMoney();
 //
	// 		// Now we set the remaining variables.
	// 		foreach (var (key, value) in nodeData)
	// 		{
	// 			if (key is "Filename" or "Parent" or "PosX" or "PosY")
	// 			{
	// 				continue;
	// 			}
	// 			newObject.Set(key, value);
	// 		}
	// 	}
 //
	// 	
	// 	
	// }
	// private int GetMoney()
 //    {
 //     	return Money;
 //    }
}
