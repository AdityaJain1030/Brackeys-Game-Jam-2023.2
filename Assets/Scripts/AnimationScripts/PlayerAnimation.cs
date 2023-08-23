using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : BaseAnimation
{
    private PlayerMovement playerMovementScript;
    public override void Awake() {
        base.Awake();
        playerMovementScript = GetComponent<PlayerMovement>();
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
        Debug.Log(playerMovementScript.IsRunning);
        animator.SetBool("isRunning", playerMovementScript.IsRunning);
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
    }
}
