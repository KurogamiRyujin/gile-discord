using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

/// <summary>
/// Script attached to the User's Progress Screen 
/// </summary>
public class UsersProgressScreen : MonoBehaviour {

    /// <summary>
    /// Reference to the animator attached to this gameobject
    /// </summary>
	[SerializeField] Animator animator;
    /// <summary>
    /// Reference to the container of user panels
    /// </summary>
	[SerializeField] GameObject container;
    /// <summary>
    /// Reference to the prefab of the user panels
    /// </summary>
	[SerializeField] GameObject userPrefab;
    /// <summary>
    /// Reference to the UserLoadOptionsScreen script
    /// </summary>
	[SerializeField] UserLoadOptionsScreen loadOptionsScreen;

    /// <summary>
    /// Instance of the GameData class used for saving and loading data
    /// </summary>
	GameData gameData;
    /// <summary>
    /// Data path of the JSON file for the game data
    /// </summary>
	string gameDataPath;
    /// <summary>
    /// Data path of the folder where saved files of the current user is located
    /// </summary>
	string folderDataPath;
    /// <summary>
    /// Reference to the ScrollRect script attached to this gameobject
    /// </summary>
	ScrollRect scrollRect;
    /// <summary>
    /// Flag for loading the game data
    /// </summary>
	bool isLoaded;

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
//		gameObject.SetActive (false);
		gameData = new GameData();
		folderDataPath = Application.persistentDataPath + "/Game Data";	
		gameDataPath = folderDataPath + "/GameData.json";
		scrollRect = GetComponentInChildren<ScrollRect> ();
		isLoaded = false;

//		CloseMenu();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Triggers the animation that opens the menu 
    /// </summary>
	public void OpenMenu() {
//		gameObject.SetActive (true);
		animator.SetBool ("isOpen", true);
		if (!isLoaded) {
			LoadGameData ();
			isLoaded = true;
		}
	}

    /// <summary>
    /// Triggers the animation that closes the menu 
    /// </summary>
	public void CloseMenu() {
//		gameObject.SetActive (false);
		animator.SetBool ("isOpen", false);
//		animator.Play ("Close_Menu");
	}

    /// <summary>
    /// Called when a user panel is clicked
    /// </summary>
    /// <param name="username">Username of clicked user panel</param>
	public void OnUserPanelClick(string username) {
		//load saved progress
		CloseMenu();
		PlayerPrefs.SetString ("Username", username);
		PedagogicalComponent_v2.Instance.InitializeLearnerModelingComponent ();
		loadOptionsScreen.OpenMenu (username);
	}

    /// <summary>
    /// Loads the game data, if it exists
    /// </summary>
	void LoadGameData() {
		if (File.Exists (gameDataPath)) {
			string data = File.ReadAllText (gameDataPath);
			gameData = JsonUtility.FromJson<GameData> (data);

			if (gameData.usernames.Count > 0) {
				foreach (string user in gameData.usernames) {
//					Debug.Log (user);
					GameObject obj = Instantiate (userPrefab, container.transform);
					obj.GetComponentInChildren<TextMeshProUGUI> ().text = user;
					obj.GetComponent<Button>().onClick.AddListener(() => { OnUserPanelClick(user); });
				}
			}

			Debug.Log ("[PROGRESS SCREEN]: Loaded game data");

		} else {
			Debug.LogError ("Unable to read the saved game data, file doesn't exist");
			gameData = new GameData ();

		}
	}


}
