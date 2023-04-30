using Godot;
using System;

public partial class Level : TileMap
{
	private int _prevBlockPosition;
	private float _prevPlayerPosition;
	private int _removeCell;
	private Player _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetParent().GetChild<Player>(0);
		_prevPlayerPosition = _player.Position.X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_prevBlockPosition += Mathf.Abs(_player.Position.X - _prevPlayerPosition) > 1.0 ? 1 : 0;
		_prevPlayerPosition = _player.Position.X;
		_removeCell = (int)(_player.Position.X / 16) - 32;
		GenerateChunk();
	}

	private void GenerateChunk()
	{
		SetCell(0, new Vector2I(_prevBlockPosition, 0), 1, new Vector2I(0, 0));
		SetCell(0, new Vector2I(_prevBlockPosition, 1), 1, new Vector2I(0, 1));
		for (var i = 2; i < 10; i++)
		{
			SetCell(0, new Vector2I(_prevBlockPosition, i), 1, new Vector2I(0, 2));
			SetCell(0, new Vector2I(_removeCell, i));
		}
		
		SetCell(0, new Vector2I(_removeCell, 0));
		SetCell(0, new Vector2I(_removeCell, 1));
		
	}
}
