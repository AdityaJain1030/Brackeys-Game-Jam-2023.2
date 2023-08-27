using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeCollider : MonoBehaviour
{
    [SerializeField] public BoxCollider2D bc;
    [SerializeField] private float range;
    [SerializeField] private float colliderHeight;
    [SerializeField] private float colliderDistance;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private SpriteRenderer sr;
    
    void Awake() {
        // sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(
        bc.bounds.center + transform.right * range * transform.localScale.x * colliderDistance * (sr.flipX ? -1 : 1), 
        new Vector3(bc.bounds.size.x * range, bc.bounds.size.y * colliderHeight, bc.bounds.size.z)
        );
        // Debug.Log("DRAWING GIZMOS!!!!");
    }
    
    private bool isEnemyContacted() {
        RaycastHit2D hit = Physics2D.BoxCast(bc.bounds.center + transform.right * range * transform.localScale.x * colliderDistance * (sr.flipX ? -1 : 1), 
        new Vector3(bc.bounds.size.x * range, bc.bounds.size.y * colliderHeight, bc.bounds.size.z), 
        0, 
        Vector2.left, 
        0, 
        enemyLayer);
        
        if (hit.collider != null) {
            return true;
        } else {
            return false;
        }
    }
    public void collideBlade() {
        RaycastHit2D hit = Physics2D.BoxCast(bc.bounds.center + transform.right * range * transform.localScale.x * colliderDistance * (sr.flipX ? -1 : 1), 
        new Vector3(bc.bounds.size.x * range, bc.bounds.size.y * colliderHeight, bc.bounds.size.z), 
        0, 
        Vector2.left, 
        0, 
        enemyLayer);
        
        if (hit.collider != null) {
            hit.collider.gameObject.GetComponent<EnemyHealth>().inflictDamage(1.0f);
            // return true;
        } else {
            // return false;
        }
    }
}
