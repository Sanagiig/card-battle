using Godot;
using System;
using System.Threading.Tasks;

public partial class DraggingState : CardState
{
	public override async void Enter()
	{
		base.Enter();
		await base.Init();

		CardUI.DropPointDetector.Monitoring = true;

		var uiLayer = GetTree().GetFirstNodeInGroup("UiLayer");
		if (uiLayer == null)
		{
			GD.PrintErr("UiLayer not found");
		}

		CardUI.Reparent(uiLayer);
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
		// 鼠标拖拽
		if (@event is InputEventMouseMotion mouseMotion)
		{
			CardUI.GlobalPosition = mouseMotion.Position - CardUI.PivotOffset;
		}
		else if (@event is InputEventScreenDrag screenDrag)
		{
			CardUI.GlobalPosition = screenDrag.Position - CardUI.PivotOffset;
			// 释放
		}
		else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased() || @event is InputEventScreenTouch screenTouch && screenTouch.IsReleased())
		{
			_DragRelease();
		}

		_DragToAim();
	}

	public override void OnGuiInput(InputEvent @event)
	{
		base.OnGuiInput(@event);
		// 鼠标拖拽
		if (@event is InputEventMouseMotion mouseMotion)
		{
			CardUI.GlobalPosition += mouseMotion.Position - CardUI.PivotOffset;
		}
		else if (@event is InputEventScreenDrag screenDrag)
		{
			CardUI.GlobalPosition += screenDrag.Position - CardUI.PivotOffset;
			// 释放
		}
		else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased() || @event is InputEventScreenTouch screenTouch && screenTouch.IsReleased())
		{
			GD.Print("DraggingState OnGuiInput mouseButton");
			_DragRelease();
		}

		_DragToAim();
	}

	public override void OnMouseEntered()
	{
		base.OnMouseEntered();
	}

	public override void OnMouseExited()
	{
		base.OnMouseExited();
		CardUI.DropPointDetector.Monitoring = false;
	}

	private void _DragToAim()
	{
		// 如果有施法目标，在目标区域，则进入瞄准状态
		if (CardUI.MustChooseTarget() && CardUI.IsOnDropArea)
		{
			EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.AIMING);
			return;
		}
	}

	private void _DragRelease()
	{
		if (CardUI.IsOnDropArea)
		{
			EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.RELEASED);
			return;
		}


		EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.BASE);
	}
}
