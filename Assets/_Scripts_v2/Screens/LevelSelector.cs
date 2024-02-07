using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Handles which levels can be accessed by the user
/// </summary>
public class LevelSelector : MonoBehaviour {

    /// <summary>
    /// Reference to the buttons that redirects to scenes under the Addition of Similar Fractions topic
    /// </summary>
    [SerializeField] GameObject[] addSimSceneButtons;
    /// <summary>
    /// Reference to the buttons that redirects to scenes under the Subtraction of Similar Fractions topic
    /// </summary>
	[SerializeField] GameObject[] subSimSceneButtons;
    /// <summary>
    /// Reference to the buttons that redirects to scenes under the Addition and Subtraction of Dissimilar Fractions topic
    /// </summary>
	[SerializeField] GameObject[] disSceneButtons;
    /// <summary>
    /// Reference to the ScrollRect component of this gameobject
    /// </summary>
	ScrollRect scrollRect;

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
		scrollRect = gameObject.GetComponent<ScrollRect> ();
	}


	// Update is called once per frame
	void Update () {

	}

    /// <summary>
    /// Disables all buttons 
    /// </summary>
	void DisableAll() {
		foreach (GameObject go in addSimSceneButtons)
			go.SetActive (false);
		foreach (GameObject go in subSimSceneButtons)
			go.SetActive (false);
		foreach (GameObject go in disSceneButtons)
			go.SetActive (false);
	}

    /// <summary>
    /// Unlocks levels for each topic based on user's current level
    /// </summary>
    /// <param name="topic">Current topic chosen</param>
    /// <param name="levelReached">Current level of user</param>
	public void LoadUserData(SelectSceneScreen.Topic topic, int levelReached) {
		Debug.Log ("Level Reached " + levelReached);
		DisableAll ();
		switch (topic) {
		case SelectSceneScreen.Topic.ADD:
			Debug.Log ("Loaded add level");
			//			Debug.Log (addSimSceneButtons.Length);
			for (int i = 0; i < addSimSceneButtons.Length; i++) {
				addSimSceneButtons [i].gameObject.SetActive (true);
				if (i + 1 > levelReached) {
					addSimSceneButtons [i].transform.GetChild (0).gameObject.SetActive (true);
				}
			}
			break;
		case SelectSceneScreen.Topic.SUB:
			Debug.Log ("Loaded sub level");
			Debug.Log (subSimSceneButtons.Length);
			for (int i = 0; i < subSimSceneButtons.Length; i++) {
				subSimSceneButtons [i].gameObject.SetActive (true);
				if (i + 1 > levelReached)
					subSimSceneButtons [i].transform.GetChild (0).gameObject.SetActive (true);
			}
			break;
		case SelectSceneScreen.Topic.MIXED:
			Debug.Log ("Loaded mixed level");
			Debug.Log (disSceneButtons.Length);
			for (int i = 0; i < disSceneButtons.Length; i++) {
				disSceneButtons [i].gameObject.SetActive (true);
				if (i + 1 > levelReached)
					disSceneButtons [i].transform.GetChild (0).gameObject.SetActive (true);
			}
			break;
		}
	}

    /// <summary>
    /// Checks if a scene is unlocked or not
    /// </summary>
    /// <param name="topic">Current topic chosen</param>
    /// <param name="index">Index in array of buttons</param>
    /// <returns>Returns true if clicked button leads to an unlocked scene; false, if otherwise</returns>
	public bool isSceneLocked(SelectSceneScreen.Topic topic, int index) {
		Debug.Log ("Checking if locked");
		switch (topic) {
		case SelectSceneScreen.Topic.ADD:
			if (addSimSceneButtons [index].transform.GetChild (0).gameObject.activeSelf)
				return true;
			return false;
			break;
		case SelectSceneScreen.Topic.SUB:
			if (subSimSceneButtons [index].transform.GetChild (0).gameObject.activeSelf)
				return true;
			return false;
			break;
		case SelectSceneScreen.Topic.MIXED:
			if (disSceneButtons [index].transform.GetChild (0).gameObject.activeSelf)
				return true;
			return false;
			break;
		}
		return false;
	}

}
