using Godot;
using System;

public partial class HealthEffect : BaseEffect
{
	[Export]
	public int Amount { get; set; }

	public override void Execute(Node[] targets)
	{
		base.Execute(targets);
		foreach (var target in targets)
		{
			if (target is BaseCharacter character)
			{
				character.CharacterStats.Health += Amount;
			}
		}
	}
}
