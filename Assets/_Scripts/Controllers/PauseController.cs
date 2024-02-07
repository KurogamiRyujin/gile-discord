using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

	[SerializeField] private GameObject pausePanel;
	public static bool isPaused;
	public KeyCode RESUME = KeyCode.Return;
	[SerializeField] private GameObject pauseOverlay;

	void Start() {
		pausePanel.SetActive(false);
		this.pauseOverlay = GameObject.FindGameObjectWithTag ("PauseOverlay");
		this.pauseOverlay.SetActive(false);
	}

	void Update() {
		/*
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!pausePanel.activeInHierarchy) {
				PauseGame ();
			} else if (pausePanel.activeInHierarchy) {
				ContinueGame ();   
			}
		}
		*/
	}

	public void PauseGame() {
		Time.timeScale = 0;
		pausePanel.SetActive(true);
		isPaused = true;
		this.pauseOverlay.SetActive(true);
		//Disable scripts that still work while timescale is set to 0
	} 


	public void ShowOverlay() {
		this.pauseOverlay.SetActive(true);
	}

	public void HideOverlay() {
		this.pauseOverlay.SetActive(false);
	}

	public void ContinueGame() {
		Time.timeScale = 1;
		isPaused = false;
		this.HideOverlay ();
		//enable the scripts again
	}

	public void ContinueGame(bool hasOverlay) {
		this.ContinueGame ();
		this.pauseOverlay.SetActive (hasOverlay);
		//enable the scripts again
	}
}
