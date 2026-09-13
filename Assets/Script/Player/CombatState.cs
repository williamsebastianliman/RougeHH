using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatState : PlayerState
{
    public CombatState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void EnterState()
    {
        playerMovement.clearSavedEnemy();
        playerMovement.clearList();
    }

    public override void UpdateState()
    {
        playerMovement.movePlayerCombat();
    }

    public override void HandleInput(InputAction.CallbackContext context)
    {
        
    }
}
