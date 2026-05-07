using Godot;
using System;

public partial class CharacterStats : Stats
{
	[Export]
	public CardPile StartingDeck { get; private set; }

	[Export]
	public int CardsPerTurn { get; private set; } = 5;

	[Export]
	public int MaxActionPoints { get; private set; } = 5;

	[Export]

	public int ActionPoints { get => _ActionPoints; private set => SetActionPoints(value); }
	private int _ActionPoints;

	public CardPile DeckPile { get; protected set; }
	public CardPile DiscardPile { get; protected set; } 
	public CardPile DrawPile { get; protected set; }



	public void SetActionPoints(int actionPoints)
	{
		_ActionPoints = actionPoints;
		EmitSignal(SignalName.StatsChanged);
	}

	public void ResetActionPoints()
	{
		ActionPoints = MaxActionPoints;
	}

	public bool CanPlayCard(Card card){
		return _ActionPoints >= card.Cost;
	}

	public override CharacterStats CreateInstance()
	{
		// todo : 需要确认duplicate 是否能够正常复制资源，instance 是否正确
		var instance = base.CreateInstance() as CharacterStats;

		instance._ActionPoints = MaxActionPoints;
		instance.DeckPile = instance.StartingDeck.Duplicate() as CardPile;
		instance.DrawPile = new CardPile();
		instance.DiscardPile = new CardPile();
		return instance;
	}

	public void EmitChange(){
			EmitSignal(SignalName.StatsChanged);
	}
}
