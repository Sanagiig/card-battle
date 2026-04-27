using Godot;
using System;

public partial class GameManager : Node
{
	public CardUI CurUsingCard;
	
	public static GameManager Instance { get; private set; }
	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GD.PrintErr("[GameManager] Instance already exists");
		}
	}
}
