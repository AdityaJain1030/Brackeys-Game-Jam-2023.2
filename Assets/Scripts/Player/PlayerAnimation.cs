using System.Collections;
using System.Collections.Generic;
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
        } else if (playerCombatScript.IsInwardSlashing) {
            spriteRenderer.material = materials[2];
        } else if (playerCombatScript.IsOutwardSlashing) {
            spriteRenderer.material = materials[3];
        } else if (playerCombatScript.IsAcrossSlashing) {
            spriteRenderer.material = materials[4];
        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AdventurerIdle")) {
            spriteRenderer.material = materials[0];
        }
        
        if (playerMovementScript.IsFacingRight) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }
}
