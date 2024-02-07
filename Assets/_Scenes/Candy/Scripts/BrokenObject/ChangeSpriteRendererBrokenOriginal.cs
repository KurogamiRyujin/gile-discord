using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteRendererBrokenOriginal : MonoBehaviour {

	[SerializeField] protected LineRenderer lineRenderer;
	[SerializeField] protected SpriteRenderer spriteRenderer;

	void Awake() {
		this.lineRenderer = GetComponent<LineRenderer> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		GameController_v7.Instance.GetPauseController ().onPauseGame += this.OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += this.OnContinue;
	}
	void Destroy() {
		GameController_v7.Instance.GetPauseController ().onPauseGame -= this.OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= this.OnContinue;
	}

	public void OnPause() {
		this.lineRenderer.sortingLayerName = "BrokenClone";
		this.spriteRenderer.sortingLayerName = "BrokenClone";
	}

	public void OnContinue() {
		this.lineRenderer.sortingLayerName = "Below Player";
		this.spriteRenderer.sortingLayerName = "Below Player";
	}
}
