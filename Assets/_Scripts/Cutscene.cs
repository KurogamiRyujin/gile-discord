using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for cutscene behaviours.
/// 
/// Cutscenes are short interludes where dialogues occur and, if any, CGs are viewed.
/// It can either serve as aesthetic complement or technical aid such as tutorials.
/// </summary>
public abstract class Cutscene : MonoBehaviour {
//	public PlayerPlatformerController playerController;
//	public PlayerAttack playerAttack;
    
	protected int sequenceCount = 0;
    /// <summary>
    /// Flag if the cutscene is playing.
    /// </summary>
	protected bool isPlaying = false;
//	protected string name;
	protected string scene;
    /// <summary>
    /// Flag if the cutscene has been triggered.
    /// </summary>
	protected bool isTriggered = false;

    /// <summary>
    /// Play scenes. Can be overriden to allow implementation of particular cutscenes.
    /// </summary>
	public virtual void PlayScenes() {
		isTriggered = true;
		isPlaying = true;
		GameController_v7.Instance.GetObjectStateManager ().RecordState (this.gameObject, 0, this.scene);
//		objectStateManager.RecordState (this.gameObject, 0, this.scene);
	}

    /// <summary>
    /// Initializes the cutscene behaviour. Can be overriden to allow specific initialization for additional properties.
    /// </summary>
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

    /// <summary>
    /// Disables player controls. Prevents certain actions during cutscenes.
    /// </summary>
	protected abstract void disablePlayerControls ();
    
	public string GetSceneFrom() {
		return this.scene;
	}

	public string GetCutsceneName() {
		return this.name;
	}

    /// <summary>
    /// Checker if the cutscene has been triggered.
    /// </summary>
    /// <returns>If cutscene is already triggered. Otherwise, false.</returns>
	public bool IsTriggered() {
		return this.isTriggered;
	}
}
