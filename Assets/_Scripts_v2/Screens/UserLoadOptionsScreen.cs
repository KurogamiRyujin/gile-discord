
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script attached to the User Load Options Screen
/// </summary>
public class UserLoadOptionsScreen : MonoBehaviour {

    /// <summary>
    /// Reference to the textbox that displays the name of the current user
    /// </summary>
    [SerializeField] TextMeshProUGUI usernameText;
    /// <summary>
    /// Reference to the TopicPopup script
    /// </summary>
    [SerializeField] TopicPopup topicPopup;

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
		CloseMenu();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Triggers the animation that opens the menu 
    /// </summary>
	public void OpenMenu(string username) {
		gameObject.SetActive (true);
		usernameText.text = username;
	}

    /// <summary>
    /// Triggers the animation that closes the menu 
    /// </summary>
	public void CloseMenu() {
		gameObject.SetActive (false);
	}

    /// <summary>
    /// Triggers the animation that opens the topic popup on click of the 'Select' button
    /// </summary>
	public void OnSelectSceneClick() {
//		CloseMenu ();
		topicPopup.OpenMenu ();
	}
		
}
