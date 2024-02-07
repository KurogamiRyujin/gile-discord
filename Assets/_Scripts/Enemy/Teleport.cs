using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleporter functionality for the Ringleader's movement behaviour.
/// </summary>
public class Teleport : MonoBehaviour {

    /// <summary>
    /// Script's name.
    /// </summary>
	public const string scriptName = "Teleport";

	[SerializeField] List<Transform> positions;
    /// <summary>
    /// Reference to the player avatar's position.
    /// </summary>
	Transform yuniPos;
    /// <summary>
    /// Direction to the latest target the Ringleader is aiming at.
    /// </summary>
	private Vector2 latestTarget;
    /// <summary>
    /// Reference to all the Ringleader's sprites.
    /// </summary>
	Component[] spriteMeshes;
    /// <summary>
    /// Flag if the teleporter is performing its teleportation function.
    /// </summary>
	private bool isTeleporting = false;
    /// <summary>
    /// Reference to the player avatar's status.
    /// </summary>
	private PlayerYuni player;
    /// <summary>
    /// Reference to the Ringleader's health.
    /// </summary>
	EnemyHealth enemyHealth;

    /// <summary>
    /// Reference to the Ringleader's animatable.
    /// </summary>
	[SerializeField] private RingleaderAnimatable ringleaderAnimatable;
    
    /// <summary>
    /// Reference to the Ringleader's rectangular tranform.
    /// </summary>
	private RectTransform rectTransform;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		spriteMeshes = GetComponentsInChildren<Anima2D.SpriteMeshInstance> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		rectTransform = GetComponent<RectTransform> ();
	}

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
		yuniPos = FindObjectOfType<PlayerYuni> ().transform;

		this.ringleaderAnimatable = gameObject.GetComponent<RingleaderAnimatable> ();
		General.CheckIfNull (ringleaderAnimatable, "ringleaderAnimatable", scriptName);
	}

    /// <summary>
    /// Teleport towards a target position.
    /// </summary>
    /// <param name="pos"></param>
	public void TeleportTo(Vector3 pos) {
		StartCoroutine (TeleportingTo (pos));
	}

    /// <summary>
    /// Teleport near the player avatar.
    /// </summary>
	public void TeleportToPlayer() {
		StartCoroutine (TeleportingToPlayer ());
	}

    /// <summary>
    /// Coroutine done when the Ringleader is teleporting towards a target position.
    /// </summary>
    /// <param name="pos">Target Position</param>
    /// <returns>None</returns>
    private IEnumerator TeleportingTo(Vector3 pos) {
		isTeleporting = true;

		ringleaderAnimatable.TeleportOut ();
		while (ringleaderAnimatable.IsPlaying ())
			yield return null;

		Vector2 targetPos = new Vector2 (pos.x, pos.y);

		transform.position = targetPos;

		enemyHealth.enabled = true;

		ringleaderAnimatable.TeleportIn ();
		while (ringleaderAnimatable.IsPlaying ())
			yield return null;

		yield return new WaitForSeconds (2.0f);
		isTeleporting = false;
	}

    /// <summary>
    /// Coroutine done when the Ringleader is teleporting near the player avatar.
    /// </summary>
    /// <returns>None</returns>
	IEnumerator TeleportingToPlayer () {
		isTeleporting = true;

		ringleaderAnimatable.TeleportOut ();
		while (ringleaderAnimatable.IsPlaying ())
			yield return null;

		int side = Random.Range (0, 2);

		switch (side) {
		case 0:
			transform.position = new Vector2 (latestTarget.x - rectTransform.rect.width, latestTarget.y);
			break;
		case 1:
			transform.position = new Vector2 (latestTarget.x + rectTransform.rect.width, latestTarget.y);
			break;
		}

		ringleaderAnimatable.TeleportIn ();
		while (ringleaderAnimatable.IsPlaying ())
			yield return null;

		yield return new WaitForSeconds (2.0f);
		isTeleporting = false;
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
    void Update () {
		if (ringleaderAnimatable == null) {
			this.ringleaderAnimatable = gameObject.GetComponent<RingleaderAnimatable> ();
		}

		yuniPos = this.GetPlayer().gameObject.transform;
		latestTarget = new Vector2 (yuniPos.position.x, yuniPos.position.y);
		Random.Range (0, 2);
	}

    /// <summary>
    /// Returns the player avatar's status.
    /// </summary>
    /// <returns>Player Avatar's Status</returns>
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}

    /// <summary>
    /// Sets the reference to the player avatar's status.
    /// </summary>
    /// <param name="playerInstance"></param>
	public void SetPlayer(PlayerYuni playerInstance) {
		this.player = playerInstance;
	}

    /// <summary>
    /// Coroutine done when the Ringleader teleports. Fades out the Ringleader's sprites.
    /// </summary>
    /// <param name="sm">A sprite instance</param>
    /// <param name="duration">Duration for the fading.</param>
    /// <returns></returns>
	IEnumerator FadeOut(Anima2D.SpriteMeshInstance sm, float duration) {
		ringleaderAnimatable.TeleportOut ();

		while (ringleaderAnimatable.IsPlaying ()) {
			yield return null;
		}
	}

    /// <summary>
    /// Coroutine done when the Ringleader teleports. Fades the Ringleader's sprites into visibility.
    /// </summary>
    /// <param name="sm">A sprite instance</param>
    /// <param name="duration">Duration for the fading.</param>
    /// <returns></returns>
	IEnumerator FadeIn(Anima2D.SpriteMeshInstance sm, float duration) {
		ringleaderAnimatable.TeleportIn ();

		while (ringleaderAnimatable.IsPlaying ()) {
			yield return null;
		}
	}

    /// <summary>
    /// Checks if the Ringleader is teleporting.
    /// </summary>
    /// <returns></returns>
	public bool IsTeleporting() {
		return this.isTeleporting;
	}

}
