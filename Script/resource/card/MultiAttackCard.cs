using Godot;
using System;
using System.Linq;

public partial class MultiAttackCard : Card
{
	public override void Apply()
	{
		base.Apply();
		if (GetTargetCharacterCount() == 0)
		{
			GD.PrintErr($"SingleAttackCard: Invalid target count: {GetTargetCharacterCount()}");
			return;
		}

		foreach(var target in CardTargetCharacters){
			target.TakeDamage(Amount);
		}
	}
}
