using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to the topic pop-up screen
/// </summary>
public class TopicPopup : MonoBehaviour {
    /// <summary>
    /// Reference to the animator attached to this gameobject
    /// </summary>
	[SerializeField] Animator animator;
    /// <summary>
    /// Reference to the SelectSceneScren script
    /// </summary>
	[SerializeField] SelectSceneScreen selectSceneScreen;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Triggers the animation that opens the popup 
    /// </summary>
    public void OpenMenu() {
		animator.SetBool ("isOpen", true);
	}

    /// <summary>
    /// Triggers the animation that closes the popup 
    /// </summary>
	public void CloseMenu() {
		animator.SetBool ("isOpen", false);
	}

    /// <summary>
    /// Redirects to the Select Scene Screen on Addition of Similar Fractions topic click
    /// </summary>
	public void OnAddSimClick() {
		CloseMenu ();
		selectSceneScreen.OpenMenu ("Tutorials", SelectSceneScreen.Topic.ADD);
	}

    /// <summary>
    /// Redirects to the Select Scene Screen on Subtraction of Similar Fractions topic click
    /// </summary>
    public void OnSubSimClick() {
		CloseMenu ();
		selectSceneScreen.OpenMenu ("Stage 1", SelectSceneScreen.Topic.SUB);
	}

    /// <summary>
    /// Redirects to the Select Scene Screen on Addition and Dissimilar Fractions topic click
    /// </summary>
    public void OnDisClick() {
		CloseMenu ();
		selectSceneScreen.OpenMenu ("Stage 2", SelectSceneScreen.Topic.MIXED);
	}
}
