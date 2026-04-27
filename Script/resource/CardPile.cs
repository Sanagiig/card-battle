using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class CardPile : Resource
{
	[Signal]
	public delegate void CardPileSizeChangedEventHandler(int cardAmount);

	[Export]
	public Array<Card> Cards = new Array<Card>();

	public bool IsEmpty()
	{
		return Cards.Count == 0;
	}

	public Card DrawCard()
	{
		var card = Cards[0];
		Cards.RemoveAt(0);

		EmitSignal(SignalName.CardPileSizeChanged, Cards.Count);
		return card;
	}

	public void AddCard(Card card)
	{
		Cards.Append(card);
		EmitSignal(SignalName.CardPileSizeChanged, Cards.Count);
	}

	public void Shuffle()
	{
		Cards.Shuffle();
	}
}
