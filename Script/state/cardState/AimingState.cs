using Godot;
using System;

public partial class AimingState : CardState
{
	private bool _IsMotionInDropArea = false;
	public override async void Enter()
	{
		base.Enter();
		await Init();

		_IsMotionInDropArea = false;
		EventHub.Instance.GlobalMotionEnteredDropArea += _OnMotionEnteredDropArea;
		EventHub.Instance.GlobalMotionExitedDropArea += _OnMotionExitedDropArea;

		AimStart();
		GameManager.Instance.CurUsingCard = CardUI;


	}

	public override void Update(double delta)
	{
		base.Update(delta);
	}

	public override void Exit()
	{
		base.Exit();
		GameManager.Instance.CurUsingCard = null;

		EventHub.Instance.GlobalMotionEnteredDropArea -= _OnMotionEnteredDropArea;
		EventHub.Instance.GlobalMotionExitedDropArea -= _OnMotionExitedDropArea;
	}

	public override void OnInput(InputEvent @event)
	{
		base.OnInput(@event);
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// GD.Print($"mouse pos: {mouseMotion.Position} global pos: {mouseMotion.GlobalPosition}");
			_AimPosChange(mouseMotion.Position);
		}
		else if (@event is InputEventScreenDrag screenDrag)
		{
			_AimPosChange(screenDrag.Position);
		}
		else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased())
		{
			// GD.Print("[AimingState] mouse release at ");
			_AimRelease();
		}
		else if (@event is InputEventScreenTouch screenTouch && screenTouch.IsReleased())
		{
			_AimRelease();
		}
	}

	public override void OnGuiInput(InputEvent @event)
	{
		base.OnGuiInput(@event);
	}

	public override void OnMouseEntered()
	{
		base.OnMouseEntered();
	}

	public override void OnMouseExited()
	{
		base.OnMouseExited();
	}

	private void _OnMotionEnteredDropArea()
	{
		GD.Print("[AimingState] motion entered drop area");
		_IsMotionInDropArea = true;
	}

	private void _OnMotionExitedDropArea()
	{
		_IsMotionInDropArea = false;
	}

	#region Aim Logic
	public async void AimStart()
	{
		var cardContainer = CardUI.CardContainer;
		var offset = new Vector2(cardContainer.Size.X / 2, -CardUI.Size.Y / 2);

		offset.X -= CardUI.Size.X / 2;
		offset.Y += cardContainer.GlobalPosition.Y;

		CardUI.ColorRect.Color = Colors.WebPurple;

		await CardUI.AnimateToPosition(offset, 0.2f);
		EventHub.Instance.EmitSignal(EventHub.SignalName.CardAimStarted, CardUI);
	}

	public void AimEnd()
	{
		EventHub.Instance.EmitSignal(EventHub.SignalName.CardAimEnded, CardUI);
	}

	private void _AimRelease()
	{
		var nextState = _IsMotionInDropArea ? StateEnum.RELEASED : StateEnum.BASE;
		GD.Print($"[AimingState] AimRelease isInDropArea: {_IsMotionInDropArea}, nextState: {nextState}");
		EventHub.Instance.EmitSignal(EventHub.SignalName.CardAimEnded, CardUI);
		EmitSignal(SignalName.TransitionRequested, (int)State, (int)nextState);
	}

	private void _AimPosChange(Vector2 pos)
	{
		EventHub.Instance.EmitSignal(EventHub.SignalName.CardAimPositionChanged, pos);
	}
	#endregion
}
