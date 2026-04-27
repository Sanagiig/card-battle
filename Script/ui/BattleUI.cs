using Godot;
using System;

public partial class BattleUI : CanvasLayer
{
	public HBoxContainer CardContainer { get; private set; }

	public override void _EnterTree()
	{
		CardContainer = GetNode<HBoxContainer>("CardContainer");
	}

	public override void _Ready()
	{
		EventHub.Instance.CardResetRequested += _OnCardResetRequested;
	}

	public override void _Process(double delta)
	{
	}

	private void _OnCardResetRequested(CardUI cardUI)
	{
		// CardContainer
		cardUI.Reparent(CardContainer);
		CardContainer.MoveChild(cardUI, 0);
	}
}
