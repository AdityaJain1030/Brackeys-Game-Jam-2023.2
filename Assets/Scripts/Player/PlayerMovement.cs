/*
	Created by @DawnosaurDev at youtube.com/c/DawnosaurStudios
	Thanks so much for checking this out and I hope you find it helpful! 
	If you have any further queries, questions or feedback feel free to reach out on my twitter or leave a comment on youtube :D

	Feel free to use this in your own games, and I'd love to see anything you make!
 */

using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters. If you don't want to use it
	//just paste in all the parameters, though you will need to manuly change all references in this script

	//HOW TO: to add the scriptable object, right-click in the project window -> create -> Player Data
	//Next, drag it into the slot in playerMovement on your player

	public PlayerData Data;

	#region Variables
	//Components
    public Rigidbody2D rigidBody { get; private set; }

	//Variables control the various actions the player can perform at any time.
	//These are fields which can are public allowing for other sctipts to read them
	//but can only be privately written to.
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsWallJumping { get; private set; }
	public bool IsSliding { get; private set; }
	public bool IsRunning { get; private set; }
	public bool IsDashing{ get; private set; }

	//Timers (also all fields, could be private and a method returning a bool could be used)
	public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }

	//Jump
	private bool _isJumpCut;
	private bool _isJumpFalling;

	//Wall Jump
	private float _wallJumpStartTime;
	private int _lastWallJumpDir;

	private Vector2 _moveInput;
	public float LastPressedJumpTime { get; private set; }
	public float LastPressedDashTime { get; private set; }

	private bool dashed = false;
	//Set all of these up in the inspector
	[Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.7f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.25f, 1.7f);
	[Space(5)]
	[SerializeField] private Transform _topLedgeCheck;
	[SerializeField] private Transform _botLedgeCheck;
	[SerializeField] private Vector2 _ledgeCheckSize = new Vector2(0.25f, 1.25f);
	[SerializeField] private GameObject boundsChecksGameObject;

    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

    private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		LastPressedDashTime -= Time.deltaTime;
		#endregion

		#region INPUT HANDLER
		_moveInput.x = Input.GetAxisRaw("Horizontal");
		_moveInput.y = Input.GetAxisRaw("Vertical");

		if (_moveInput.x != 0)
			CheckDirectionToFace(_moveInput.x > 0);

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
			OnJumpInput();
        }

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
		{
			OnJumpUpInput();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && LastPressedDashTime < 0) {
			// rigidBody.AddForce(5000.0f * Vector2.right * Mathf.Sign(_moveInput.x), ForceMode2D.Impulse);
			Vector2 currVel = rigidBody.velocity;
			rigidBody.velocity = new Vector2(5 * Mathf.Sign(_moveInput.x), 0);
			LastPressedDashTime = 1.0f;
		}
		#endregion

		#region COLLISION CHECKS
		if (!IsJumping)
		{
			//Ground Check
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
			{
				LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }		

			//Right Wall Check
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
					|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
				LastOnWallRightTime = Data.coyoteTime;

			//Right Wall Check
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
				LastOnWallLeftTime = Data.coyoteTime;

			//Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
			LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
		}
		#endregion

		#region JUMP CHECKS
		if (IsJumping && rigidBody.velocity.y < 0)
		{
			IsJumping = false;

			if(!IsWallJumping)
				_isJumpFalling = true;
		}

		if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
		{
			IsWallJumping = false;
		}

		if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
			_isJumpCut = false;

			if(!IsJumping)
				_isJumpFalling = false;
		}

		if (IsDashing && LastPressedDashTime > 5.0) {
			IsDashing = false;
		}

		//Jump
		if (CanJump() && LastPressedJumpTime > 0)
		{
			IsJumping = true;
			IsWallJumping = false;
			_isJumpCut = false;
			_isJumpFalling = false;
			Jump();
		}

		//WALL JUMP
		else if (CanWallJump() && LastPressedJumpTime > 0)
		{
			IsWallJumping = true;
			IsJumping = false;
			_isJumpCut = false;
			_isJumpFalling = false;
			_wallJumpStartTime = Time.time;
			_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
			
			WallJump(_lastWallJumpDir);
		}
		
		// DASH

		if (CanDash() && LastPressedJumpTime > 0) {
			IsDashing = true;
			Dash();
		}
		#endregion

		#region SLIDE CHECKS
		if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
			IsSliding = true;
		else
			IsSliding = false;
		#endregion

		#region GRAVITY
		//Higher gravity if we've released the jump input or are falling
		if (IsSliding)
		{
			SetGravityScale(0);
		}
		else if (rigidBody.velocity.y < 0 && _moveInput.y < 0)
		{
			//Much higher gravity if holding down
			SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -Data.maxFastFallSpeed));
		}
		else if (_isJumpCut)
		{
			//Higher gravity if jump button released
			SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -Data.maxFallSpeed));
		}
		else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rigidBody.velocity.y) < Data.jumpHangTimeThreshold)
		{
			SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
		}
		else if (rigidBody.velocity.y < 0)
		{
			//Higher gravity if falling
			SetGravityScale(Data.gravityScale * Data.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -Data.maxFallSpeed));
		}
		else
		{
			//Default gravity if standing on a platform or moving upwards
			SetGravityScale(Data.gravityScale);
		}
		#endregion
    
		#region LEDGE CHECK
		//Ledge Check
		if (!Physics2D.OverlapBox(_topLedgeCheck.position, _ledgeCheckSize, 0, _groundLayer) && Physics2D.OverlapBox(_botLedgeCheck.position, _ledgeCheckSize, 0, _groundLayer))
		{
			//If we are moving horizontal and the ledge check above us is not colliding with the ground layer but we are
			//then we must be on a ledge
			// if (Mathf.Abs(rigidBody.velocity.x) > 0)
			{
				//We set the player's y velocity to 0 so they don't keep moving upwards
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
				//We set the player's position to the ledge check's position + 
				transform.position = (Vector2)_topLedgeCheck.position + transform.localScale.y/2 * Vector2.up;
			}
		}
		#endregion
	}

    private void FixedUpdate()
	{
		//Handle Run
		if (IsWallJumping)
			Run(Data.wallJumpRunLerp);
		else {
			Run(1);
		}

		if (LastOnGroundTime > 0 && Mathf.Abs(_moveInput.x) > 0) IsRunning = true;
		else IsRunning = false;

		//Handle Slide
		if (IsSliding)
			Slide();
    }

    #region INPUT CALLBACKS
	//Methods which whandle input detected in Update()
    public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut() || CanWallJumpCut())
			_isJumpCut = true;
	}

	public void OnDashInput() {
		LastPressedDashTime = 5;
	}
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
	{
		rigidBody.gravityScale = scale;
	}
    #endregion

	//MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		targetSpeed = Mathf.Lerp(rigidBody.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rigidBody.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(Data.doConserveMomentum && Mathf.Abs(rigidBody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rigidBody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0; 
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - rigidBody.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		rigidBody.AddForce(movement * Vector2.right, ForceMode2D.Force);

		/*
		 * For those interested here is what AddForce() will do
		 * rigidBody.velocity = new Vector2(rigidBody.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / rigidBody.mass, rigidBody.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
	}

	private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		Vector3 scale = boundsChecksGameObject.transform.localScale; 
		scale.x *= -1;
		boundsChecksGameObject.transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
    #endregion

    #region JUMP METHODS
    private void Jump()
	{
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = Data.jumpForce;
		if (rigidBody.velocity.y < 0)
			force -= rigidBody.velocity.y;

		rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}

	private void Dash() 
	{
		LastPressedDashTime = 0;

		float force = 5.0f;
		rigidBody.AddForce(Vector2.right * force, ForceMode2D.Impulse);

	}

	private void WallJump(int dir)
	{
		//Ensures we can't call Wall Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		#region Perform Wall Jump
		Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
		force.x *= dir; //apply force in opposite direction of wall

		if (Mathf.Sign(rigidBody.velocity.x) != Mathf.Sign(force.x))
			force.x -= rigidBody.velocity.x;

		if (rigidBody.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= rigidBody.velocity.y;

		//Unlike in the run we want to use the Impulse mode.
		//The default mode will apply are force instantly ignoring masss
		rigidBody.AddForce(force, ForceMode2D.Impulse);
		#endregion
	}
	#endregion

	#region OTHER MOVEMENT METHODS
	private void Slide()
	{
		//Works the same as the Run but only in the y-axis
		//THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
		float speedDif = Data.slideSpeed - rigidBody.velocity.y;	
		float movement = speedDif * Data.slideAccel;
		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		rigidBody.AddForce(movement * Vector2.up);
	}
    #endregion


    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

    private bool CanJump()
    {
		return LastOnGroundTime > 0 && !IsJumping;
    }
	
	private bool CanDash() 
	{
		return !IsDashing;
	}

	private bool CanWallJump()
    {
		return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
			 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
	}

	private bool CanJumpCut()
    {
		return IsJumping && rigidBody.velocity.y > 0;
    }

	private bool CanWallJumpCut()
	{
		return IsWallJumping && rigidBody.velocity.y > 0;
	}

	public bool CanSlide()
    {
		if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0)
			return true;
		else
			return false;
	}
    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(_topLedgeCheck.position, _ledgeCheckSize);
		Gizmos.DrawWireCube(_botLedgeCheck.position, _ledgeCheckSize);
	}
    #endregion
}

// created by Dawnosaur :D