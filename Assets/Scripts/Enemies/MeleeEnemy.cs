using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float damage;
    

    [Header ("Collider Parameters")]

    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    
    private Animator anim;

    private PlayerHealth playerHealth;

    private EnemyPatrol enemyPatrol;
    
    private void Awake() {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame a
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (playerInSight()) 
        {
            if (cooldownTimer >= attackCooldown) 
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (enemyPatrol != null) 
            enemyPatrol.enabled = !playerInSight();
    }

    private bool playerInSight() {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
        0, 
        Vector2.left, 
        0, 
        playerLayer);

        if (hit.collider != null) {
            Debug.Log("COLLIDER != NULL");
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        }
        return hit.collider != null;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
        boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
        // Debug.Log("DRAWING GIZMOS!!!!");
    }

    private void DamagePlayer() 
    {
        if (playerInSight()) 
        {
            playerHealth.inflictDamage(damage);
            // Damage player
        }
    }
}
