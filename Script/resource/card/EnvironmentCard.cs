using Godot;
using System;

public partial class EnvironmentCard : Card
{
	public enum EffectTypeEnum
	{
		BLOCK,
		DAMAGE,
		HEALTH,
		COST
	}

	public EffectTypeEnum EffectType;
	public override void Apply()
	{
		base.Apply();
		var targets = RefreshTargets();

		switch (EffectType)
		{
			case EffectTypeEnum.BLOCK:
				foreach (var target in targets)
				{
					target.CharacterStats.Block += Amount;
				}
				break;
			case EffectTypeEnum.DAMAGE:
				foreach (var target in targets)
				{
					target.TakeDamage(Amount);
				}
				break;
			case EffectTypeEnum.HEALTH:
				foreach (var target in targets)
				{
					target.CharacterStats.Health += Amount;
				}
				break;
			// case EffectTypeEnum.COST:

			// 	break;
		}
	}
}
