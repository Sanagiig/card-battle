using Godot;
using System;

public partial class MultiHealthCard : Card
{
	public override void Apply()
	{
		base.Apply();
		if (GetTargetCharacterCount() == 0)
		{
			GD.PrintErr($"SingleAttackCard: Invalid target count: {GetTargetCharacterCount()}");
			return;
		}

		foreach (var target in CardTargetCharacters)
		{
			target.CharacterStats.Health += Amount;
		}
	}
}
