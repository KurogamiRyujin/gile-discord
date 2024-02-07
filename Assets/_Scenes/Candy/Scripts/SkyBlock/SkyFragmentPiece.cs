using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This extends the SkyFragment.
 * It is tangible to the player but intangible to the needle.
 * It is breakable by the hammer.
 * 
 * Its Break function allows it to be destroyed and have its value
 * returned to the SkyFragmentBlock (does this through the SkyBlock parent)
 *
**/
public class SkyFragmentPiece : SkyFragment {
	[SerializeField] private HollowBlock hollowBlock;
	[SerializeField] protected DetachedManager detachedManagerParent;
//	[SerializeField] protected HintBubbleSkyPiece fractionBubbleLabel;

	[SerializeField] protected Material overlapMaterial;
	[SerializeField] protected Material defaultMaterial;
	[SerializeField] protected Material attachedMaterial;

	[SerializeField] protected bool isSimulated;
	[SerializeField] protected float mass;
	[SerializeField] protected float gravity;
	[SerializeField] protected float linearDrag;
	[SerializeField] protected float angularDrag;
	[SerializeField] protected RigidbodyType2D bodyType;

//	[SerializeField] protected bool isDropping;

	[SerializeField] protected bool isAssigned;
	[SerializeField] protected bool isCarried;
	[SerializeField] protected Color observedPieceColor = new Color(0.74f, 0.62f, 0.25f, 1f);
	[SerializeField] protected Color observedOutlineColor = new Color (0.46f, 0.35f, 0.05f, 1f);
	[SerializeField] protected Transform previousParent;
	[SerializeField] protected PlatformLock platformLock;
	[SerializeField] protected bool disabledPlatformLock;

	[SerializeField] protected bool neverBreak;
	private float speed = 500f;
	private float torque = 300f;

	public int sceneObjectId;

	void Awake () {
		base.Awake ();
		observedPieceColor = new Color(0.74f, 0.62f, 0.25f, 1f);
//		observedOutlineColor = new Color (0.46f, 0.35f, 0.05f, 1f);
		observedOutlineColor = new Color (1f, 1f, 1f, 1f);
//		this.isDropping = false;
//		this.GetFractionBubbleLabel ().SetSkyPiece (this);
	}
	public SpriteRenderer GetSpriteRenderer() {
		return this.spriteRenderer;
	}
	public Material GetDefaultMaterial() {
		if (this.defaultMaterial == null) {
			this.defaultMaterial = spriteRenderer.material;
		}
		return this.defaultMaterial;
	}
	public void WearAttachedSkin() {
		if (this.attachedMaterial != null) {
			this.spriteRenderer.material = this.attachedMaterial;
		}
	}

	public void WearDefaultSkin() {
		if (this.GetDefaultMaterial () != null) {
			this.spriteRenderer.material = this.defaultMaterial;
		}
	}
	// To be called by Sky Block only. (the Drop function called through prefill)
	public void DisablePlatformLocking() {
		this.disabledPlatformLock = true;
	}

	public Material GeOverlapMaterial() {
		if (this.overlapMaterial == null) {
			this.overlapMaterial = spriteRenderer.material;
		}
		return this.overlapMaterial;
	}

	// Disable platform parenting on 
	public void LockPiece(PlatformLock platform) {
		if (!this.disabledPlatformLock) {
//			Debug.Log ("<color=red>Piece LOCK</color>");
			this.platformLock = platform;
			this.previousParent = gameObject.transform.parent;
			gameObject.transform.SetParent (platform.gameObject.transform);
		}
	}

	public void UnlockPiece(PlatformLock platform) {
		if (!this.disabledPlatformLock) {
			if (this.platformLock == platform) {
				this.platformLock = null;
				gameObject.transform.SetParent (this.previousParent);
			}
		}
	}

	public void SetPreviousParent(Transform parent) {
		this.previousParent = parent;
	}
	// Fix label rotation
//	void Update() {
//		FixLabelRotation ();
//	}

//	void FixLabelRotation() {
//		if (GetFractionBubbleLabel () != null) {
//			Vector3 prevRotation = GetFractionBubbleLabel ().transform.eulerAngles;
//			Vector3 newRotation =  new Vector3 (
//				GetFractionBubbleLabel ().transform.localRotation.x,
//				GetFractionBubbleLabel ().transform.localRotation.y,
//				-gameObject.transform.localRotation.z);
//
//			if (prevRotation != newRotation) {
//				GetFractionBubbleLabel ().transform.eulerAngles = newRotation;
//			}
//		}
	
//	}

//	public HintBubbleSkyPiece GetFractionBubbleLabel() {
//		if (this.fractionBubbleLabel == null) {
//			this.fractionBubbleLabel.GetComponentInChildren<HintBubbleSkyPiece> ();
//		}
//		return this.fractionBubbleLabel;
//	}
	public void DisableCollider() {
		this.GetCollider ().enabled = false;
	}
	public void EnableCollider() {
		this.GetCollider ().enabled = true;
	}
	public override void Initialize(SkyBlock parent, float numValue, float denValue) {
		base.Initialize (parent, numValue, denValue);
//		this.GetFractionBubbleLabel ().UpdateLabel ((int)this.GetNumerator(), (int)this.GetDenominator());
		this.detachedManagerParent = this.skyBlockParent.GetDetachedManager ();
	}

	public void Carry(GameObject parent, Vector3 position) {
		if (!IsAssigned ()) {
			HideLabel ();
			this.Unobserve ();
			this.SetCarried (true);

			this.GetRigidBody2D ().bodyType = RigidbodyType2D.Kinematic;
			this.GetRigidBody2D ().velocity = Vector2.zero;
			this.GetRigidBody2D ().angularVelocity = 0f;
			gameObject.transform.SetParent (parent.transform);
			this.previousParent = parent.transform;
			gameObject.transform.localPosition = position;
			gameObject.transform.eulerAngles = new Vector3 (0f, 0f, -45f);
			this.boxCollider.isTrigger = true;
		}
	}

	public void Drop () {
		this.WearDefaultSkin ();
		this.SetCarried (false);
		this.GetRigidBody2D ().bodyType = RigidbodyType2D.Dynamic;
		gameObject.transform.SetParent (detachedManagerParent.gameObject.transform);
		this.boxCollider.isTrigger = false;
		StartCoroutine (DropRoutine());
	}

	public void FillDrop (HollowBlock hollowBlock) {
		this.WearDefaultSkin ();
		this.SetCarried (false);
		this.GetRigidBody2D ().bodyType = RigidbodyType2D.Dynamic;
		gameObject.transform.SetParent (detachedManagerParent.gameObject.transform);
		this.boxCollider.isTrigger = false;
		this.Unobserve ();
		hollowBlock.Absorb (this);
	}

	// Change color when overlapping
	public void Observe() {
		if (!this.IsCarried () && !IsAssigned()) {
			Debug.Log ("OBSERVE");
			this.SoftChangeColor (this.observedPieceColor, this.observedOutlineColor);
//			EventBroadcaster.Instance.PostEvent (EventNames.SHOW_PLAYER_CARRY);
			this.spriteRenderer.material = this.overlapMaterial;
		}
	}

	public void Unobserve() {
		Debug.Log ("UNOBSERVE");
		this.SoftChangeColor (this.pieceColor, this.pieceOutlineColor);
		if (this.IsAssigned ()) {
			this.WearAttachedSkin ();
		} else {
			this.WearDefaultSkin ();
		}
//		this.spriteRenderer.material = this.defaultMaterial;
	}

	public Collider2D GetCollider() {
		return this.boxCollider;
	}

	// Logs the values and destroys the rigidbody
	public void DisableRigidBody() {
		this.SetAssigned (true);
		this.GetRigidBody2D ().isKinematic = true;
		this.GetRigidBody2D ().angularVelocity = 0f;
		this.GetRigidBody2D ().velocity = Vector3.zero;
		gameObject.transform.eulerAngles = Vector3.zero;
		gameObject.transform.localPosition = Vector3.zero;
//		this.bodyType = this.GetRigidBody2D ().bodyType;
//		this.mass = this.GetRigidBody2D ().mass;
//		this.linearDrag = this.GetRigidBody2D ().drag;
//		this.angularDrag = this.GetRigidBody2D ().angularDrag;
//		this.gravity = this.GetRigidBody2D ().gravityScale;
//		Destroy (this.GetRigidBody2D ());
	}

	public void SetHollowBlock(HollowBlock block) {
		this.hollowBlock = block;
	}

	// Creates a new rigidbody and initializes it with the previously logged values
	public void EnableRigidBody() {
		this.SetAssigned (false);
		this.GetRigidBody2D ().isKinematic = false;
//		Rigidbody2D newRigidBody = gameObject.AddComponent<Rigidbody2D> ();
//		newRigidBody.bodyType = this.bodyType;
//		newRigidBody.mass = this.mass;
//		newRigidBody.drag = this.linearDrag;
//		newRigidBody.angularDrag = this.angularDrag;
//		newRigidBody.gravityScale = this.gravity;
	}
	
	public bool IsAssigned() {
		return this.isAssigned;
	}
	public bool IsCarried() {
		return this.isCarried;
	}
	public void SetCarried(bool value) {
		this.isCarried = value;
	}

	IEnumerator DropRoutine() {
		Debug.Log ("<color=magenta>DROP ROUTINE ENTER</color>");
//		this.isDropping = true;
		Collider2D[] colliders;
		Vector3 prevPosition = Vector3.zero;
		bool hasAbsorbed = false;
		// While falling
		while (!hasAbsorbed //&& gameObject.transform.position != prevPosition
			&& !this.boxCollider.IsTouchingLayers(LayerMask.NameToLayer("Ground"))) {
//			this.GetRigidBody2D().velocity.y > 0f) {
//			ContactFilter2D contactFilter = new ContactFilter2D ();
//			Physics2D.OverlapBox (this.boxCollider.transform.position, this.boxCollider.size, this.boxCollider.transform.eulerAngles.z, contactFilter, colliders);
			colliders = Physics2D.OverlapBoxAll (this.boxCollider.transform.position, this.boxCollider.size, this.boxCollider.transform.eulerAngles.z);

			foreach(Collider2D collider in colliders) {
				if (!hasAbsorbed && !isCarried
					&& collider.gameObject.GetComponent<HollowBlock> () != null) {
					// Absorb
					hasAbsorbed = true;
					Debug.Log ("<color=blue>HOLLOW BLOCK COLLISION</color>");
					HollowBlock hollowBlock = collider.gameObject.GetComponent<HollowBlock> ();
					hollowBlock.Absorb (this);

				}
				yield return null;
			}
			yield return null;
			prevPosition = gameObject.transform.position;
		}
//		this.isDropping = false;
		Debug.Log ("<color=magenta>DROP ROUTINE EXIT</color>");
	}

	public void SetAssigned(bool value) {
		this.isAssigned = value;
	}

	public void ShowLabel() {
//		this.GetHollowBlock ().ActivateHint ();
//		if (!IsAssigned() && !IsCarried()) {
//			this.GetFractionBubbleLabel ().ShowHint ();
//		}
	}

	public void HideLabel() {
//		this.GetHollowBlock ().DectivateHint ();
//		this.GetFractionBubbleLabel ().HideHint ();
	}

	// If unassigned piece fell (i.e. held by player, fell off platform)
	public void DeathBreak() {
		if (this.GetHollowBlock () != null) {
			hollowBlock.UpdateStabilityLabel ();
		}
		this.transform.position = this.skyBlockParent.gameObject.transform.position;
	}

	// If assigned piece fell (i.e. already in box but player died)
	public void Return() {
		this.EnableRigidBody ();
		this.SetHollowBlock (null);
		gameObject.transform.SetParent (this.GetDetachedManagerParent().gameObject.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.eulerAngles = Vector3.zero;
	}

	// Release is like return, but to the previously attached Hollow Block's position
	// To be called by Hollow Blocks only or self
	public void Release() {
		this.WearDefaultSkin ();
		this.isAssigned = false;
		this.SetCarried (false);
		this.GetRigidBody2D ().bodyType = RigidbodyType2D.Dynamic;
		gameObject.transform.SetParent (detachedManagerParent.gameObject.transform);
		this.boxCollider.isTrigger = false;

//		this.EnableRigidBody ();
		this.GetRigidBody2D ().velocity = Vector2.zero;
		this.GetRigidBody2D ().angularVelocity = 0f;
		gameObject.transform.position = this.GetHollowBlock ().gameObject.transform.position;
		gameObject.transform.eulerAngles = Vector3.zero;

		// To add force

		this.GetRigidBody2D ().AddRelativeForce (Random.onUnitSphere * speed);
		this.GetRigidBody2D ().AddTorque (torque);

//		gameObject.transform.SetParent (this.GetDetachedManagerParent().gameObject.transform);
		if (hollowBlock != null) {
			hollowBlock.UpdateStabilityLabel ();
		}
		this.SetHollowBlock (null);
	}



	public void Break() {
		this.WearDefaultSkin ();
		if (this.GetHollowBlock () != null) {
			hollowBlock.UpdateStabilityLabel ();
		}
		if (!neverBreak) {
			this.skyBlockParent.Absorb (this);
		}
	}
	public void ParentBreak() {
		if (!GetHollowBlock ().IsBroken ()) { // To prevent children calls from cascading
			this.GetHollowBlock ().Break ();
		}
	}

	public void SetPiecesNeverBreak(bool isNeverBreak) {
		this.neverBreak = isNeverBreak;
	}

	void OnCollisionEnter2D(Collision2D other) {
		// NOTE: Use other.collider.gameObject.layer to get gameObject
		if (other.collider.enabled == true &&
			other.collider.gameObject.GetComponent<BreakerCollider>() != null &&
			!other.collider.gameObject.GetComponent<BreakerCollider>().HasHitHollowBlock()) { // And has not just hit hollow block


			// If assigned, detach. If Unassigned, break
			if (this.IsAssigned ()) {
				this.Release ();
			} else {
				Debug.Log ("SKY BREAKER");
				if (!neverBreak) {
					this.boxCollider.isTrigger = true;
					this.boxCollider.enabled = false;
				}
				this.Break ();
			}
//			if (this.GetHollowBlock () != null && !this.GetHollowBlock ().IsBroken ()) {
//				this.ParentBreak ();
//			} else {
//				this.Break ();
//			}
		}
	}

	public HollowBlock GetHollowBlock() {
		if (this.hollowBlock == null) {
			this.hollowBlock = GetComponentInParent<HollowBlock> ();
		}
		return this.hollowBlock;
	}
	public DetachedManager GetDetachedManagerParent() {
		return this.detachedManagerParent;
	}
		
	public int GetSceneObjectId() {
		return this.sceneObjectId;
	}

	public void SetSceneObjectId(int id) {
		this.sceneObjectId = id;
	}
}
