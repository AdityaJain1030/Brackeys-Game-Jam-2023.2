using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podium : MonoBehaviour
{
    // public GameObject thingsToTrigger;
    [NonSerialized] public bool isTriggered = false;
    public float triggerRadius;
    public GameObject candle;
    // Start is called before the first frame update
    void Start()
    {
        candle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            candle.SetActive(true);
        }
    }

    //on fixedUpdate, draw an overlap spehre to check if the player is within the trigger radius
    private void FixedUpdate()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, triggerRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                isTriggered = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
