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

        Debug.Log(playerCombatScript.IsInwardSlashing + " ");
        if (playerMovementScript.IsRunning) {
            spriteRenderer.material = materials[1];
        } else if (playerCombatScript.IsInwardSlashing) {
            spriteRenderer.material = materials[2];
            Debug.Log(materials[2].name);
        } else if (playerCombatScript.IsOutwardSlashing) {
            spriteRenderer.material = materials[3];
            Debug.Log(materials[3].name);
        } else if (playerCombatScript.IsAcrossSlashing) {
            spriteRenderer.material = materials[4];
            Debug.Log(materials[4].name);
        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AdventurerIdle")) {
            spriteRenderer.material = materials[0];
        }
        
        if (playerMovementScript.IsFacingRight) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }

        // if (playerCombatScript.IsInwardSlashing) {
        //     // Debug.Log("INWARDS");
        //     spriteRenderer.material = materials[2];
        // }
        // if (playerCombatScript.IsOutwardSlashing) {
        //     spriteRenderer.material = materials[3];
        // }
        // if (playerCombatScript.IsAcrossSlashing) {
        //     spriteRenderer.material = materials[4];
        // }
    }
}
