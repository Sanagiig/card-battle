using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public partial class CardUI : Control
{
	private static StyleBoxFlat CardNormalStyle = ResourceLoader.Load<StyleBoxFlat>("res://Resource/card/ui/CardNormalStyle.tres");

	private static StyleBoxFlat CardDragStyle = ResourceLoader.Load<StyleBoxFlat>("res://Resource/card/ui/CardDragStyle.tres");

	private static StyleBoxFlat CardAimingStyle = ResourceLoader.Load<StyleBoxFlat>("res://Resource/card/ui/CardAimingStyle.tres");

	[Export]
	public Card CardData;

	public Panel CardOuterPanel { get; private set; }

	public Panel CardInnerPanel { get; private set; }

	public Label StateLabel { get; private set; }

	public Label CostLabel { get; private set; }

	public TextureRect CardIcon { get; private set; }

	public Area2D DropPointDetector { get; private set; }

	public CardStateMachine StateMachine { get; private set; }

	public HBoxContainer CardContainer { get; private set; }

	public bool IsOnDropArea { get; private set; }

	public override void _EnterTree()
	{
		CardOuterPanel ??= GetNode<Panel>("CardOuterPanel");
		CardOuterPanel ??= GetNode<Panel>("CardOuterPanel/MarginContainer/CardInnerPanel");

		StateLabel ??= GetNode<Label>("CardOuterPanel/MarginContainer/CardInnerPanel/StateLabel");
		CostLabel ??= GetNode<Label>("CardOuterPanel/MarginContainer/CardInnerPanel/CostLabel");

		CardIcon ??= GetNode<TextureRect>("CardOuterPanel/MarginContainer/CardInnerPanel/CardIcon");
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
		if (CardData == null)
		{
			GD.PrintErr($"[CardUI] CardData is null");
			return;
		}
		SetCardData(CardData);
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

	#region  Events
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
	#endregion

	#region Assets
	public bool MustChooseTarget()
	{
		return CardData.Target == Card.CardTarget.SINGLE_ENEMY;
	}
	#endregion

	#region Action
	public void SetCardData(Card card)
	{
		CardData = card;
		// StateLabel.Text = "";
		CostLabel.Text = card.Cost.ToString();
		CardIcon.Texture = card.Image;
	}

	public async Task AnimateToPosition(Vector2 pos, float duration = 0.5f)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "global_position", pos, duration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		await ToSignal(tween, Tween.SignalName.Finished);
	}

	public void ToNormalStyle()
	{
		CardOuterPanel.AddThemeStyleboxOverride("panel", CardNormalStyle); ;
	}

	public void ToDragStyle()
	{
		CardOuterPanel.AddThemeStyleboxOverride("panel", CardDragStyle);
	}

	public void ToAimingStyle()
	{
		CardOuterPanel.AddThemeStyleboxOverride("panel", CardAimingStyle);
	}

	public void Play()
	{
		CardData.Apply();
		QueueFree();
	}
	#endregion
}
