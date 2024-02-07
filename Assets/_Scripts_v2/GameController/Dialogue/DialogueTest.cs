using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to test the functionality of the dialogue box.
/// </summary>
public class DialogueTest : MonoBehaviour {

	public Dialogue dialogue1;
	public string[] dialogue2;
	
	// Update is called once per frame
	void Update () {
		bool dialogue1Trigger = Input.GetKeyDown (KeyCode.N);
		bool dialogue2Trigger = Input.GetKeyDown (KeyCode.M);

		if (dialogue1Trigger)
			GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogue1, null);
		if (dialogue2Trigger)
			GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogue2, null);
	}
}
