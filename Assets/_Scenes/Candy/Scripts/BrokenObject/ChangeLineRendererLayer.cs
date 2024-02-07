using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLineRendererLayer : MonoBehaviour {

	[SerializeField] protected LineRenderer lineRenderer;

	void Awake() {
		this.lineRenderer = GetComponent<LineRenderer> ();
		GameController_v7.Instance.GetPauseController ().onPauseGame += this.OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += this.OnContinue;
	}
	void Destroy() {
		GameController_v7.Instance.GetPauseController ().onPauseGame -= this.OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= this.OnContinue;
	}

	public void OnPause() {
		this.lineRenderer.sortingLayerName = "BrokenClone";
	}

	public void OnContinue() {
		this.lineRenderer.sortingLayerName = "Below Player";
	}
}
