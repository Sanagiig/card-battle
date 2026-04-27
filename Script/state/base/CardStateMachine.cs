using Godot;
using System;

public partial class CardStateMachine : Node
{
	public Godot.Collections.Array<CardState> CardStateArr { get; protected set; } = new Godot.Collections.Array<CardState>();

	public Godot.Collections.Dictionary<CardState.StateEnum, CardState> StateMap { get; protected set; } = new Godot.Collections.Dictionary<CardState.StateEnum, CardState>();

	[Export]
	public CardState CurrentState
	{
		get => _CurrentState;
		protected set
		{
			_CurrentState = value;
			// GD.PrintS($"[CardStateMachine] CurrentState {_CurrentState.Name}");
			// GD.PrintErr(System.Environment.StackTrace);
		}
	}
	private CardState _CurrentState;
	public CardState PrevState { get; protected set; }

	public override void _Process(double delta)
	{
		CurrentState?.Update(delta);
	}

	public void InitStates(CardUI cardUI)
	{
		foreach (var node in GetChildren())
		{
			if (node is CardState state)
			{
				CardStateArr.Add(state);
				state.TransitionRequested += OnTransitionRequested;
				StateMap[state.State] = state;
				state.CardUI = cardUI;
			}
		}

		if (CardStateArr.Count > 0)
		{
			CurrentState = CardStateArr[0];
			CurrentState.Enter();
		}
	}

	public virtual void OnInputEvent(InputEvent eventInput)
	{
		CurrentState?.OnInput(eventInput);
	}

	public virtual void OnGuiInputEvent(InputEvent eventInput)
	{
		// GD.Print($"[CardStateMachine] GuiInput {@eventInput.GetType()}");
		// GD.Print($"cur state {CurrentState.Name}");
		CurrentState?.OnGuiInput(eventInput);
	}

	public virtual void OnMouseEntered()
	{
		CurrentState?.OnMouseEntered();
	}

	public virtual void OnMouseExited()
	{
		CurrentState?.OnMouseExited();
	}

	public virtual void OnTransitionRequested(CardState.StateEnum from, CardState.StateEnum to)
	{
		if (!StateMap.ContainsKey(from))
		{
			GD.PrintErr($"[CardStateMachine] Invalid transition from {from} ");
			return;
		}

		if (!StateMap.ContainsKey(to))
		{
			GD.PrintErr($"[CardStateMachine] Invalid transition  to {to}");
			return;
		}

		var fromState = StateMap[from];
		var toState = StateMap[to];
		GD.Print($"[CardStateMachine] Transition from {from} to {to}");
		fromState.Exit();
		toState.Enter();
		PrevState = fromState;
		CurrentState = toState;
	}
}
