using Godot;
using System;
using System.Threading.Tasks;

public partial class CardState : BaseCardState
{
	[Signal]
	public delegate void TransitionRequestedEventHandler(StateEnum from, StateEnum to);

	public enum StateEnum
	{
		BASE,
		CLICKED,
		DRAGGING,
		AIMING,
		RELEASED,
	}

	[Export]
	public StateEnum State { get; set; }

	public CardUI CardUI { get; set; }

	public virtual async Task Init()
	{
		base.Enter();
		if (!CardUI.IsNodeReady())
		{
			await ToSignal(CardUI, CardUI.SignalName.Ready);
		}

		CardUI.StateLabel.Text = State.ToString();
		// CardUI.PivotOffset = Vector2.Zero;
	}

	public override void Enter()
	{
		GD.Print($"{Name} Enter");
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
