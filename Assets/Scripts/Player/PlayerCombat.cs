using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] public AudioSource swordSwooshSFX;
	public bool IsInwardSlashing { get; private set; }
	public bool IsOutwardSlashing { get; private set; }
	public bool IsAcrossSlashing { get; private set; }
    // Start is called before the first frame update
    
    private int comboSequence = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsInwardSlashing = false;
        IsOutwardSlashing = false;
        IsAcrossSlashing = false;
        if (Input.GetMouseButtonDown(0)) {
            if (comboSequence == 0) {
                IsInwardSlashing = true;
                IsOutwardSlashing = false;
                IsAcrossSlashing = false;
            } else if (comboSequence == 1) {
                IsInwardSlashing = false;
                IsOutwardSlashing = true;
                IsAcrossSlashing = false;
            } else if (comboSequence == 2) {
                IsInwardSlashing = false;
                IsOutwardSlashing = false;
                IsAcrossSlashing = true;
                comboSequence = -1;
            }
            comboSequence++;
        }
    }
}
