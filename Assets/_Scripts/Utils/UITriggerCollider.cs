using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITriggerCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private PlayerAttack playerAttack;

	void Start() {
//		playerAttack = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();
		playerAttack = GameObject.FindObjectOfType<PlayerYuni>().GetPlayerAttack();
	
	}


	public void OnPointerEnter(PointerEventData eventData) {
		Debug.Log ("Testing Pointer Entered: True");
		playerAttack.InDialogue (true);
	}

	public void OnPointerExit(PointerEventData eventData) {
		Debug.Log ("Testing Pointer Entered: False");
		playerAttack.InDialogue (false);
	}
}
