using UnityEngine;

[CreateAssetMenu(menuName = "Player Config")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class oldPlayerData : ScriptableObject
{
	[Header("Movement Config")]
	// Player attributes
	public float maxSpeed;
	[Range(0.01f, 1)] public float xAccelerationScale; //How fast to accelerate to max speed
	[Range(0.01f, 1)] public float xDecelerationScale; // How fast to decelerate to 0 speed
	[HideInInspector] public float xAcceleration; // The actual acceleration/force (mass = 1) applied to the player
	[HideInInspector] public float xDeceleration; // The actual deceleration/force (mass = 1) applied to the player

	[Space(10)]
	public float jumpHeight; //Height of the player's jump
	public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
	[HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.
	
	[Space(5)]
	// READONLY
	public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
	public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
										  //Also the value the player's rigidbody2D.gravityScale is set to.
	[Space(10)]
	public float coyoteTime;
	public float jumpInputBufferTime;
	[Range(0f, 1)] public float accelInAirMult; // Reduce acceleration when in the air
	[Range(0f, 1)] public float deccelInAirMult; // Reduce deceleration when in the air
	public float jumpApexXAccelerationMult; // Increase x acceleration when at the apex of a jump, helps with maneuverability at the top of a jump
	public float jumpApexXMaxSpeedMult; // Increase max speed when at the apex of a jump, helps with maneuverability at the top of a jump
	public float jumpApexThreshold; // The absolute y velocity at which the player is considered to be at the apex of their jump
	public float terminalVelocity;

	[Space(20)]

	[Header("Player Attributes")]
	public bool wallJumpUnlocked;
	public bool dashUnlocked;
	public bool grappleUnlocked;

	// [Space(10)]
	// public Vector2 wallJumpForce; //Force of Jump
	// [Range(0f, 1f)] public float wallJumpMomentumMult; //Reduces the effect of player's movement while wall jumping.
	// [Range(0f, 1.5f)] public float wallJumpMomentumMultTime; //Time after wall jumping the player's movement is slowed for.


	//Unity Callback, called when the inspector updates
    private void OnValidate()
    {
		//Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
		gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
		
		//Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
		gravityScale = gravityStrength / Physics2D.gravity.y;

		//Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / maxSpeed (this is why we got an acceleration between 0, 1)
		xAcceleration = 50 * xAccelerationScale;
		xDeceleration = 50 * xDecelerationScale;

		//Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
		jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
	}
}