using Godot;
using System;
using System.Threading.Tasks;

public partial class BaseState : CardState
{
  public override async void Enter()
  {
    base.Enter();
    await base.Init();

    CardUI.PivotOffset = Vector2.Zero;
    CardUI.DropPointDetector.Monitoring = false;
    CardUI.ToNormalStyle();

    EventHub.Instance.EmitSignal(EventHub.SignalName.CardResetRequested, CardUI);

    EventHub.Instance.CardAimStarted += _OnOtherCardAimStarted;
    EventHub.Instance.CardAimEnded += _OnOtherCardAimEnded;
  }

  public override void Update(double delta)
  {
    base.Update(delta);
  }

  public override void Exit()
  {
    base.Exit();
    EventHub.Instance.CardAimStarted -= _OnOtherCardAimStarted;
    EventHub.Instance.CardAimEnded -= _OnOtherCardAimEnded;
  }

  public override void OnInput(InputEvent @event)
  {
    base.OnInput(@event);
  }

  public override void OnGuiInput(InputEvent @event)
  {
    base.OnGuiInput(@event);
    if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
    {
      GD.Print($"mouseButton.Position {mouseButton.Position}");
      CardUI.PivotOffset = mouseButton.Position;
      EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.CLICKED);

    }
    else if (@event is InputEventScreenTouch screenTouch && screenTouch.Pressed)
    {
      CardUI.PivotOffset = screenTouch.Position;
      EmitSignal(SignalName.TransitionRequested, (int)State, (int)StateEnum.CLICKED);
    }
  }

  public override void OnMouseEntered()
  {
    base.OnMouseEntered();
  }

  public override void OnMouseExited()
  {
    base.OnMouseExited();
  }

  private void _OnOtherCardAimStarted(CardUI cardUI)
  {
    if(cardUI != CardUI){
      CardUI.MouseFilter = Control.MouseFilterEnum.Ignore;
    }
  }
  private void _OnOtherCardAimEnded(CardUI cardUI)
  {
     if(cardUI != CardUI){
      CardUI.MouseFilter = Control.MouseFilterEnum.Stop;
    }
  }
}
