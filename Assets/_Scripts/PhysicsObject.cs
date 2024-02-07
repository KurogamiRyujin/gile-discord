using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

	// The minimum value to compare the angle of the raycast
	public float minGroundNormalY = 0.65f;
	public float gravityModifier = 1f;

	protected bool grounded;
	protected Vector2 groundNormal;

	protected Vector2 targetVelocity;
	protected Vector2 velocity;
	protected Rigidbody2D rb2d;

	// For rigidbody2d.cast
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);

	protected const float minMoveDistance = 0.001f;
	// Padding to ensure that the object doesn't get stuck in another collider
	protected const float shellRadius = 0.01f;

	void OnEnable() {
		// Get and store the component's rigidbody
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Start() {
		// Don't check collision against triggers
		contactFilter.useTriggers = false;

		// Decide which layers to collide with by using layermasks
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	void Update() {
		// Zero out targetVelocity every frame to get rid of previous values
		targetVelocity = Vector2.zero;
		ComputeVelocity ();
	}

	protected virtual void ComputeVelocity() {
	
	}

	// Note: horizontal and vertical movement checking are separated
	protected virtual void FixedUpdate() {
		// Get object velocity according to gravity
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

		// Until a collision is registered in that frame, grounded is always false
		grounded = false;

		// Get the object's change in position
		Vector2 deltaPosition = velocity * Time.deltaTime;

		// The vector along the ground, perpendicular to the groundNormal
		Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

		Vector2 move = moveAlongGround * deltaPosition.x;

		// Check horizontal movement (x-axis)
		Movement (move, false);

		// Get vertical movement according to gravity
		move = Vector2.up * deltaPosition.y;

		// Check vertical movement (y-axis)
		Movement (move, true);
	}

	void Movement (Vector2 move, bool yMovement) {
		float distance = move.magnitude;

		// Check for collisions only if the distance that the object is
		// going to move is greater than a minimum value to prevent continuous
		// collision check when the object is idle
		if (distance > minMoveDistance) {
			// Check if the rigidbody is going to overlap with anything on the next frame
			// using rigidbody2d.cast
			int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

			// Copy only the indices in hitBuffer that actually hit something (list of contacts)
			hitBufferList.Clear();
			for( int i = 0; i < count; i++) {
				hitBufferList.Add (hitBuffer[i]);
			}


			// Loop over hitBufferList and compare the normal with a ground value to determine the angle
			for (int i = 0; i < hitBufferList.Count; i++) {
				// Check the normal of each of the raycast2d in the list
				Vector2 currentNormal = hitBufferList [i].normal;

				// Determine if the player is grounded
				if (currentNormal.y > minGroundNormalY) {
					grounded = true;

					// If moving on the y axis
					if (yMovement) {
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				// Get the difference between the velocity and the currentNormal to determine if we need to
				// subtract from our velocity to prevent the player from entering into another collider
				float projection = Vector2.Dot (velocity, currentNormal);
				if (projection < 0) {
					// Cancel out the part of the velocity that would be stopped by the collision
					velocity = velocity - projection * currentNormal;
				}

				// If distance is less than shellRadius (padding) use shellRadius to prevent the object from getting
				// stuck in another collider
				float modifiedDistance = hitBufferList [i].distance - shellRadius;
				distance = modifiedDistance; // < distance ? modifiedDistance : distance;
			}
		}
		if(rb2d != null)
			rb2d.position = rb2d.position + move.normalized * distance;
	}

}