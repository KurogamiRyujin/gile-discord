using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement behaviour for the Ringleader.
/// </summary>
public class RingleaderMovement : MonoBehaviour {

    /// <summary>
    /// Ringleader's movement speed.
    /// </summary>
	[SerializeField] private float movementSpeed = 2.5f;

    /// <summary>
    /// Reference to the teleporter which allows the Ringleader to teleport as part of its movement behaviour.
    /// </summary>
	private Teleport teleporter;
    /// <summary>
    /// Target direction where the Ringleader will move towards.
    /// </summary>
	private Vector2 targetPos;
    /// <summary>
    /// Reference to the Ringleader's rectangle transform.
    /// </summary>
	private RectTransform rectTransform;
    /// <summary>
    /// Flag if the Ringleader is allowed to move.
    /// </summary>
	public bool canMove = true;
    /// <summary>
    /// Flag if the Ringleader is facing to the right.
    /// </summary>
	bool isFacingRight = false;
    /// <summary>
    /// Reference to the Ringleader's animatable.
    /// </summary>
	private RingleaderAnimatable ringleaderAnimatable;
    /// <summary>
    /// Reference to the Ringleader's attack hazard.
    /// </summary>
	[SerializeField] private RingleaderNormalAttackHazard hazard;

    /// <summary>
    /// Flag if the Ringleader is talking.
    /// </summary>
    private bool talkPause;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
    void Awake() {
		rectTransform = GetComponent<RectTransform> ();
		teleporter = GetComponent<Teleport> ();
		targetPos = transform.position;
		ringleaderAnimatable = GetComponent<RingleaderAnimatable> ();
		if (hazard == null)
			hazard = GetComponentInChildren<RingleaderNormalAttackHazard> ();
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
    void Update () {
        if (!talkPause) {
            Flip();
            if (canMove) {
                Vector2 currentPos = this.transform.position;

                if (currentPos != targetPos) {
                    this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, movementSpeed * Time.deltaTime);
                }
            }
        }
	}

    /// <summary>
    /// Prompts the Ringleader will talk through a dialogue, pausing all action.
    /// </summary>
    public void TalkPause() {
        this.talkPause = true;
    }

    /// <summary>
    /// Resumes all action from the dialogue.
    /// </summary>
    public void Resume() {
        this.talkPause = false;
    }

    /// <summary>
    /// Move the Ringleader avatar towards a direction.
    /// </summary>
    /// <param name="pos">Target Direction</param>
    public void MoveTowards(Vector2 pos) {
		this.targetPos = pos;
	}

    /// <summary>
    /// Flag if the Ringleader is teleporting.
    /// </summary>
    /// <returns>If the Ringleader is teleporting. Otherwise, false.</returns>
	public bool IsTeleporting() {
		return this.teleporter.IsTeleporting ();
	}

    /// <summary>
    /// Teleport the Ringleader avatar to a target position.
    /// </summary>
    /// <param name="pos">Target Position</param>
	public void TeleportTo(Vector3 pos) {
		teleporter.TeleportTo (pos);
	}

    /// <summary>
    /// Teleports the Ringleader near the player avatar.
    /// </summary>
	public void TeleportToPlayer() {
		teleporter.TeleportToPlayer ();
	}

    /// <summary>
    /// Checker if the player avatar is near.
    /// </summary>
    /// <returns></returns>
	public bool IsNearTarget() {
		Vector2 currentPos = new Vector2 (transform.position.x, transform.position.y);

		return (Vector2.Distance (currentPos, targetPos) < rectTransform.rect.width);
	}

    /// <summary>
    /// Flip the Ringleader to face another direction.
    /// </summary>
	void Flip() {
		if (targetPos.x >= transform.position.x) {
			if (!isFacingRight) {
				for (int i = 0; i < transform.childCount-1; i++) {
					if (transform.GetChild (i).gameObject.GetComponent<BulletHell> () == null) {
						Vector3 temp = transform.GetChild(i).localScale;
						temp.x *= -1;
						transform.GetChild(i).localScale = temp;
					}
				}
				Vector2 face = new Vector2 (hazard.transform.localScale.x * -1, hazard.transform.localScale.y);

				hazard.transform.localScale = face;
				//				Vector3 temp = transform.GetChild(1).localScale;
				//				temp.x *= -1;
				//				transform.GetChild(1).localScale = temp;
				//				temp = transform.GetChild (2).localScale;
				//				temp.x *= -1;
				//				transform.GetChild(2).localScale = temp;
				isFacingRight = !isFacingRight;
			}
		} else if (targetPos.x < transform.position.x) {
			if (isFacingRight) {
				for (int i = 0; i < transform.childCount-1; i++) {
					if (transform.GetChild (i).gameObject.GetComponent<BulletHell> () == null) {
						Vector3 temp = transform.GetChild(i).localScale;
						temp.x *= -1;
						transform.GetChild(i).localScale = temp;
					}
				}
				Vector2 face = new Vector2 (hazard.transform.localScale.x * -1, hazard.transform.localScale.y);

				hazard.transform.localScale = face;
				//				Vector3 temp = transform.GetChild(1).localScale;
				//				temp.x *= -1;
				//				transform.GetChild(1).localScale = temp;
				//				temp = transform.GetChild (2).localScale;
				//				temp.x *= -1;
				//				transform.GetChild(2).localScale = temp;
				isFacingRight = !isFacingRight;
			}
		}
	}
}
