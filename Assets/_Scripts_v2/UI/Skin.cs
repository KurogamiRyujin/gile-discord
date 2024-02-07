using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skin : MonoBehaviour {

    string skinName;

	// Use this for initialization
	void Start () {
        skinName = GetComponentInChildren<TextMeshProUGUI>().text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hide() {
        gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public void Show() {
        EventBroadcaster.Instance.PostEvent(EventNames.HIDE_SKINS);
        gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        Parameters parameters = new Parameters();
        parameters.PutExtra("SKIN_NAME", skinName);
        EventBroadcaster.Instance.PostEvent(EventNames.UPDATE_SKIN_NAME, parameters);
    }
}
