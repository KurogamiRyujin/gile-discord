using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Script attached to the Select Scene screen
/// </summary>
public class SelectSceneScreen : MonoBehaviour {

    /// <summary>
    /// Reference to the header displayed above the scroll pane
    /// </summary>
	[SerializeField] TextMeshProUGUI headerText;
    /// <summary>
    /// Reference to the ScrollRect component attached to this gameobject
    /// </summary>
	[SerializeField] ScrollRect scrollContainers;
    /// <summary>
    /// Array of RectTransforms under the ScrollRect component
    /// </summary>
	[SerializeField] RectTransform[] contents;
    //	[SerializeField] ScrollRect[] scrollContainers;
    /// <summary>
    /// Data path of the folder where saved files of the current user is located
    /// </summary>
    string folderDataPath;
    /// <summary>
    /// Instance of the UserData class used for saving and loading data
    /// </summary>
	UserData userData;
    /// <summary>
    /// Current level of user in Addition of Similar Fractions topic
    /// </summary>
	int currentLevel1;
    /// <summary>
    /// Current level of user in Subtraction of Similar Fractions topic
    /// </summary>
	int currentLevel2;
    /// <summary>
    /// Current level of user in Addition and Subtraction of Dissimilar Fractions topic
    /// </summary>
	int currentLevel3;

    /// <summary>
    /// Enum that holds the different topics
    /// </summary>
    public enum Topic { 
		ADD, 
		SUB, 
		MIXED
	};


    /// <summary>
    /// Refers to the original position of the child RectTransform of the ScrollRect component
    /// </summary>
    Vector3 origPosition;
    /// <summary>
    /// Current topic being browsed by the user
    /// </summary>
	Topic currentTopic;


    /// <summary>
    /// Reference to the ScrollSnapRect component of this gameobject
    /// </summary>
    [SerializeField] ScrollSnapRect scroll;
    //	ScrollSnapRect1 scroll1;
    //	ScrollSnapRect2 scroll2;

    /// <summary>
    /// Unity Function. Called once upon creation of the object 
    /// </summary>
    void Awake() {
//		LoadUserData (Topic.ADD);
		origPosition = new Vector3 (486, 0, 0);
	}

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
		gameObject.SetActive (false);
		folderDataPath = "";
		userData = new UserData ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Opens the menu
    /// </summary>
    /// <param name="header">String to be put in the header</param>
    /// <param name="topic">Chosen topic of the user</param>
	public void OpenMenu(string header, Topic topic) {
		gameObject.SetActive (true);
		folderDataPath = Application.persistentDataPath + "/Game Data/user_"+PlayerPrefs.GetString("Username") + "/UserData.json"; 
		headerText.text = header.ToUpper();
		currentTopic = topic;
		LoadUserData (topic);
	}

    /// <summary>
    /// Closes the menu
    /// </summary>
	public void CloseMenu() { 
		gameObject.SetActive (false);
	}

    /// <summary>
    /// Disables all child RectTransforms of the ScrollRect component
    /// </summary>
	void DisableAll() {
		foreach (RectTransform s in contents)
			s.gameObject.SetActive (false);
	}

    /// <summary>
    /// Called on click of 'Confirm' button
    /// </summary>
	public void OnConfirmClick() {
//		if (currentTopic == Topic.ADD) {
			bool flag = gameObject.GetComponentInChildren<LevelSelector> ().isSceneLocked (currentTopic, scroll.GetSceneIndex ());
		Debug.Log ("Locked " + flag);
//			if (!flag) { // TODO
				FindObjectOfType<LoadingScreen> ().LoadScene (scroll.GetCurrentScene (), false);
//			}
	}

    /// <summary>
    /// loads saved user data, if it exists
    /// </summary>
    /// <param name="topic">Chosen topic of the user</param>
	void LoadUserData(Topic topic) {
		Debug.Log (folderDataPath);
		if (File.Exists (folderDataPath)) {
			string data = File.ReadAllText (folderDataPath);
			userData = JsonUtility.FromJson<UserData> (data);

			switch (topic) {
			case Topic.ADD:
				Debug.Log ("Add");
				DisableAll ();
				contents [0].gameObject.SetActive (true);
//				scrollContainers [0].gameObject.SetActive (true);
				scroll = scrollContainers.gameObject.GetComponent<ScrollSnapRect> ();
				scroll._container = contents [0]; 
				scroll._scrollRectRect.gameObject.GetComponent<ScrollRect> ().content = contents [0];
				contents [0].localPosition = origPosition;
				scroll._container.localPosition = origPosition; 
				scroll.sceneNames = scroll.GetComponent<SceneNamesReader> ().GetAddSceneNames ();
				scroll.SetPageCount (scroll.sceneNames.Length);
				scroll.SetPagePositions ();
				scroll.SetCurrentScene (0);
					gameObject.GetComponentInChildren<LevelSelector> ().LoadUserData (topic, userData.currentAdditionSimilarFractionsLevel);
					break;
			case Topic.SUB:
				Debug.Log ("Sub");
				DisableAll ();
				contents [1].gameObject.SetActive (true);
//				scrollContainers [1].gameObject.SetActive (true);
				scroll = scrollContainers.gameObject.GetComponent<ScrollSnapRect> ();
				scroll._container = contents [1]; 
				scroll._scrollRectRect.gameObject.GetComponent<ScrollRect> ().content = contents [1];
				contents[1].localPosition = origPosition;
				scroll._container.localPosition = origPosition; 
				scroll.sceneNames = scroll.GetComponent<SceneNamesReader> ().GetSubSceneNames ();
				scroll.SetPageCount (scroll.sceneNames.Length);
				scroll.SetPagePositions ();
				scroll.SetCurrentScene (0);
				gameObject.GetComponentInChildren<LevelSelector> ().LoadUserData (topic, userData.currentSubtractionSimilarFractionsLevel);
					break;
			case Topic.MIXED:
				Debug.Log ("Mixed");
				DisableAll ();
				contents [2].gameObject.SetActive (true);
//				scrollContainers.gameObject.SetActive (true);
				scroll = scrollContainers.gameObject.GetComponent<ScrollSnapRect> ();
				scroll._container = contents [2]; 
				scroll._scrollRectRect.gameObject.GetComponent<ScrollRect> ().content = contents [2];
				contents[2].localPosition = origPosition;
				scroll._container.localPosition = origPosition; 
				scroll.sceneNames = scroll.GetComponent<SceneNamesReader> ().GetDisSceneNames ();
				scroll.SetPageCount (scroll.sceneNames.Length);
				scroll.SetPagePositions ();
				scroll.SetCurrentScene (0);
				gameObject.GetComponentInChildren<LevelSelector> ().LoadUserData (topic, userData.currentDissimilarFractionsLevel);
				break;
			}
			Debug.Log ("[SELECT SCENE SCREEN]: Loaded user data");

		} else {
			Debug.Log ("Unable to read the saved user data, file doesn't exist");
			userData = new UserData ();

		}
	}
}
