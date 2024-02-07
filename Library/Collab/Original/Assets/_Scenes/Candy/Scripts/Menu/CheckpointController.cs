using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	[SerializeField] private GameObject pnlContainer;

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_CHECKPOINT, Show);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.SHOW_CHECKPOINT, Show);

	}

	void Start() {
		this.QuietHide ();
	}
	public void Show() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.pnlContainer.SetActive(true);
		GameController_v7.Instance.GetPauseController ().Pause ();
	}

	public void QuietHide() {
		this.pnlContainer.SetActive(false);
		GameController_v7.Instance.GetPauseController ().Continue ();
	}
	public void Hide() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.QuietHide ();
	}

	public void GoBackToHome() {
		this.Hide ();
		if(FindObjectOfType<SceneObjectsController>() != null)
			Destroy (FindObjectOfType<SceneObjectsController>().gameObject);
		Destroy (GameObject.FindGameObjectWithTag ("GameController").gameObject);
		FindObjectOfType<LoadingScreen> ().LoadScene ("Discord_Main", false);
	}

	public GameObject GetContainer() {
		if (this.pnlContainer == null) {
			this.pnlContainer = GetComponentInChildren<CheckpointContainer> ().gameObject;
		}
		return this.pnlContainer;
	}
}
