using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DressingRoomLeftContainer : MonoBehaviour {

    [SerializeField] TextMeshProUGUI skinName;
    [SerializeField] TextMeshProUGUI username;

    // Use this for initialization
    void Awake () {
        EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_SKIN_NAME, UpdateSkinName);
	}

    void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.UPDATE_SKIN_NAME);
    }

    void Start() {
        UpdateUsername(); 
    }

    // Update is called once per frame
    void Update () {
		
	}

    void UpdateUsername() {
        if (PlayerPrefs.HasKey("Username"))
            username.text = PlayerPrefs.GetString("Username");
    }

    void UpdateSkinName(Parameters parameters) {
        string name = parameters.GetStringExtra("SKIN_NAME", "");
        skinName.text = name;
    }


}
