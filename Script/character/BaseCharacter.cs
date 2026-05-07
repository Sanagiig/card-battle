using Godot;
using System;

public partial class BaseCharacter : Node2D
{
	[Export]
	public CharacterStats CharacterStats { get; private set; }
	public Sprite2D BodySprite { get; private set; }
	public StatusUi StatusUi { get; private set; }

	public override void _EnterTree()
	{
		BodySprite = GetNode<Sprite2D>("BodySprite");
		StatusUi = GetNode<StatusUi>("StatusUi");
	}

	public override void _Ready()
	{
		if (CharacterStats == null)
		{
			GD.PrintErr("CharacterStats is not assigned.");
			return;
		}

		// TestDamage();
		SetCharacterStats(CharacterStats);
	}

	public override void _ExitTree()
	{
		CharacterStats.StatsChanged -= OnStatsChanged;
	}

	private void OnStatsChanged()
	{
		GD.Print($"[ BaseCharacter ] Stats changed. [{CharacterStats}]");
		StatusUi.UpdateStats(CharacterStats);
	}

	public virtual void SetCharacterStats(CharacterStats stats)
	{


		CharacterStats = stats.CreateInstance();
		BodySprite.Texture = stats.Texture;

		// 防止重复订阅事件
		CharacterStats.StatsChanged -= OnStatsChanged;
		CharacterStats.StatsChanged += OnStatsChanged;
		CharacterStats.EmitChange();
	}

	public virtual void TakeDamage(int damage)
	{
		CharacterStats.TakeDamage(damage);
		if (CharacterStats.Health <= 0)
		{
			Die();
		}
	}

	public virtual void TakeBlock(int block)
	{
		CharacterStats.AddBlock(block);
	}

	public virtual async void TestDamage()
	{
		var timer = GetTree().CreateTimer(1);

		await ToSignal(timer, Timer.SignalName.Timeout);
		TakeDamage(6);
		TakeBlock(3);
	}

	public virtual void Die()
	{
		// todo : 死亡处理
		QueueFree();
	}
}
