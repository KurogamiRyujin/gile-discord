using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeFractionLabelLayer : MonoBehaviour {

	[SerializeField] protected SortingGroup sortingGroup;

	void Awake() {
		this.sortingGroup = GetComponent<SortingGroup> ();
		GameController_v7.Instance.GetPauseController ().onPauseGame += this.OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += this.OnContinue;
	}
	void Destroy() {
		GameController_v7.Instance.GetPauseController ().onPauseGame -= this.OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= this.OnContinue;
	}

	public void OnPause() {
		this.sortingGroup.sortingLayerName = "BrokenLabel";
	}

	public void OnContinue() {
		this.sortingGroup.sortingLayerName = "Below Player";
	}
}
