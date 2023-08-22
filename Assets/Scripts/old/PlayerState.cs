using UnityEngine;
using System;

public enum XDirection { LEFT, RIGHT }
public enum JumpState { GROUNDED, JUMPING, APEX, FALLING, WALLJUMPING }


public class PlayerState : MonoBehaviour {
	[NonSerialized] public XDirection facing = XDirection.RIGHT;
	[NonSerialized] public JumpState jumpState = JumpState.GROUNDED;
	[NonSerialized] public bool RecievingPlayerInput = true;
}