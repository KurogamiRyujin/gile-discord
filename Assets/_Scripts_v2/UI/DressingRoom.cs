using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DressingRoom : MonoBehaviour {


    [SerializeField] DressingRoomParent dressingRoomParent;

	// Use this for initialization
	void Start () {
        this.GetDressingRoomParent().gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSkinClick() {
    }

    public DressingRoomParent GetDressingRoomParent() {
        if (this.dressingRoomParent == null) {
            this.dressingRoomParent = GetComponentInChildren<DressingRoomParent>();
        }
        return this.dressingRoomParent;
    }
}
