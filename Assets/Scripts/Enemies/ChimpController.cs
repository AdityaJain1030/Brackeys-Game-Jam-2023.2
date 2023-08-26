using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChimpController : BaseAnimation
{
    public Transform _rightPoint, _leftPoint;
    public float walkSpeed;
    public float runSpeed;
    public bool startingRight = true;
    public bool spawnUpsideDown = false;
    public float unidirectionalDetectionRadius;
    public float omnidirectionalDetectionRadius;
    public float followRadius;
    public float attackRadius;

    public float delayAfterJumpDownAttack = 0.5f;
    public float delayAfterFarDetection = 1f;
    public float delayAfterCloseDetection = 0.5f;
    public float delayBeforeAttack = 0.3f;

    public Transform frontLedgeDetector, backLedgeDetector, frontWallChecker;
    public Vector2 frontLedgeDetectorSize;
    public Vector2 backLedgeDetectorSize;
    public Vector2 frontWallCheckerSize;
    public Rigidbody2D rigidBody;

    public LayerMask groundLayer;
    public LayerMask enemyAlertLayer;
    public GameObject boundsChecksGameObject;
    public bool _isHanging { get; private set; }
    public bool _isRight { get; private set; }
    public bool _isPatrolling { get; private set; } = true;
    public bool _isHuntingPlayer { get; private set; } = false;
    public bool _leftInitialZone { get; private set; } = false;
    public bool _isInAir { get; private set; } = false;

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
        _isRight = startingRight;
        _isHanging = spawnUpsideDown;
        if (_isHanging)
        {
            Flip();
            rigidBody.gravityScale = -rigidBody.gravityScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_isPatrolling)
        {
            CheckForPlayer();
            cycleBackAndForth();
        }

        if (_isInAir && rigidBody.velocity.y == 0) JumpFinished();

        if (_isHuntingPlayer) HuntPlayer();


        if (!_isHuntingPlayer && !_isPatrolling && !_isInAir)
            Move(0, 0);
    }

    public IEnumerator Attack()
    {
        //Windup animation
        //freeze enemy im place
        _isHuntingPlayer = false;
        //wait for player reaction
        yield return new WaitForSeconds(delayBeforeAttack);

        //start attack
        animator.SetTrigger("Attack");
    }

    public void AttackFinished()
    {
        _isHuntingPlayer = true;
    }

    public void JumpFinished()
    {
        _isInAir = false;
        rigidBody.velocity = new Vector2(0, 0);
        _isHuntingPlayer = true;
    }

    //get distance from transform using pythagorean theorem
    public float Distance(Transform transform)
    {
        return Mathf.Sqrt(Mathf.Pow(transform.position.x - this.transform.position.x, 2) + Mathf.Pow(transform.position.y - this.transform.position.y, 2));
    }

    public void HuntPlayer()
    {
        //check if player is in follow range
        Collider2D playerInFollowRadius = Physics2D.OverlapCircle(transform.position, followRadius, enemyAlertLayer);
        if (playerInFollowRadius)
        {
            // face the player
            if (PlayerDirectionRelativeToChimp(playerInFollowRadius.transform) != _isRight)
            {
                Turn();
                _isRight = !_isRight;
            }

            //check if player is in attack range
            Collider2D playerInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius, enemyAlertLayer);
            if (playerInAttackRadius)
            {
                //attack player
                StartCoroutine(Attack());
                return;
            }

            //if chimp on ledge end patrol
            if (ShouldTurn())
            {
                _isHuntingPlayer = false;
                _isPatrolling = true;

                //decrease detection radius to smaller than distance from player if player is in attack radius
                // if (omnidirectionalDetectionRadius >= Distance(playerInFollowRadius.transform))
                // {
                //     omnidirectionalDetectionRadius = Distance(playerInFollowRadius.transform) * .75f;
                // }
                // if (unidirectionalDetectionRadius >= Distance(playerInFollowRadius.transform))
                // {
                //     unidirectionalDetectionRadius = Distance(playerInFollowRadius.transform) * .75f;
                // }
                return;
            }

            // move towards player if not in attck range
            Move(runSpeed, _isRight ? 1 : -1);
        }
        else
        {
            //go back to patrolling
            _isHuntingPlayer = false;
            _isPatrolling = true;
        }
    }

    public bool ShouldTurn()
    {
        return (!Physics2D.OverlapBox(frontLedgeDetector.position, frontLedgeDetectorSize, 0, groundLayer) && 
            Physics2D.OverlapBox(backLedgeDetector.position, backLedgeDetectorSize, 0, groundLayer)) || 
            Physics2D.OverlapBox(frontWallChecker.position, frontWallCheckerSize, 0, groundLayer);
    }

    public void cycleBackAndForth()
    {
        // turn on initial left and right if in initial zone
        if (!_leftInitialZone && _rightPoint && _leftPoint)
        {

            //dont move at all if leftPoint and rightPoint are the same
            if (_leftPoint == _rightPoint) return;

            //turn if past rightPoint
            if (transform.position.x >= _rightPoint.position.x)
            {
                Turn();
                _isRight = !_isRight;
            }
            //turn if past leftPoint
            if (transform.position.x <= _leftPoint.position.x)
            {
                Turn();
                _isRight = !_isRight;
            }
        }

        //turn on ledge
        if (ShouldTurn())
        {
            Turn();
            _isRight = !_isRight;
        }

        Move(walkSpeed, _isRight ? 1 : -1);
    }

    // returns true if right else false for left
    private bool PlayerDirectionRelativeToChimp(Transform playerTransform)
    {
        if (playerTransform.position.x > transform.position.x)
            return true;

        return false;
    }

    private float PlayerYDistanceRelativeToChimp(Transform playerTransform)
    {
        if (playerTransform.position.y > transform.position.y)
            return -1f;

        return transform.position.y - playerTransform.position.y;
    }

    private IEnumerator JumpDown(float waitSecs)
    {
        // invalidate left and right initial zone
        _leftInitialZone = true;

        //start idle animation
        animator.SetTrigger("Idle");

        //player reaction wait
        yield return new WaitForSeconds(waitSecs);

        //if player still valid jump down attack, else go back to patrolling
        Collider2D playerInFollowRadius = Physics2D.OverlapCircle(transform.position, followRadius, enemyAlertLayer);
        if (playerInFollowRadius)
        {
            animator.SetTrigger("Jump");
            // find the right x velocity to reach player
            float xDistance = playerInFollowRadius.transform.position.x - transform.position.x;
            float yDistance = PlayerYDistanceRelativeToChimp(playerInFollowRadius.transform);

            //find time to reach player
            float timeToReachPlayer = Mathf.Sqrt(2 * yDistance / Physics2D.gravity.y * rigidBody.gravityScale);

            //find x velocity to reach player
            float xVelocity = xDistance / timeToReachPlayer;
            // Debug.Log(xVelocity);
            _isHanging = false;

            //set velocity
            Flip();
            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
            rigidBody.gravityScale = -rigidBody.gravityScale;
            _isInAir = true;
        }


        //run hunter check/sequence
        yield return StartHunterMode(delayAfterJumpDownAttack);
    }

    private IEnumerator StartHunterMode(float waitSecs)
    {
        //start idle animation
        // yield return null;
        animator.SetTrigger("Idle");

        //player reaction wait
        yield return new WaitForSeconds(waitSecs);

        // check if player still within valid
        Collider2D playerInFollowRadius = Physics2D.OverlapCircle(transform.position, followRadius, enemyAlertLayer);
        if (playerInFollowRadius)
        {
            _isHuntingPlayer = true;
        }
        else
        {
            _isPatrolling = true;
        }
    }

    private void CheckForPlayer()
    {
        Collider2D playerInOmniDirectionalRadius = Physics2D.OverlapCircle(transform.position, omnidirectionalDetectionRadius, enemyAlertLayer);
        Collider2D playerInUniDirectionalRadius = Physics2D.OverlapCircle(transform.position, unidirectionalDetectionRadius, enemyAlertLayer);
        // Debug.Log(playerInOmniDirectionalRadius);
        // Debug.Log(playerInUniDirectionalRadius);
        // if player is in omnidirectional detection radius, turn around
        // dont do anything if player is above chimp
        if (playerInOmniDirectionalRadius && (PlayerYDistanceRelativeToChimp(playerInOmniDirectionalRadius.transform) == 0))
        {
            Debug.Log("HI");
            if (PlayerDirectionRelativeToChimp(playerInOmniDirectionalRadius.transform) != _isRight)
            {
                Turn();
                _isRight = !_isRight;
            }
            _isPatrolling = false;
            if (_isHanging)
            {
                //Jump down through coroutine
                StartCoroutine(JumpDown(delayAfterCloseDetection));
            }
            return;
        }

        // if player is in unidirectional detection radius and facing right direction
        if (playerInUniDirectionalRadius)
        {
            if (PlayerDirectionRelativeToChimp(playerInUniDirectionalRadius.transform) == _isRight &&
        PlayerYDistanceRelativeToChimp(playerInUniDirectionalRadius.transform) >= 0)
            {
            Debug.Log("BYE");

                _isPatrolling = false;
                if (_isHanging)
                {
                    //Jump down through coroutine
                    StartCoroutine(JumpDown(delayAfterFarDetection));
                }
                return;
            }
        }

        // patrol if player is not in either radius
        _isPatrolling = true;
    }

    public void Move(float speed, int direction)
    {
        animator.SetTrigger("Crawl");
        int upsideDown = _isHanging ? -1 : 1;
        rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);
    }

    public void Turn()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        Vector3 scale = boundsChecksGameObject.transform.localScale;
        scale.x *= -1;
        boundsChecksGameObject.transform.localScale = scale;
    }

    public void Flip()
    {
        spriteRenderer.flipY = !spriteRenderer.flipY;
        Vector3 position = boundsChecksGameObject.transform.localPosition;
        position.y *= -1;
        boundsChecksGameObject.transform.localPosition = position;

    }

    //draw gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, omnidirectionalDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, unidirectionalDetectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(frontLedgeDetector.position, frontLedgeDetectorSize);
        Gizmos.DrawWireCube(backLedgeDetector.position, backLedgeDetectorSize);
        Gizmos.DrawWireCube(frontWallChecker.position, frontWallCheckerSize);
    }   
}
