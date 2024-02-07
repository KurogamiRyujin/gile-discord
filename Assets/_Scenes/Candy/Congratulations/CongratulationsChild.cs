using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Element of the UI displaying the notification of game completion.
/// </summary>
public class CongratulationsChild : MonoBehaviour {
    /// <summary>
    /// Reference to an image manager.
    /// </summary>
    private CongratulationsImageManager imageManager;

    // Use this for initialization
    /// <summary>
    /// Standard Unity Function. Called once in the component's lifetime to jumpstart behaviour.
    /// </summary>
    void Start () {
		
	}
	
	// Update is called once per frame
    /// <summary>
    /// Standard Unity Function. Called every frame.
    /// </summary>
	void Update () {
		
	}

    /// <summary>
    /// Load skin into the image manager.
    /// </summary>
    public void LoadSkin() {
        if (this.GetImageManager() != null) {
            this.GetImageManager().LoadSkin(GameController_v7.Instance.GetPlayerSkin());
        }
    }

    /// <summary>
    /// Returns the image manager.
    /// </summary>
    /// <returns>The Image Manager.</returns>
    public CongratulationsImageManager GetImageManager() {
        if (this.imageManager == null) {
            this.imageManager = GetComponentInChildren<CongratulationsImageManager>();
        }
        return this.imageManager;
    }
}
