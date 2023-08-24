using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBladeAnimation : BaseAnimation
{
    // [SerializeField] GameObject playerMovementScript;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    private PlayerMovement playerMovementScript;
    private PlayerCombat playerCombatScript;
    public override void Awake() {
        base.Awake();
        playerMovementScript = player.GetComponent<PlayerMovement>();
        playerCombatScript = player.GetComponent<PlayerCombat>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setBladeState();
    }
    
    void setBladeState() {
        // animator.SetBool("inwardSlash", playerCombatScript.IsInwardSlashing);
        // animator.SetBool("outwardSlash", playerCombatScript.IsOutwardSlashing);
        // animator.SetBool("acrossSlash", playerCombatScript.IsAcrossSlashing);

        if (playerMovementScript.IsFacingRight) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
        
        if (playerCombatScript.IsInwardSlashing)  {
            animator.SetTrigger("inwardSlash");
            // Debug.Log("INWARDS BLADE");
        } else if (playerCombatScript.IsOutwardSlashing) {
            animator.SetTrigger("outwardSlash");
            // Debug.Log("OUTWARDS BLADE");
        } else if (playerCombatScript.IsAcrossSlashing) {
            animator.SetTrigger("acrossSlash");
            // Debug.Log("MIDDLE BLADE");
        }
    }
}
