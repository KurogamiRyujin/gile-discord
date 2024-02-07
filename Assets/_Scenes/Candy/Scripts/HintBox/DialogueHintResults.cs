using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHintResults : MonoBehaviour {
	[SerializeField] private ResultsUI resultsUI;

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.PLAY_HINT_RESULTS, PlayResults);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.PLAY_HINT_RESULTS);
	}

	void PlayResults() {
		this.GetResultsUI ().PlaySuccess ();
	}


	public ResultsUI GetResultsUI() {
		if (this.resultsUI == null) {
			this.resultsUI = GetComponent<ResultsUI> ();
		}
		return this.resultsUI;
	}
}
