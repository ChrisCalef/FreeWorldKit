
using System;
using System.Collections;
using UnityEngine;


// FPS walk script
public class WalkController : MonoBehaviour
{
	private readonly float speed = 8.0f;
	private readonly float jumpSpeed = 8.0f;
	private readonly float gravity = 20.0f;

	private Vector3 moveDirection = Vector3.zero;
	private bool grounded = false;
	
	private AnimationSynchronizer animator;
	private CharacterController controller;

	void Awake() {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<AnimationSynchronizer>();
	}
	
	void FixedUpdate() {
		if (grounded) {
			// We are grounded, so recalculate movedirection directly from axes
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			if (moveDirection == Vector3.zero) {
				animator.PlayIdle();
			}
			else {
				if (moveDirection.z > 0) {
					animator.PlayRunForward();
				}
				else if (moveDirection.z < 0) {
					animator.PlayRunForward();
				}
			}	 	
				
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			
			if (Input.GetButton ("Jump")) {
				moveDirection.y = jumpSpeed;
			}
			
		}
				
		// Apply gravity
		moveDirection.y -= gravity * Time.deltaTime;
			
		// Move the controller
		CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
		grounded = (flags & CollisionFlags.CollidedBelow) != 0;
				
	}
	
}
