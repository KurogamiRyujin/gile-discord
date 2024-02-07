using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinContainer : MonoBehaviour {

    [SerializeField] Skin[] skins;

	// Use this for initialization
	void Awake () {
        EventBroadcaster.Instance.AddObserver(EventNames.HIDE_SKINS, HideAll);
	}

    void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.HIDE_SKINS);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void HideAll() {
        foreach(Skin skin in skins) {
            skin.Hide();
        }
    }
}
