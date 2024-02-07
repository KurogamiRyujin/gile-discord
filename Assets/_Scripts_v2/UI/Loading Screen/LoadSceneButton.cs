using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Handles scene loading on button click
/// </summary>
public class LoadSceneButton : MonoBehaviour {

    /// <summary>
    /// Name of scene to be loaded
    /// </summary>
    [SerializeField] string sceneToLoad;
    /// <summary>
    /// Data path of the JSON file for Yuni's stats
    /// </summary>
    string playerDataPath;
    /// <summary>
    /// Instance of the UserData class used for saving and loading data
    /// </summary>
	UserData userData;

    /// <summary>
    /// Unity Function. Called once upon creation of the object 
    /// </summary>
	void Awake() {
		userData = new UserData ();
		if (PlayerPrefs.HasKey ("Username"))
			playerDataPath = Application.persistentDataPath + "/Game Data/user_" + PlayerPrefs.GetString ("Username")+"/UserData.json";
		else
			playerDataPath = Application.persistentDataPath + "/Game Data/user_default/UserData.json";
		GetComponent<Button> ().onClick.AddListener (LoadScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Loads a scene
    /// </summary>
	void LoadScene() {
		if (File.Exists (playerDataPath) && !sceneToLoad.Equals("Discord_Main")) {
			string data = File.ReadAllText (playerDataPath);
			userData = JsonUtility.FromJson<UserData> (data);
			sceneToLoad = userData.lastSceneVisited;
		}

		print ("<color='red'>SCENE TO LOAD: " + sceneToLoad+ "</color>");
		FindObjectOfType<LoadingScreen> ().LoadScene (sceneToLoad, true);
	}
}
