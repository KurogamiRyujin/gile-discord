using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as the checkpoint while the player avatar traverses the game.
/// 
/// Either prompts the player if they wish to continue or return to the main menu or automatically returns them to the main menu.
/// </summary>
public class ToHomeCollider : MonoBehaviour {
	

	private PlayerYuni player;
	private bool isPlaying;
	private bool isTriggered;
    /// <summary>
    /// Flag if the player is automatically returns to the main menu upon reaching the checkpoint.
    /// </summary>
	[SerializeField] private bool isAutomatic;

	void Awake() {
		this.isPlaying = false;
		this.isTriggered = false;
		EventBroadcaster.Instance.AddObserver (EventNames.HIDE_CHECKPOINT, StopPlaying);
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.HIDE_CHECKPOINT);
	}
	private PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
	private IEnumerator SetZero() {
		this.isPlaying = true;
		this.isTriggered = true;

		if (this.isAutomatic) {
			this.ToHome ();
		} else {
			this.ShowCheckpoint();
		}
		yield return null;
	}

    /// <summary>
    /// Broadcasts for a menu to popup.
    /// 
    /// Menu asks the player if they want to continue playing through or return to the main menu.
    /// </summary>
	public void ShowCheckpoint() {
		EventBroadcaster.Instance.PostEvent(EventNames.SHOW_CHECKPOINT);
	}

	/// <summary>
    /// Returns the player to the main menu.
    /// </summary>
	public void ToHome() {
		if (FindObjectOfType<SceneObjectsController> () != null) {
			Destroy (FindObjectOfType<SceneObjectsController>().gameObject);
		}
		Destroy (GameObject.FindGameObjectWithTag ("GameController").gameObject);
		FindObjectOfType<LoadingScreen> ().LoadScene ("Discord_Main", false);
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			PlayScenes ();
		}
	}
	public void StopPlaying() {
		this.isPlaying = false;
	}
	public void PlayScenes() {
//		if (!isPlaying && !isTriggered) {
		if (!isPlaying) {
			StartCoroutine (SetZero ());
		}
	}
}
