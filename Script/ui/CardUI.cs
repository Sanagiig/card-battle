using Godot;
using System;
using System.Threading.Tasks;

public partial class CardUI : Control
{
	[Export]
	public Card CardData;

	public ColorRect ColorRect { get; private set; }

	public Label StateLabel { get; private set; }

	public Area2D DropPointDetector { get; private set; }

	public CardStateMachine StateMachine { get; private set; }

	public HBoxContainer CardContainer { get; private set; }

	public bool IsOnDropArea { get; private set; }

	public override void _EnterTree()
	{
		ColorRect ??= GetNode<ColorRect>("Panel/MarginContainer/ColorRect");
		StateLabel ??= GetNode<Label>("Panel/MarginContainer/Label");
		DropPointDetector ??= GetNode<Area2D>("DropPointDetector");
		StateMachine ??= GetNode<CardStateMachine>("StateMachine");
		CardContainer ??= GetParent<HBoxContainer>();

		// GD.Print($"[CardUI] EnterTree");
		MouseEntered += StateMachine.OnMouseEntered;
		MouseExited += StateMachine.OnMouseExited;

		DropPointDetector.AreaEntered += OnAreaEntered;
		DropPointDetector.AreaExited += OnAreaExited;
	}

	public override void _Ready()
	{
		// GD.Print($"[CardUI] Ready");
		StateMachine.InitStates(this);
	}

	public override void _ExitTree()
	{
		MouseEntered -= StateMachine.OnMouseEntered;
		MouseExited -= StateMachine.OnMouseExited;

		DropPointDetector.AreaEntered -= OnAreaEntered;
		DropPointDetector.AreaExited -= OnAreaExited;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		StateMachine.OnInputEvent(@event);
		// GD.Print($"[CardUI] Input {@event.GetType()}");
	}

	public override void _GuiInput(InputEvent @event)
	{
		// GD.Print($"[CardUI] GuiInput {@event.GetType()}");
		StateMachine.OnGuiInputEvent(@event);
	}

	public void OnAreaEntered(Area2D area)
	{
		GD.Print($"[CardUI] AreaEntered {area.Name}");
		if (area.Name == "DropArea")
		{
			IsOnDropArea = true;
		}
	}

	public void OnAreaExited(Area2D area)
	{
		GD.Print($"[CardUI] AreaExited {area.Name}");
		if (area.Name == "DropArea")
		{
			IsOnDropArea = false;
		}
	}

	#region Assets
	public bool MustChooseTarget()
	{
		return CardData.Target == Card.CardTarget.SINGLE_ENEMY;
	}
	#endregion

	#region Action
	public async Task AnimateToPosition(Vector2 pos, float duration = 0.5f)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "global_position", pos, duration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		await ToSignal(tween, Tween.SignalName.Finished);
	}
	#endregion
}
