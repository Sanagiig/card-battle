using Godot;
using System;

public partial class Card : Resource
{
  public enum CardType
  {
    ATTACK,
    SKILL,
    POWER,
  }

  public enum CardTarget
  {
    SELF,
    SINGLE_ENEMY,
    ALL_ENEMY,
    EVERYONE,
  }

  [ExportGroup("Card Attributes")]
  [Export]
  public string Id;

  [Export]
  public CardType Type;

  [Export]
  public CardTarget Target;

  public bool IsSingleTarget(){
    return Target == CardTarget.SELF;
  }
}
