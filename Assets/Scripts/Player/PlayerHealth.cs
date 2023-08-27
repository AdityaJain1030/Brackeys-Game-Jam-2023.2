using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public GameObject healthUI;
    private HealthBar healthBarScript;
    // Start is called before the first frame update
    public PlayerData config;

    public float health { get; private set; }

    void Awake() {
        healthBarScript = healthUI.GetComponent<HealthBar>();
    }
    void Start()
    {
        health = config.maxHealth;
    }

    //Handle Drawing the health to screen
    void Update()
    {
        
    }

    public void inflictDamage(float damage) {
        health -= damage;
        if (health < 0) {
            healthBarScript.depleteHealth(health * -1);
            health = 0.0f;
        } else {
            healthBarScript.depleteHealth(damage); 
        }
    }
}
