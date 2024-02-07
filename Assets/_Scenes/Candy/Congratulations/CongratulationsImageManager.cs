using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Manages the UI elements displaying the notification of game completion.
/// </summary>
public class CongratulationsImageManager : MonoBehaviour {
    /// <summary>
    /// List of images to be displayed.
    /// </summary>
    [SerializeField] private List<CongratulationsImage> imageList;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
    private void Awake() {
        this.imageList = GetComponentsInChildren<CongratulationsImage>().ToList();
    }

    /// <summary>
    /// Hide all displayed UI elements.
    /// </summary>
    public void HideAllSkins() {
        foreach (CongratulationsImage image in this.GetImageList()) {
            image.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Load skin and use.
    /// </summary>
    /// <param name="skinType"></param>
    public void LoadSkin(GameController_v7.PlayerSkin skinType) {
        this.HideAllSkins();
        this.UseSkin(skinType);

    }

    /// <summary>
    /// Displays the loaded skin onto a UI element.
    /// </summary>
    /// <param name="skinType">Skin</param>
    public void UseSkin(GameController_v7.PlayerSkin skinType) {
        bool hasEntered = false;
       foreach(CongratulationsImage image in this.GetImageList()) {
            if (image.GetSkin() == skinType) {
                hasEntered = true;
                image.gameObject.SetActive(true);
            }
        }
        if (!hasEntered) {
            if (this.GetImageList() != null && this.GetImageList().Count > 0) {
                this.GetImageList()[0].gameObject.SetActive(true);
            }
        }
    
    }
        public List<CongratulationsImage> GetImageList() {
        if (this.imageList == null) {
            this.imageList = GetComponentsInChildren<CongratulationsImage>().ToList();
        }
        return this.imageList;
    }
}
