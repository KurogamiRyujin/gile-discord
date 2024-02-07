using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUI : MonoBehaviour {
	
	public void Show() {
		SoundManager.Instance.Play (AudibleNames.Room.ITEM_GET, false);
		gameObject.SetActive (true);
	}

	public void Close() {
		gameObject.SetActive (false);
	}
}
