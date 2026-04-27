using Godot;
using System;
using System.Threading.Tasks;

public partial class ReleasedState : CardState
{
	private float _Cooldown = 0.5f;
	public override async void Enter()
	{
		base.Enter();
		await base.Init();
		CardUI.ColorRect.Color = Colors.Blue;
		CardUI.DropPointDetector.Monitoring = true;
	}

	public override void Update(double delta)
	{
		base.Update(delta);
		_Cooldown -= (float)delta;
		if (_Cooldown <= 0)
		{
			Release();
		}
	}

	public override void Exit()
	{
		base.Exit();
		CardUI.DropPointDetector.Monitoring = false;
	}

	public override void OnInput(InputEvent @event)
	{
		base.OnInput(@event);
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

	public void Release()
	{
		GD.Print($"[ReleasedState] Release {CardUI.IsOnDropArea}");
		if (CardUI.IsOnDropArea)
		{
			// todo - play release animation
			CardUI.QueueFree();
		}
		else
		{
			EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.BASE);
		}
	}
}
