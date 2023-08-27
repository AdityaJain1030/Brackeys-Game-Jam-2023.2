using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] public float health;
    [SerializeField] public float lightGiven;

    [SerializeField] public Animator anim;

    [SerializeField] GameObject particlePrefab;
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
            GameObject instantiated = Instantiate(particlePrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            Debug.Log(instantiated.name);
            ParticleSystem p = instantiated.GetComponent<ParticleSystem>();
            ParticleSystem.TriggerModule tm = p.trigger;
            tm.AddCollider(GameObject.Find("ParticleCollector").GetComponent<BoxCollider2D>());
            instantiated.GetComponent<ParticleLightCollection>().player = GameObject.Find("Player");
            p.Play();
            // Destroy(gameObject);
        }
    }
}
