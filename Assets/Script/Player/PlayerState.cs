using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerState
{
    protected PlayerMovement playerMovement;
    public PlayerState(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void HandleInput(InputAction.CallbackContext context) { }
}
