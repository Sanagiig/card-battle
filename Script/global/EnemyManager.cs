using Godot;
using System;

public partial class EnemyManager : Node
{
	public static EnemyManager Instance { get; private set; }

	public Godot.Collections.Array<Enemy> Enemies { get; private set; } = new Godot.Collections.Array<Enemy>();

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GD.PrintErr("[EnemyManager] Instance already exists");
		}

		EventHub.Instance.EnemySpawned += _EnemySpawned;
		EventHub.Instance.EnemyDestroyed += _EnemyDestroyed;
	}

	private void _EnemySpawned(Enemy enemy)
	{
		Enemies.Add(enemy);
	}

	private void _EnemyDestroyed(Enemy enemy)
	{
		Enemies.Remove(enemy);
	}
}
