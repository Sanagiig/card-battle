using Godot;
using System;

public partial class BaseCardState : Node
{
	public virtual void Enter() { }

	public virtual void Update(double delta) { }

	public virtual void Exit() { }

	public virtual void OnInput(InputEvent @event) { }
	
	public virtual void OnGuiInput(InputEvent @event) { }

	public virtual void OnMouseEntered() { }

	public virtual void OnMouseExited() { }
}
