using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visible : MonoBehaviour {

	[SerializeField] protected Image image;

	public Image GetImage() {
		return this.image;
	}
}