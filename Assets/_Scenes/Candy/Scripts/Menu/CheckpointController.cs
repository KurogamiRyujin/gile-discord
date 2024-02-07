using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	[SerializeField] private GameObject pnlContainer;

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_CHECKPOINT, Show);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_CHECKPOINT);

	}
		
	void Start() {
		this.QuietHide ();
	}

	/// <summary>
	/// Show this instance.
	/// </summary>
	public void Show() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.pnlContainer.SetActive(true);
		GameController_v7.Instance.GetPauseController ().Pause ();
	}

	/// <summary>
	/// Hide this instance without button sound
	/// </summary>
	public void QuietHide() {
		this.pnlContainer.SetActive(false);
		GameController_v7.Instance.GetPauseController ().Continue ();
	}
	/// <summary>
	/// Hide this instance.
	/// </summary>
	public void Hide() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		// Call this to re-enable CheckpointCollider in End scenes
		EventBroadcaster.Instance.PostEvent (EventNames.HIDE_CHECKPOINT);
		this.QuietHide ();
	}

	/// <summary>
	/// Determines whether this instance is open.
	/// </summary>
	/// <returns><c>true</c> if this instance is open; otherwise, <c>false</c>.</returns>
	public bool IsOpen() {
		if (this.GetContainer ().activeInHierarchy) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Goes back to home screen.
	/// </summary>
	public void GoBackToHome() {
		this.Hide ();
		if(FindObjectOfType<SceneObjectsController>() != null)
			Destroy (FindObjectOfType<SceneObjectsController>().gameObject);
		Destroy (GameObject.FindGameObjectWithTag ("GameController").gameObject);
		FindObjectOfType<LoadingScreen> ().LoadScene ("Discord_Main", false);
	}

	/// <summary>
	/// Gets the container.
	/// </summary>
	/// <returns>The container.</returns>
	public GameObject GetContainer() {
		if (this.pnlContainer == null) {
			this.pnlContainer = GetComponentInChildren<CheckpointContainer> ().gameObject;
		}
		return this.pnlContainer;
	}
}
