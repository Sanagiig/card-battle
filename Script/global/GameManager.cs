using Godot;
using System;
using System.Linq;

public partial class GameManager : Node
{
	public CardUI CurUsingCard;

	public BaseCharacter CurrentTurnCharacter { get; private set; }

	public bool IsPlayerTurn => CurrentTurnCharacter is Player;

	public bool IsEnemyTurn => CurrentTurnCharacter is Enemy;

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
			QueueFree();
			return;
		}

		EventHub.Instance.RoundStarted += _OnTurnRoundStart;
		EventHub.Instance.RoundEnded += _OnTurnRoundEnd;

		// todo make the round manager
		// var players = GetAllPlayers();
		// _OnTurnRoundStart(players[0]);
	}

	#region Events
	private void _OnTurnRoundStart(BaseCharacter character)
	{
		CurrentTurnCharacter = character;
	}

	private void _OnTurnRoundEnd(BaseCharacter character)
	{
		CurrentTurnCharacter = null;
	}
	#endregion

	#region Actions

	#endregion

	#region Get Info
	public Enemy[] GetAllEnemies()
	{
		var enemies = GetTree().GetNodesInGroup(GroupConst.EnemyGroup);
		return enemies.Select(e => e as Enemy).ToArray();
	}

	public Player[] GetAllPlayers()
	{
		var players = GetTree().GetNodesInGroup(GroupConst.PlayerGroup);
		return players.Select(p => p as Player).ToArray();
	}

	public BaseCharacter[] GetAllCharacters()
	{
		var enemies = GetAllEnemies();
		var players = GetAllPlayers();
		var characters = enemies.Cast<BaseCharacter>().Concat(players.Cast<BaseCharacter>()).ToArray();

		return characters;
	}
	#endregion
}
