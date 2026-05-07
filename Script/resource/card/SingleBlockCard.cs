using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class SingleBlockCard : Card
{
	public override void Apply()
	{
		base.Apply();
		if (GetTargetCharacterCount() != 1)
		{
			GD.PrintErr($"SingleAttackCard: Invalid target count: {GetTargetCharacterCount()}");
			return;
		}

		CardTargetCharacters[0].CharacterStats.Block += Amount;
	}
}
