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
    SINGLE_FRIEND,
    ALL_ENEMY,
    ALL_FRIEND,
    EVERYONE,
  }

  [ExportGroup("Card Attributes")]
  [Export]
  public string Id;

  [Export]
  public CardType Type;

  [Export]
  public CardTarget Target;

  [Export]
  public int Amount;

  [Export]
  public int Cost;

  [ExportGroup("Card Visuals")]
  [Export]
  public Texture2D Image;
  [Export(PropertyHint.MultilineText)]
  public string Description;

  public BaseCharacter[] CardTargetCharacters;

  public bool IsSingleTarget()
  {
    return Target == CardTarget.SELF;
  }

  #region  Get Info
  public int GetTargetCharacterCount()
  {
    return CardTargetCharacters == null ? 0 : CardTargetCharacters.Length;
  }
  #endregion

  #region Actions
  public BaseCharacter[] RefreshTargets()
  {
    switch (Target)
    {
      case CardTarget.SELF:
        CardTargetCharacters = [GameManager.Instance.CurrentTurnCharacter];
        break;
      case CardTarget.ALL_ENEMY:
        CardTargetCharacters = GameManager.Instance.IsPlayerTurn
          ? GameManager.Instance.GetAllEnemies()
          : GameManager.Instance.GetAllPlayers();
        break;
      case CardTarget.ALL_FRIEND:
        CardTargetCharacters = GameManager.Instance.IsPlayerTurn
          ? GameManager.Instance.GetAllPlayers()
          : GameManager.Instance.GetAllEnemies();
        break;
      case CardTarget.EVERYONE:
        CardTargetCharacters = GameManager.Instance.GetAllCharacters();
        break;
    }

    return CardTargetCharacters;
  }

  public virtual void Apply()
  {
    RefreshTargets();
    GD.Print($"[ {Type} ] Applied");
    GD.Print($"[ {Type} ] Targets: {GetTargetCharacterCount()}");
    GD.Print($"[ {Type} ] Targets: {CardTargetCharacters.Length}");
    GD.Print($"[ {Type} ] Targets: {CardTargetCharacters[0].Name}");
  }
  #endregion
}
