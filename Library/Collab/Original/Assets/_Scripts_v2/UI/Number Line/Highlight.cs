using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

	private Transform followPos;
	private LineRenderer highlight;

	// Use this for initialization
	void Awake () {
		this.highlight = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.highlight.SetPosition (0, this.transform.position);
		if (followPos != null)
			this.highlight.SetPosition (this.highlight.positionCount - 1, followPos.position);
		else
			this.highlight.SetPosition (this.highlight.positionCount - 1, this.transform.position);
	}

	public void SetFollowPos(Transform followPos) {
		this.followPos = followPos;
	}

	public void SetColor(Color color) {
		this.highlight.endColor = color;
	}

	public Vector3 GetHighlightEndPointPos() {
		return this.highlight.GetPosition (this.highlight.positionCount - 1);
	}
}
