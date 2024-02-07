using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cutscene graphic behaviour for CG objects.
/// </summary>
public class CutsceneGraphic : MonoBehaviour {

//	private Image cutsceneGraphic;
    /// <summary>
    /// Transform position where text will be located.
    /// </summary>
	private GameObject textPlacement;
//	[SerializeField] private List<Sprite> cutsceneGraphics;
    /// <summary>
    /// List of text placements.
    /// </summary>
	[SerializeField] private List<GameObject> textPlacements;
    /// <summary>
    /// Index of the current text placement where an element will be displayed.
    /// </summary>
	private int index;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
//		this.cutsceneGraphic = GetComponent<Image> ();

		GameController_v7.Instance.GetDialogueManager ().Register (this);
		this.index = 0;
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		GameController_v7.Instance.GetDialogueManager ().UnregisterCG (this);
	}

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
		HideCG ();
	}

    //	public void SetCG(Sprite cg) {
    //		this.cutsceneGraphic.sprite = cg;
    //	}

    /// <summary>
    /// Sets the element to be displayed based on the current index.
    /// </summary>
    /// <param name="index"></param>
    public void SetCG(int index) {
		if (this.index >= 0 && this.index < this.textPlacements.Count) {
			this.InstantiateTextPlacement ();

			this.index+=1;
		}
	}

    /// <summary>
    /// Destroys the current placement and CG with it.
    /// </summary>
	public void DestroyTextPlacement() {
		if (this.textPlacement != null) {
			Destroy (textPlacement);
		}
	}

    /// <summary>
    /// Creates a new placement and the CG on it.
    /// </summary>
	public void InstantiateTextPlacement() {
		this.DestroyTextPlacement ();
		if (this.index >= 0 && this.index < this.textPlacements.Count) {
			this.textPlacement = Instantiate (textPlacements[this.index], textPlacements[this.index].transform.position, Quaternion.identity);
			this.textPlacement.gameObject.transform.SetParent (gameObject.transform);
			Debug.Log ("ENTERED INSTANTIATE");
			RectTransform rectTransform = this.textPlacement.GetComponent<RectTransform> ();
			if (rectTransform != null) {
				//				rectTransform.anchoredPosition = Vector3.zero;
				//				rectTransform.anchoredPosition = Vector2.zero; 
				rectTransform.anchoredPosition3D = Vector3.zero;
				rectTransform.offsetMax = Vector2.zero;
				rectTransform.offsetMin = Vector2.zero;
				//				rectTransform.anchorMax = Vector2.zero;
				//				rectTransform.anchorMin = Vector2.zero;
				//				rectTransform.sizeDelta = Vector2.one;
				rectTransform.localScale = new Vector3 (1f, 1f, 1f);
			}
		}
//		if (this.index >= 0 && this.index < this.textPlacements.Count) {
//			this.textPlacement = Instantiate (textPlacements[this.index], textPlacements[this.index].transform.position, Quaternion.identity);
//			this.textPlacement.gameObject.transform.SetParent (gameObject.transform);
//			Debug.Log ("ENTERED INSTANTIATE");
//			RectTransform rectTransform = this.textPlacement.GetComponent<RectTransform> ();
//			if (rectTransform != null) {
////				rectTransform.anchoredPosition = Vector3.zero;
////				rectTransform.anchoredPosition = Vector2.zero; 
//				rectTransform.anchoredPosition3D = Vector3.zero;
//				rectTransform.offsetMax = Vector2.zero;
//				rectTransform.offsetMin = Vector2.zero;
////				rectTransform.anchorMax = Vector2.zero;
////				rectTransform.anchorMin = Vector2.zero;
////				rectTransform.sizeDelta = Vector2.one;
//				rectTransform.localScale = new Vector3 (1f, 1f, 1f);
//			}
//		}
	}

    /// <summary>
    /// Shows the next CG to be displayed.
    /// </summary>
	public void ShowCG() {
		this.SetCG (this.index);
//		Color color = new Color (this.cutsceneGraphic.color.r, this.cutsceneGraphic.color.g, this.cutsceneGraphic.color.b, 255.0f);
//		this.cutsceneGraphic.color = color;
//		this.index += 1;
	}

    /// <summary>
    /// Hides the current CG.
    /// </summary>
	public void HideCG() {
		this.DestroyTextPlacement ();
//		Color color = new Color (this.cutsceneGraphic.color.r, this.cutsceneGraphic.color.g, this.cutsceneGraphic.color.b, 0.0f);
//		this.cutsceneGraphic.color = color;
	}
}
