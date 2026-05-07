using Godot;
using System;

public partial class SingleAttackCard : Card
{
	public override void Apply()
	{
		base.Apply();
		if (GetTargetCharacterCount() != 1)
		{
			GD.PrintErr($"SingleAttackCard: Invalid target count: {GetTargetCharacterCount()}");
			return;
		}

		CardTargetCharacters[0].TakeDamage(Amount);
	}
}
