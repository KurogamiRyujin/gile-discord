using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles any tutorial cutscenes in the room.
/// </summary>
public class TutorialController : MonoBehaviour {

	Cutscene[] cutscenes;

	// Use this for initialization
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.DISABLE_TUTORIALS, DisableAllTutorials);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.DISABLE_TUTORIALS);
	}

	void Start () {
		cutscenes = FindObjectsOfType<Cutscene> ();
		foreach (Cutscene cutscene in cutscenes) {
			Debug.Log ("Cutscene "+cutscene.gameObject.name);
			//			cutscene.gameObject.SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Disables all tutorials in the room.
    /// </summary>
	void DisableAllTutorials() {
//		cutscenes = FindObjectsOfType<Cutscene> ();
		foreach (Cutscene cutscene in cutscenes) {
			Debug.Log ("Disabled " + cutscene.gameObject.name);
			cutscene.gameObject.SetActive (false);
		}
	}
}
