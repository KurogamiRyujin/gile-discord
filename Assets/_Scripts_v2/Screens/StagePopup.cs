using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePopup: MonoBehaviour {

	[SerializeField] Animator animator;
	[SerializeField] SelectSceneScreen selectSceneScreen;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void OpenMenu() {
		animator.SetBool ("isOpen", true);
	}

	public void CloseMenu() {
		animator.SetBool ("isOpen", false);
	}

	public void OnPanelClick() {

	}
}
