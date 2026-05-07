using Godot;
using System;

public partial class Player : BaseCharacter
{
  public override void _Ready()
  {
    // todo round started
    base._Ready();
    EventHub.Instance.EmitSignal(EventHub.SignalName.RoundStarted, this);
  }
}
