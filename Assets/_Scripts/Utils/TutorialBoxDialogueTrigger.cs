using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoxDialogueTrigger : AreaTrigger {

	private TutorialCutscene tutorialCutscene;

	void Start() {
		tutorialCutscene = GetComponentInParent<TutorialCutscene> ();
		targetTag = "Player";
	}

	protected override void Effect () {
		tutorialCutscene.ApproachedBox ();
	}
}
