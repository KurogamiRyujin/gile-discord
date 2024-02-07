using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the functionality for the menu.
/// </summary>
public class MenuController : MonoBehaviour {

	[SerializeField] private GameObject pnlContainer;
	[SerializeField] private GameObject pnlButtons;
	[SerializeField] private GameObject pnlConfirmHome;
	[SerializeField] private GameObject pnlConfirmPlay;
	[SerializeField] private GameObject pnlConfirmRetry;
	[SerializeField] private GameObject pnlOptions;

	[SerializeField] private Button btnHome;
	[SerializeField] private Button btnPlay;
	[SerializeField] private Button btnQuit;
	[SerializeField] private Button btnOptions;

    /// <summary>
    /// Flag if the resume button is pressed.
    /// </summary>
	private bool isPlayPressed;
    /// <summary>
    /// Flag if the retry button is pressed.
    /// </summary>
	private bool isRetryPressed;
    /// <summary>
    /// Flag if the help button is pressed.
    /// </summary>
	private bool isHelpPressed;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
//		this.pnlConfirmHome = GetComponentInChildren<PanelConfirmHome> ().gameObject;
//		this.pnlConfirmPlay = GetComponentInChildren<PanelConfirmPlay> ().gameObject;
//		this.pnlConfirmQuit = GetComponentInChildren<PanelConfirmQuit> ().gameObject;
//		this.pnlButtons = GetComponentInChildren<ButtonContainer> ().gameObject;

		GameController_v7.Instance.GetMobileUIManager().SubscribeMenuController(this);
	}

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start() {
		GameController_v7.Instance.GetMobileUIManager ().HideMenu ();
	}

    /// <summary>
    /// Display the menu overlay and pause the game.
    /// </summary>
	public void Show() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.ShowButtons ();
		this.pnlContainer.SetActive(true);
		GameController_v7.Instance.GetPauseController ().Pause ();
	}

    /// <summary>
    /// Hide the menu overlay and continue the game.
    /// </summary>
	public void Hide() {
		this.pnlContainer.SetActive(false);
		GameController_v7.Instance.GetPauseController ().Continue ();
	}

    /// <summary>
    /// Hide the panels.
    /// </summary>
	public void HidePanels() {
		this.GetButtonContainer ().gameObject.SetActive (false);
		this.GetPanelConfirmHome ().gameObject.SetActive (false);
		this.GetPanelConfirmRetry ().gameObject.SetActive (false);
		this.GetPanelConfirmPlay ().gameObject.SetActive (false);
        this.GetPanelOptions().gameObject.SetActive(false);
    }

    /// <summary>
    /// Reset the room puzzle.
    /// </summary>
	public void Retry() {
		// Activate Everything
		EventBroadcaster.Instance.PostEvent(EventNames.RETRY);
		EventBroadcaster.Instance.PostEvent(EventNames.ACQUIRE_ALL);
		// TODO : Retry stage
		this.Hide();
	}

    /// <summary>
    /// Checker if the play button was pressed.
    /// </summary>
    /// <returns>If play button was pressed. Otherwise, false.</returns>
	public bool IsPlayPressed() {
		return this.isPlayPressed;
	}
//	public bool IsHelpPressed() {
//		return this.isHelpPressed;
//	}
//	public void HelpUp() {
//		this.isHelpPressed = false;
//	}
//	public void HelpDown() {
//		this.isHelpPressed = true;
//	}
    
    /// <summary>
    /// Called when the player releases the press on the play button.
    /// Sets the isPlayPressed flag to false.
    /// </summary>
	public void PlayUp() {
		this.isPlayPressed = false;
	}

    /// <summary>
    /// Called when the player starts the press on the play button.
    /// Sets the isPlayPressed flag to true.
    /// </summary>
	public void PlayDown() {
		this.isPlayPressed = true;
	}

    /// <summary>
    /// Checker if the retry button is pressed.
    /// </summary>
    /// <returns>If retry button is pressed. Otherwise, false.</returns>
	public bool IsRetryPressed() {
		return this.isRetryPressed;
	}

    /// <summary>
    /// Called when the player releases the press on the retry button.
    /// Sets the isRetryPressed flag to false.
    /// </summary>
	public void RetryUp() {
		this.isRetryPressed = false;
	}

    /// <summary>
    /// Called when the player starts the press on the retry button.
    /// Sets the isRetryPressed flag to true.
    /// </summary>
	public void RetryDown() {
		this.isRetryPressed = true;
	}

    /// <summary>
    /// Return to the main menu.
    /// </summary>
	public void GoBackToHome() {
		Hide ();
		if(FindObjectOfType<SceneObjectsController>() != null)
			Destroy (FindObjectOfType<SceneObjectsController>().gameObject);
		Destroy (GameObject.FindGameObjectWithTag ("GameController").gameObject);
		FindObjectOfType<LoadingScreen> ().LoadScene ("Discord_Main", false);
	}
    
	public void ShowButtons() {
		this.HidePanels ();
		this.GetButtonContainer ().gameObject.SetActive (true);
	}
    
	public void ShowConfirmPlay() {
		this.HidePanels ();
		this.GetPanelConfirmPlay ().gameObject.SetActive (true);
	}

	public void ShowConfirmHome() {
		this.HidePanels ();
		this.GetPanelConfirmHome ().gameObject.SetActive (true);
	}

	public void ShowConfirmRetry() {
		this.HidePanels ();
		this.GetPanelConfirmRetry ().gameObject.SetActive (true);
	}

	public void ShowOptions() {
		this.HidePanels ();
        this.GetPanelOptions().gameObject.SetActive(true);
    }

	public GameObject GetButtonContainer() {
		if (this.pnlButtons == null) {
			this.pnlButtons = GetComponentInChildren<ButtonContainer> ().gameObject;
		}
		return this.pnlButtons;
	}

	public GameObject GetPanelConfirmHome() {
//		if (this.pnlConfirmHome == null) {
//			this.pnlConfirmHome = GetComponentInChildren<ButtonContainer> ().gameObject;
//		}
		return this.pnlConfirmHome;
	}

	public GameObject GetPanelConfirmPlay() {
//		if (this.pnlConfirmPlay == null) {
//			this.pnlConfirmPlay = GetComponentInChildren<ButtonContainer> ().gameObject;
//		}
		return this.pnlConfirmPlay;
	}

	public GameObject GetPanelConfirmRetry() {
//		if (this.pnlConfirmRetry == null) {
//			this.pnlConfirmRetry = GetComponentInChildren<ButtonContainer> ().gameObject;
//		}
		return this.pnlConfirmRetry;
	}

	public GameObject GetPanelOptions() {
		return this.pnlOptions;
	}

}
