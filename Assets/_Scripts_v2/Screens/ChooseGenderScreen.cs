using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script attached to the Choose Gender screen
/// </summary>
public class ChooseGenderScreen : MonoBehaviour {
    /// <summary>
    /// Instance of the UserData class used for saving and loading data
    /// </summary>
    UserData userData;
    /// <summary>
    /// Instance of the UserData class used for saving and loading data
    /// </summary>
    UserData loadedUserData;
    /// <summary>
    /// Data path of the JSON file for the current user's data
    /// </summary>
    string userDataPath;
    /// <summary>
    /// Data path of the folder where saved files of the current user is located
    /// </summary>
    string folderDataPath;
    /// <summary>
    /// Name of the scene to be loaded
    /// </summary>
    string sceneToLoad;

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
        CloseMenu();
        userData = new UserData();
        sceneToLoad = "Playtesting Backstage";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called when the user clicks one of the gender buttons
    /// </summary>
    /// <param name="gender">Can be either male or female</param>
    public void OnClick(string gender) {
        SaveUserData(gender);
        CloseMenu();
        //if (newUser) {
        if (loadedUserData.lastSceneVisited == "none") {
            Debug.LogError("New user");
            FindObjectOfType<LoadingScreen>().LoadScene(sceneToLoad, false);
        }
        //else {
        //    Debug.LogError("Old user");
        //    FindObjectOfType<LoadingScreen>().LoadScene(loadedUserData.lastSceneVisited, false);
        //}
        //}
    }

    /// <summary>
    /// Triggers the animation that closes the menu 
    /// </summary>
    public void CloseMenu() {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Triggers the animation that opens the menu 
    /// </summary>
    public void OpenMenu() {
        gameObject.SetActive (true);
        folderDataPath = Application.persistentDataPath + "/Game Data/user_" + PlayerPrefs.GetString("Username");
        userDataPath = folderDataPath + "/UserData.json";
        LoadUserData();
    }

    /// <summary>
    /// Loads saved user data, if it exists
    /// </summary>
    void LoadUserData() {
        //Debug.LogError(userDataPath);
        if (File.Exists(userDataPath)) {
            string data = File.ReadAllText(userDataPath);
            loadedUserData = JsonUtility.FromJson<UserData>(data);
            
            Debug.LogError(loadedUserData.gender);
        }
        else {
            //			Debug.LogError ("Unable to read the saved data, file doesn't exist");
            userData = new UserData();
        }
    }

    /// <summary>
    /// Saves user data
    /// </summary>
    /// <param name="gender">Can be either male or female</param>
    void SaveUserData(string gender) {
        userData.username = loadedUserData.username;
        userData.gender = gender;
        userData.lastSceneVisited = loadedUserData.lastSceneVisited;
        userData.currentAdditionSimilarFractionsLevel = loadedUserData.currentAdditionSimilarFractionsLevel;
        userData.currentSubtractionSimilarFractionsLevel = loadedUserData.currentSubtractionSimilarFractionsLevel;
        userData.currentDissimilarFractionsLevel = loadedUserData.currentDissimilarFractionsLevel;


        string newData = JsonUtility.ToJson(userData, true);
        File.WriteAllText(userDataPath, newData);
        userData = new UserData();

        Debug.Log("Saved user data");

    }


}
