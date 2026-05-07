using Godot;
using System;

public partial class StatusUi : Control
{
	public HBoxContainer Box { get; private set; }

	public HBoxContainer BlockBox { get; private set; }

	public HBoxContainer HealthBox { get; private set; }

	public Label BlockLabel { get; private set; }

	public Label HealthLabel { get; private set; }

	public override void _EnterTree()
	{
		Box ??= GetNode<HBoxContainer>("Box");
		BlockBox ??= GetNode<HBoxContainer>("Box/BlockBox");
		HealthBox ??= GetNode<HBoxContainer>("Box/HealthBox");

		BlockLabel ??= GetNode<Label>("Box/BlockBox/Label");
		HealthLabel ??= GetNode<Label>("Box/HealthBox/Label");
	}

	public void SetBlock(int block)
	{
		BlockLabel.Text = block.ToString();
	}

	public void SetHealth(int health)
	{
		HealthLabel.Text = health.ToString();
	}

	public void UpdateStats(Stats stats)
	{
		SetHealth(stats.Health);
		HealthBox.Visible = stats.Health > 0;
		SetBlock(stats.Block);
		BlockBox.Visible = stats.Block > 0;
	}
}
