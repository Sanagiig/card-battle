using Godot;
using System;

public partial class ManaUi : Panel
{
	public Label ManaLabel;

	public override void _EnterTree()
	{
		ManaLabel ??= GetNode<Label>("ManaLabel");
	}

	public override void _Ready()
	{
	}


	public override void _Process(double delta)
	{
	}
}
