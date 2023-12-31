// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.Experimental.GraphView;
// using UnityEngine;
// public class PlayerMovement : MonoBehaviour
// {
//     //player config
// 	[SerializeField] private PlayerData config;	

//     public Rigidbody2D rigidBody;
//     public BoxCollider2D groundCollider;
//     public LayerMask groundLayer;

//     // player state 
//     [NonSerialized] public Vector2 _input = Vector2.zero;
//     [NonSerialized] public float _coyoteTimer = 0f;
//     [NonSerialized] public float _pressedJumpTimer = 0f;
//     [NonSerialized] public bool jumpCut = false;
//     [NonSerialized] public bool recievingPlayerInput = true;

//     // shared state
//     [SerializeField] private PlayerState ps;


//     private void Setup()
//     {
//         SetGravityScale(config.gravityScale);
//     }

//     private void Update()
//     {
//         _coyoteTimer -= Time.deltaTime;
//         _pressedJumpTimer -= Time.deltaTime;

//         bool doJump = false;

//         if (recievingPlayerInput)
//         {
//             (float x, float y, bool jump) = GetInput();
//             _input.x = x;
//             _input.y = y;
//             doJump = jump;
//         }

//         // if we arent grounded anymore and we were grounded before, start the coyote timer
//         //we can use jumpstate to check if we were grounded before as jumpState wont update till StateUpdate() is called
//         if (!isGrounded() && ps.jumpState == JumpState.GROUNDED)
//         {
//             _coyoteTimer = config.coyoteTime;
//         }

//         StateUpdate();

//         // start the jump buffer timer if we press jump
//         if (doJump)
//         {
//             _pressedJumpTimer = config.jumpInputBufferTime;
//         }

//         //release key
//         if (Input.GetKeyUp(KeyCode.Space))
//         {
//             // if the player let go of the jump button mid jump, jump cut
//             if (ps.jumpState == JumpState.JUMPING)
//             {
//                 jumpCut = true;
//             }
//         }

//     }

//     private void FixedUpdate()
//     {
//         Move(1);
//         // if we are grounded or in coyote time and press jump within the jump buffer window, jump
//         if ((_coyoteTimer > 0f || ps.jumpState == JumpState.GROUNDED) && _pressedJumpTimer > 0f)
//         {
//             Jump();
//         }
//         if (jumpCut)
//         {
//             CutJump();
//             jumpCut = false;
//         }
//     }

//     public void StateUpdate()
//     {
//         if (isGrounded())
//         {
//             ps.jumpState = JumpState.GROUNDED;
//         }
//         // if we were jumping and we are now at the apex of our jump, start apex sequence
//         if (ps.jumpState == JumpState.JUMPING && Mathf.Abs(rigidBody.velocity.y) <= config.jumpApexThreshold)
//         {
//             ps.jumpState = JumpState.APEX;
//         }
//         // if we were apexing or jumping still and we are now falling, start falling sequence
//         if (rigidBody.velocity.y < -config.jumpApexThreshold)
//         {
//             ps.jumpState = JumpState.FALLING;
//         }
//     }

// 	public void SetGravityScale(float scale)
//     {
//         rigidBody.gravityScale = scale;
//     } 

//     public bool isGrounded()
//     {
//         // get a sphere at the bottom of the player
//         Vector2 capsuleDims = groundCollider.size * (Vector2)transform.localScale;
//         float radius = 0.5f * Mathf.Min(capsuleDims.x, capsuleDims.y) - 0.1f;
//         Vector2 pos = (Vector2)transform.position + groundCollider.offset - Vector2.up * (Mathf.Max(capsuleDims.x, capsuleDims.y) * 0.5f - radius);

//         return Physics2D.OverlapCircle(pos, radius, groundLayer);
//     }

//     public (float, float, bool) GetInput()
//     {
//         float x = Input.GetAxisRaw("Horizontal");
//         float y = Input.GetAxisRaw("Vertical");
//         bool jump = Input.GetKeyDown(KeyCode.Space);

//         return (x, y, jump);
//     }

//     public void Turn(XDirection direction)
//     {
//         if (direction != ps.facing)
//         {
//             Flip();
//         }
//     }

//     public void Flip()
//     {
//         //stores scale and flips the player along the x axis, 
//         Vector3 scale = transform.localScale;
//         scale.x *= -1;
//         transform.localScale = scale;

//         if (ps.facing == XDirection.LEFT)
//         {
//             ps.facing = XDirection.RIGHT;
//         }
//         else
//         {
//             ps.facing = XDirection.LEFT;
//         }
//     }

//     public void Jump()
//     {
//         _pressedJumpTimer = 0;
//         _coyoteTimer = 0;

//         float force = config.jumpForce;

//         //We increase the force applied if we are falling
//         //This means we'll always feel like we jump the same amount 
//         //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
//         if (rigidBody.velocity.y < 0)
//             force -= rigidBody.velocity.y;

//         //Apply the jump force
//         rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

//         //Set the jump state
//         ps.jumpState = JumpState.JUMPING;
//     }

//     public void CutJump()
//     {
//         rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
//     }

//     public void Move(float lerpAmount)
//     {
//         //Calculate the direction we want to move in and our desired velocity
//         float targetSpeed = _input.x * config.maxSpeed;
//         //We can reduce are control using Lerp() this smooths changes to are direction and speed
//         targetSpeed = Mathf.Lerp(rigidBody.velocity.x, targetSpeed, lerpAmount);

//         float accelRate;

//         //Gets an acceleration value based on if we are accelerating (includes turning) 
//         //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
//         // DO NOT TOUCH, I CALCULATED IT OUT AND IT WORKS NO MATTER HOW WEIRD IT LOOKS
//         if (ps.jumpState == JumpState.GROUNDED)
//             accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? config.xAcceleration : config.xDeceleration;
//         else
//             accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? config.xAcceleration * config.accelInAirMult : config.xDeceleration * config.deccelInAirMult;

//         #region Add Bonus Jump Apex Acceleration
//         //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
//         if (ps.jumpState == JumpState.APEX)
//         {
//             accelRate *= config.jumpApexXAccelerationMult;
//             targetSpeed *= config.jumpApexXMaxSpeedMult;
//         }
//         #endregion

//         //Calculate difference between current velocity and desired velocity
//         float speedDif = targetSpeed - rigidBody.velocity.x;
//         //Calculate force along x-axis to apply to the player

//         float movement = speedDif * accelRate;

//         //Convert this to a vector and apply to rigidbody
//         rigidBody.AddForce(movement * Vector2.right, ForceMode2D.Force);

//         /*
// 		 * For those interested here is what AddForce() will do
// 		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
// 		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
// 		*/
//     }

//     #region EDITOR METHODS
//     private void OnDrawGizmosSelected()
//     {
//         //Draws a wire sphere at the ground check position
//         //
//         // Debug.Log(collider.size);
//         Vector2 capsuleDims = groundCollider.size * (Vector2)transform.localScale;
//         float radius = 0.5f * Mathf.Min(capsuleDims.x, capsuleDims.y) - 0.1f;
//         Vector2 pos = (Vector2)transform.position + groundCollider.offset - Vector2.up * (Mathf.Max(capsuleDims.x, capsuleDims.y) * 0.5f - radius);
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireSphere(pos, radius);
//         // Gizmos.color = Color.blue;
//         // Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
//         // Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
//     }
//     #endregion
// }
