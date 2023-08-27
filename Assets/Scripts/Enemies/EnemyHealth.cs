using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] public float health;

    [SerializeField] public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void inflictDamage(float damage) {
        anim.SetTrigger("hurt");
        health -= damage;
        if (health < 0) {
            health = 0.0f;
            Destroy(gameObject);
        }
    }
}
