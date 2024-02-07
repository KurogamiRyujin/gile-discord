using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CutscenePersistent : MonoBehaviour {
//	public PlayerPlatformerController playerController;
//	public PlayerAttack playerAttack;

	protected int sequenceCount = 0;
	protected bool isPlaying = false;
//	protected string name;
	protected string scene;
	protected bool isTriggered = false;

	public virtual void PlayScenes() {
		isTriggered = true;
		isPlaying = true;
		GameController_v7.Instance.GetObjectStateManager ().RecordState (this.gameObject, 0, this.scene);
//		objectStateManager.RecordState (this.gameObject, 0, this.scene);
	}

	protected virtual void Init() {
		this.scene = SceneManager.GetActiveScene ().name;

		if (PlayerPrefs.HasKey (scene + "_" + name)) {
			string cutsceneJSON = PlayerPrefs.GetString (scene + "_" + name);
			CutsceneJSONParams cutsceneStats = JsonUtility.FromJson<CutsceneJSONParams> (cutsceneJSON);
//			this.isTriggered = cutsceneStats.isTriggered;
		}
	}

//	protected void disablePlayerControls() {
//		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerPlatformerController> ();
//	    playerAttack = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();
//		playerController.canMove (false);
//		playerAttack.canAttack (false);
//	}

	protected abstract void disablePlayerControls ();

	public string GetSceneFrom() {
		return this.scene;
	}

	public string GetCutsceneName() {
		return this.name;
	}

	public bool IsTriggered() {
		return this.isTriggered;
	}
}
