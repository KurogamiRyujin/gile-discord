using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
	public Dialogue dialogue;

	[SerializeField] private DialogueManager dialogueManager;


	void Start() {
		dialogueManager = GameObject.FindObjectOfType<DialogueManager> ();
//		if (dialogueManager == null) {
//			dialogueManager = GameObject.FindGameObjectWithTag ("DialogueManager").GetComponent<DialogueManager> ();
//		}
	}

	public void TriggerDialogue() {
		TextAsset textFile = dialogue.getTextFile ();
		if (textFile != null) {
			List<string> text = new List<string>(textFile.text.Split ('\n'));
			dialogue.name = text [0];
			text.RemoveAt (0);
			dialogue.sentences = text.ToArray();
		}
//		this.dialogueManager.StartDialogue (dialogue);
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogue, null);

	}

	public DialogueManager GetDialogueManager() { 
		return this.dialogueManager;
	}
}
