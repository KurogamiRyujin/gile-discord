using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

/// <summary>
/// Script attached to the name input box popup
/// </summary>
public class NameInputBox : MonoBehaviour {
    /// <summary>
    /// Reference to the animator attached to this gameobject
    /// </summary>
    [SerializeField] Animator animator;
    /// <summary>
    /// Reference to the input field in the pop-up
    /// </summary>
    [SerializeField] InputField input;
    /// <summary>
    /// Reference to the ChooseGenderScreen script
    /// </summary>
    [SerializeField] ChooseGenderScreen chooseGenderScreen;
    /// <summary>
    /// Instance of the GameData class used for saving and loading data
    /// </summary>
    GameData gameData;
    /// <summary>
    /// Instance of the UserData class used for saving and loading data
    /// </summary>
	UserData userData;
    /// <summary>
    /// Data path of the JSON file for the current user's data
    /// </summary>
	string userDataPath;
    /// <summary>
    /// Data path of the JSON file for the game data
    /// </summary>
	string gameDataPath;
    /// <summary>
    /// Data path of the folder where saved files of the current user is located
    /// </summary>
	string folderDataPath;
    /// <summary>
    /// Name of scene to be loaded
    /// </summary>
	string sceneToLoad;

    /// <summary>
    /// List of registered usernames
    /// </summary>
	List<string> usernames;
    /// <summary>
    /// Number of registered users
    /// </summary>
	int numberOfUsers;

    // Use this for initialization

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    void Start () {
		usernames = new List<string> ();
		numberOfUsers = 0;
		gameData = new GameData ();
		userData = new UserData ();
		folderDataPath = Application.persistentDataPath + "/Game Data";
		gameDataPath = folderDataPath + "/GameData.json";
//		sceneToLoad = "Beta Testing Slice and Fill Tutorial 2 Slice and Fill";
//		sceneToLoad = "Beta Testing Slice and Fill Tutorial 1 Pickup and Fill";
		sceneToLoad = "Playtesting Backstage";
//		sceneToLoad = "Arbitrary_Equivalent_0";

		if(PlayerPrefs.HasKey("Username"))
			PlayerPrefs.DeleteKey ("Username");

//		CloseMenu ();
//		gameObject.SetActive (false);

	}

	// Update is called once per frame
	void Update () {
//		
	}

    /// <summary>
    /// called on value change of the input field
    /// </summary>
	public void OnEditText() {
		if (!CheckIfValid (input.text))
			input.transform.Find("Text").GetComponent<Text>().color = Color.red;
		else
			input.transform.Find("Text").GetComponent<Text>().color = Color.black;
	}

    /// <summary>
    /// Called on click of the check button
    /// </summary>
	public void OnSubmit() {
		string username = input.text;
		if (CheckIfValid (username)) {
			Debug.Log ("Entered username: " + username);
			numberOfUsers++;
			usernames.Add (username);

			folderDataPath = Application.persistentDataPath + "/Game Data/user_" +username;
			if (!Directory.Exists (folderDataPath)) {
				Directory.CreateDirectory (folderDataPath);
			}

			userDataPath = folderDataPath + "/UserData.json";

			PlayerPrefs.SetString ("Username", username);
			SaveUserData (username);
			SaveGameData ();
			CloseMenu ();

			PedagogicalComponent_v2.Instance.InitializeLearnerModelingComponent ();

            chooseGenderScreen.OpenMenu();
			//FindObjectOfType<LoadingScreen> ().LoadScene (sceneToLoad, false);
		} else {
			Debug.Log ("Invalid username");
		}
	}

    /// <summary>
    /// Creates a new JSON file for the newly created user
    /// </summary>
	void SaveUserData(string username) {
		userData.username = username;
        userData.gender = "none";
		userData.lastSceneVisited = "none";
		userData.currentAdditionSimilarFractionsLevel = 1;
		userData.currentSubtractionSimilarFractionsLevel = 1;
		userData.currentDissimilarFractionsLevel = 1;
//		userData.currentAdditionDissimilarFractionsLevel = 1;
//		userData.currentSubtractionDissimilarFractionsLevel = 1;
//		userData.currentAdditionEquivalentFractionsLevel = 1;
//		userData.currentSubtractionEquivalentFractionsLevel = 1;

		string newData = JsonUtility.ToJson (userData, true);
		File.WriteAllText (userDataPath, newData);
		userData = new UserData ();

        Debug.LogError("SAVED USER DATA");
		Debug.Log ("Saved user data");
	}

    /// <summary>
    /// Updates the list of registered usernames and number of registered users
    /// </summary>
	void SaveGameData() {
		gameData.numberOfUsers = numberOfUsers;
		gameData.usernames = usernames;
		string newData = JsonUtility.ToJson (gameData, true);
		File.WriteAllText (gameDataPath, newData);
		gameData = new GameData ();

		Debug.Log ("Saved game data");
	}

    /// <summary>
    /// Loads saved game data, if it exists
    /// </summary>
    void LoadGameData() {
		if (File.Exists (gameDataPath)) {
			string data = File.ReadAllText (gameDataPath);
			gameData = JsonUtility.FromJson<GameData> (data);

			numberOfUsers = gameData.numberOfUsers;
			usernames = gameData.usernames;

			Debug.Log ("[INPUT BOX] Loaded game data");

		} else {
			Debug.LogError ("Unable to read the saved game data, file doesn't exist");
			gameData = new GameData ();

			numberOfUsers = 0;
			usernames = new List<string> ();
		}
	}

    /// <summary>
    /// Triggers the animation that closes the menu
    /// </summary>
	public void CloseMenu() {
		input.text = "";
		animator.SetBool ("isOpen", false);
	}


    /// <summary>
    /// Triggers the animation that opens the menu
    /// </summary>
    public void OpenMenu() {
//		gameObject.SetActive (true);
		animator.SetBool ("isOpen", true);
		LoadGameData ();
	}

    /// <summary>
    /// Checks if the entered username doesn't exist yet
    /// </summary>
    /// <param name="entered">Entered username</param>
    bool CheckIfValid(string entered) {
		if (usernames.Count == 0) {
			if (entered.Length >= 3) {
				return true;
			} else {
				
				return false;
			}
		}
		else {
//			bool doesUsernameExist = usernames.Contains (entered);
			bool doesUsernameExist = usernames.Any (x => x == entered);
			if (!doesUsernameExist) {
				if (entered.Length >= 3)
					return true;
				return false;
			} else
				return false;
		}

		return false;
//		if (usernames.Count > 0) {
//			bool doesUsernameExist = usernames.Exists (x => x = entered);
//			Debug.Log ("Is username existing " + doesUsernameExist);
//			if (entered.Length < 5 || doesUsernameExist)
//				text.color = Color.red;
//			else
//				text.color = Color.black;
//		} else {
//			if (entered.Length < 5)
//				text.color = Color.red;
//			else
//				text.color = Color.black;
//		}
	}
}
