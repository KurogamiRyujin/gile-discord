using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintColorPressed : MonoBehaviour {
	[SerializeField] Image image;
	void Awake() {
		this.image = GetComponent<Image> ();
		this.Close ();
	}
	public void Open() {
		image.color = new Color (0.55f, 1.0f, 0.49f, 1.0f);
	}
	public void Close() {
		image.color = new Color (0.55f, 1.0f, 0.49f, 0.0f);
	}
}
