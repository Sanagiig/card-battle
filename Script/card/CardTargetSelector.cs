using Godot;
using System;

public partial class CardTargetSelector : Node2D
{

	public Area2D MotionArea { get; private set; }

	public Line2D CardArc { get; private set; }

	public Sprite2D ArrowSprite { get; private set; }

	public CardUI CurUsingCard { get; private set; }

	public Vector2 CurMotionPosition { get; private set; } = Vector2.Zero;

	private Enemy _CurTargetEnemy;

	public bool IsMotionOnDropArea = false;
	public override void _EnterTree()
	{
		MotionArea ??= GetNode<Area2D>("MotionArea");
		CardArc ??= GetNode<Line2D>("CardArc");
		ArrowSprite ??= GetNode<Sprite2D>("ArrowSprite");

		// GD.Print($"CardTargetSelector EnterTree {EventHub.Instance}");

		EventHub.Instance.CardAimPositionChanged += _OnCardAimPositionChanged;
		EventHub.Instance.CardAimEnded += _OnCardAimEnded;
		EventHub.Instance.CardAimStarted += _OnCardAimStarted;

		MotionArea.AreaEntered += _OnMotionAreaEntered;
		MotionArea.AreaExited += _OnMotionAreaExited;
	}

	public override void _Ready()
	{
		ArrowSprite ??= GetNode<Sprite2D>("ArrowSprite");
		Hide();
	}

	public override void _ExitTree()
	{
		EventHub.Instance.CardAimStarted -= _OnCardAimStarted;
		EventHub.Instance.CardAimEnded -= _OnCardAimEnded;
		EventHub.Instance.CardAimPositionChanged -= _OnCardAimPositionChanged;

		MotionArea.AreaEntered -= _OnMotionAreaEntered;
		MotionArea.AreaExited -= _OnMotionAreaExited;
	}

	#region Event Handlers
	private void _OnCardAimStarted(CardUI cardUI)
	{
		CurUsingCard = cardUI;
		MotionArea.Monitoring = true;
		_CurTargetEnemy = null;
		Show();
	}

	private void _OnCardAimEnded(CardUI cardUI)
	{
		CurUsingCard = null;
		MotionArea.Monitoring = false;
		Hide();
	}

	private void _OnCardAimPositionChanged(Vector2 position)
	{
		if (CurUsingCard == null)
		{
			return;
		}

		CurMotionPosition = position;
		MotionArea.GlobalPosition = position;
		UpdateArcLine(position);
	}

	private void _OnMotionAreaEntered(Area2D area)
	{
		GD.Print($"[CardTargetSelector] MotionAreaEntered {area.Name}");

		IsMotionOnDropArea = true;

		CardArc.DefaultColor = Colors.White;
		ArrowSprite.Show();
		EventHub.Instance.EmitSignal(EventHub.SignalName.GlobalMotionEnteredDropArea);
	}

	private void _OnMotionAreaExited(Area2D area)
	{
		// GD.Print($"[CardTargetSelector] MotionAreaExited {area.Name}");
		CardArc.DefaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		IsMotionOnDropArea = false;
		ArrowSprite.Hide();
		EventHub.Instance.EmitSignal(EventHub.SignalName.GlobalMotionExitedDropArea);
	}
	#endregion

	public Enemy GetClosestEnemy()
	{
		var enemyArr = EnemyManager.Instance.Enemies;
		Enemy closestEnemy = null;
		float closestDistance = float.MaxValue;

		foreach (var enemy in enemyArr)
		{
			if (closestEnemy == null)
			{
				closestEnemy = enemy;
				closestDistance = closestEnemy.GlobalPosition.DistanceTo(CurMotionPosition);
			}
			else
			{
				var newDis = enemy.GlobalPosition.DistanceTo(CurMotionPosition);
				if (newDis < closestDistance)
				{
					closestEnemy = enemy;
					closestDistance = newDis;
				}
			}
		}

		return closestEnemy;
	}

	public void UpdateArcLine(Vector2 position)
	{
		var enemy = GetClosestEnemy();

		if (enemy == null)
		{
			GD.PrintErr("[CardTargetSelector] No enemy found");
			return;
		}


		CardArc.ClearPoints();
		var enemyTargetPos = enemy.GetDownCenterGlobalPosition();
		var startPoint = CurUsingCard.GlobalPosition;

		startPoint.X += CurUsingCard.Size.X / 2;

		// 离开 DropArea
		if (!IsMotionOnDropArea)
		{
			for (int i = 0; i < 20; i++)
			{
				var t = i / 20f;
				var point = Arc.GetQuadraticBezierPoint(startPoint, position, startPoint + new Vector2(0, -100), t);
				CardArc.AddPoint(point);
			}
			return;
		}

		AnimateUpdateArrowPos(enemyTargetPos);
		
		for (int i = 0; i < 20; i++)
		{
			var t = i / 20f;
			var point = Arc.GetQuadraticBezierPoint(startPoint, enemyTargetPos, position, t);
			CardArc.AddPoint(point);
		}

		_CurTargetEnemy = enemy;
	}

	public void AnimateUpdateArrowPos(Vector2 pos)
	{
		ArrowSprite.Position = pos;
		// if (_CurTargetEnemy == null)
		// {
		// 	ArrowSprite.Position = pos;
		// 	return;
		// }

		// var tween = GetTree().CreateTween();
		// tween.TweenProperty(ArrowSprite, "global_position", pos, 0.1f)
		// 	.SetTrans(Tween.TransitionType.Sine)
		// 	.SetEase(Tween.EaseType.InOut);
	}
}
