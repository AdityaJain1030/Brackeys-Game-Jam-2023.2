using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerData config;

    public float health { get; private set; }

    void Start()
    {
        health = config.maxHealth;
    }

    //Handle Drawing the health to screen
    void Update()
    {
        
    }
}
