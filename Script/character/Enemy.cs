using Godot;
using System;

public partial class Enemy : Node2D
{
	public Sprite2D BodySprite { get; protected set; }

	public override void _EnterTree()
	{
		BodySprite ??= GetNode<Sprite2D>("BodySprite");
	}

	public override void _Ready()
	{
		EventHub.Instance.EmitSignal(EventHub.SignalName.EnemySpawned, this);
	}

	public override void _Process(double delta)
	{
	}

	#region Get Info
	public Vector2 GetSize()
	{
		var spriteSize = BodySprite.Texture.GetSize();
		spriteSize.X *= BodySprite.Scale.X;
		spriteSize.Y *= BodySprite.Scale.Y;

		return spriteSize;
	}
	public Vector2 GetDownCenterGlobalPosition()
	{
		var spriteSize = GetSize();
		// GD.Print($"size {spriteSize} {GlobalPosition}");

		var finalPos = GlobalPosition;
		// finalPos.X -= spriteSize.X / 2;
		finalPos.Y += spriteSize.Y;
		return finalPos;
	}
	#endregion
}
