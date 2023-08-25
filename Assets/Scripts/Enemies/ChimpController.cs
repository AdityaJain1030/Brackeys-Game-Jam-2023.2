using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimpController : MonoBehaviour
{
    public Transform _pointA, _pointB;
    public float walkSpeed;
    public float runSpeed;
    public bool _isRight = true;
    public bool _hangingFromWall = false;
    public Rigidbody2D rigidBody;

    public float unidirectionalDetectionRadius;
    public float omnidirectionalDetectionRadius;
    public float followRadius;

    public Transform frontLedgeDetector, backLedgeDetector;
    public Vector2 frontLedgeDetectorSize;
    public Vector2 backLedgeDetectorSize;

    public LayerMask groundLayer;
    public SpriteRenderer spriteRenderer;

    public GameObject boundsChecksGameObject;

    //ENEMY EXPLANATION
    // This enemy is a chimp that hangs from the ceiling and moves back and forth
    // When the player enters the unidirectional direction radius or the omnidirectional detection radius
    // The chimp will turn around and jump on the player
    // While the player is in the follow radius, the chimp will follow the player, as long as the chimp can avoid falling off the platform
    // If the player leaves the follow radius, the chimp will cycle between whatever platform it is currently on
    // If the chimp is within an attack radius, it will attack the player
    // If the chimp is not within an attack radius, it will move towards the player
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Move(walkSpeed, _isRight ? 1 : -1);
        if (!Physics2D.OverlapBox(frontLedgeDetector.position, frontLedgeDetectorSize, 0, groundLayer) && 
        Physics2D.OverlapBox(backLedgeDetector.position, backLedgeDetectorSize, 0, groundLayer)) {
            Turn();
            _isRight = !_isRight;
        }

        // flip gravity for hanging from ceiling
        if (_hangingFromWall)
        {
            rigidBody.gravityScale = -rigidBody.gravityScale;
        }
    }

    public void Move(float speed, int direction) {
        rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);
    }

    public void Turn()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        Vector3 scale = boundsChecksGameObject.transform.localScale; 
		scale.x *= -1;
		boundsChecksGameObject.transform.localScale = scale;
    }

    //draw gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unidirectionalDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, omnidirectionalDetectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(frontLedgeDetector.position, frontLedgeDetectorSize);
        Gizmos.DrawWireCube(backLedgeDetector.position, backLedgeDetectorSize);
    }
}
