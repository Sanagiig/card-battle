using Godot;
using System;

public partial class ClickedState : CardState
{
	public override async void Enter()
	{
		base.Enter();
		await base.Init();

		CardUI.ColorRect.Color = Colors.Orange;
	}

	public override void Update(double delta)
	{
		base.Update(delta);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void OnInput(InputEvent @event)
	{
		base.OnInput(@event);
	}

	public override void OnGuiInput(InputEvent @event)
	{
		base.OnGuiInput(@event);
		if (@event is InputEventMouseMotion mouseMotion)
		{
			EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.DRAGGING);
		}
		else if (@event is InputEventScreenDrag screenDrag)
		{
			EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.DRAGGING);
		}
		else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased() || @event is InputEventScreenTouch screenTouch && screenTouch.IsReleased())
		{
			EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.RELEASED);
		}
	}

	public override void OnMouseEntered()
	{
		base.OnMouseEntered();
	}

	public override void OnMouseExited()
	{
		base.OnMouseExited();
	}
}
