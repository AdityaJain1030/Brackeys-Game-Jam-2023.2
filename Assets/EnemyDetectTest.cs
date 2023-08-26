using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectTest : MonoBehaviour
{
    [SerializeField] public float damage = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.tag );
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("inflictDamage", 0.5f);
        }
    }
}
