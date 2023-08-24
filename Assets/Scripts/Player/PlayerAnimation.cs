using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    private PlayerMovement playerMovementScript;

    private PlayerCombat playerCombatScript;
    public override void Awake() {
        base.Awake();
        playerMovementScript = GetComponent<PlayerMovement>();
        playerCombatScript = GetComponent<PlayerCombat>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        setPlayerState();
    }
 
    void setPlayerState() {
        // Debug.Log(playerMovementScript.IsRunning);
        animator.SetBool("isRunning", playerMovementScript.IsRunning);

        animator.SetBool("slashingInwards", playerCombatScript.IsInwardSlashing);
        animator.SetBool("slashingOutwards", playerCombatScript.IsOutwardSlashing);
        animator.SetBool("slashingAcross", playerCombatScript.IsAcrossSlashing);

        if (playerMovementScript.IsRunning) {
            spriteRenderer.material = materials[1];
        } else {
            spriteRenderer.material = materials[0];
        }
        
        if (playerMovementScript.IsFacingRight) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }

        if (playerCombatScript.IsInwardSlashing) {
            // Debug.Log("INWARDS");
            spriteRenderer.material = materials[2];
        } else if (playerCombatScript.IsOutwardSlashing) {
            spriteRenderer.material = materials[3];
        } else if (playerCombatScript.IsAcrossSlashing) {
            spriteRenderer.material = materials[4];
        }
    }
}
