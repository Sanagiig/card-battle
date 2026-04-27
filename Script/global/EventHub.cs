using Godot;
using System;

public partial class EventHub : Node
{	
	#region Card Events
	[Signal]
	public delegate void CardResetRequestedEventHandler(CardUI cardUI);

	[Signal]
	public delegate void CardAimStartedEventHandler(CardUI cardUI);

	[Signal]
	public delegate void CardAimEndedEventHandler(CardUI cardUI);

	[Signal]
	public delegate void CardAimPositionChangedEventHandler(Vector2 position);

	#endregion

	#region Enemy Events
	[Signal]
	public delegate void EnemySpawnedEventHandler(Enemy enemy);

	[Signal]
	public delegate void EnemyDestroyedEventHandler(Enemy enemy);
	#endregion

	#region Global Motion Events
	[Signal]
	public delegate void GlobalMotionEnteredDropAreaEventHandler();

	[Signal]
	public delegate void GlobalMotionExitedDropAreaEventHandler();
	#endregion
	public static EventHub Instance { get; private set; }

	public override void _EnterTree()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GD.PrintErr("[EventHub] Instance already exists");
		}
	}
}
