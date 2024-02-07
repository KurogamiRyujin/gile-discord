using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NEED TO FINISH DOCUMENTATION

/// <summary>
/// Image UI element behaviour.
/// </summary>
public class ImageUI : MonoBehaviour {
	
    /// <summary>
    /// Static instance to the Image UI.
    /// </summary>
	static ImageUI instance;
    /// <summary>
    /// Access to the image UI static instance.
    /// </summary>
	public static ImageUI Instance {
		get { return instance; }
	}
    
	[SerializeField] private DialogueVisible[] visibles;

	[SerializeField] private List<Image> dialogueImages;
	private Dictionary<string, Image> imagePairs;

	void Awake () {
		GameController_v7.Instance.GetImageManager().SubscribeImageUI(this);
	}


	void Start () {
		this.visibles = GetComponentsInChildren<DialogueVisible>();
		GenerateImagePairs ();
	}

	void GenerateImagePairs() {
		this.imagePairs = new Dictionary<string, Image>();
		foreach (DialogueVisible visible in visibles) {
			imagePairs.Add (visible.GetID(), visible.GetImage());
		}
	}

	public Image GetImage(string id) {
		if (this.imagePairs == null) {
			this.GenerateImagePairs ();
		}
		if (this.imagePairs.ContainsKey (id))
			return this.imagePairs [id];
		else {
			return null;
		}
	}
}
