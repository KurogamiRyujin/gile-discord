using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for a save point which saves the player's progress.
/// </summary>
public class SavePoint : MonoBehaviour {

    /// <summary>
    /// Reference to the mobile UI.
    /// </summary>
	MobileUI mobile;
    /// <summary>
    /// Reference to the UI for the save point.
    /// </summary>
	SavePointText savePointText;
    /// <summary>
    /// Flag if the player is inside the save point.
    /// </summary>
	bool isInsideSavePoint;

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
		#if UNITY_ANDROID
		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif
		savePointText = GetComponentInChildren<SavePointText> ();
		isInsideSavePoint = false;
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
    void Update () {
		#if UNITY_ANDROID
		if (mobile.interactPressed) {
			mobile.interactPressed = false;
			SaveData();
		}
		#elif UNITY_STANDALONE
		if (Input.GetKeyDown (KeyCode.N)) {
			SaveData();
		} 
		#endif
	}

    /// <summary>
    /// Prompt to save the player's data.
    /// </summary>
	void SaveData() {
		Debug.Log("Save data triggered from save point");
		savePointText.Hide();
		Parameters parameters = new Parameters();
		parameters.PutExtra ("IS_HARD_SAVE", true);
		EventBroadcaster.Instance.PostEvent (EventNames.SAVE_DATA, parameters);
	}

    /// <summary>
    /// Unity Function. Called when the game object's collider detects another collider.
    /// 
    /// Used to check if the player is in the save point zone.
    /// </summary>
    /// <param name="other"></param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			Debug.Log ("Player inside save point");
			isInsideSavePoint = true;
			savePointText.Show();
		}
	}

    /// <summary>
    /// Unity Function. Called when another collider leaves the game object's collider.
    /// </summary>
    /// <param name="other"></param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			Debug.Log ("Player exited save point");
			isInsideSavePoint = false;
			savePointText.Hide();
		}
	}
}
