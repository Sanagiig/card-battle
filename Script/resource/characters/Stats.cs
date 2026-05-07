using Godot;
using System;

public partial class Stats : Resource
{
	[Signal]
	public delegate void StatsChangedEventHandler();

	[Export]
	public int MaxHealth = 1;

	[Export]
	public int MaxBlock = 999;

	[Export]
	public Texture2D Texture;

	[Export]
	public int Health
	{
		get => _Health;
		set
		{
			SetHealth(value);
		}
	}
	private int _Health;

	[Export]
	public int Block
	{
		get => _Block;
		set
		{
			SetBlock(value);
		}
	}
	private int _Block;

	public void SetHealth(int health)
	{
		_Health = Mathf.Clamp(health, 0, MaxHealth);
		EmitSignal(SignalName.StatsChanged);
	}

	public void AddBlock(int block)
	{
		Block += block;
	}

	public void SetBlock(int block)
	{
		_Block = Mathf.Clamp(block, 0, MaxBlock);
		EmitSignal(SignalName.StatsChanged);
	}

	public void TakeDamage(int damage)
	{
		if (damage <= 0) return;

		var finalDamage = Mathf.Max(damage - Block, 0);
		Block = Mathf.Max(Block - damage, 0);
		Health -= finalDamage;
	}

	public virtual Stats CreateInstance()
	{
		var instance = Duplicate() as Stats;

		instance._Health = instance.MaxHealth;
		instance._Block = 0;
		return instance;
	}
}
