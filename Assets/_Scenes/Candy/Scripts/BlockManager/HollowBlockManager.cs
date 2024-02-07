using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hollow block manager. Handles behaviour of the ghost blocks (formerly hollow blocks) (like how their colors change) whenever their states are changed.
/// </summary>
public class HollowBlockManager : MonoBehaviour {
	// Skins and highlights must be of equal length
	/// <summary>
	/// Skin materials used by the hollow blocks.
	/// </summary>
	[SerializeField] private List<Material> skins;
	/// <summary>
	/// Skin material used when the hollow block is focused by the player character by going near it.
	/// </summary>
	[SerializeField] private Material focusSkin;
	/// <summary>
	/// Used when the hollow block is intangible/unfilled.
	/// </summary>
	[SerializeField] private Material intangibleSkin;
	/// <summary>
	/// The unliftable skin.
	/// </summary>
	[SerializeField] private Material unliftableSkin;
	/// <summary>
	/// List of color highlights. Used to color the highlights on the stability number line that associates the block's color with a portion in the stability number line.
	/// </summary>
	[SerializeField] private List<Color> highlights;

	/// <summary>
	/// Colors used at the point in the stability number line where a ghost block's portion in the stability number line begins.
	/// </summary>
	[SerializeField] private List<Color> outlineStartColors;
	/// <summary>
	/// Colors used at the point in the stability number line where a ghost block's portion in the stability number line ends.
	/// </summary>
	[SerializeField] private List<Color> outlineEndColors;

	// Get skins from this list
	/// <summary>
	/// The skin packages.
	/// </summary>
	private List<SkinPackage> skinPackage;
	/// <summary>
	/// Index to the currently used skin in the skin package list.
	/// </summary>
	private int skinIndex;

	/// <summary>
	/// Lime Green Color.
	/// </summary>
	private Color limeGreen = new Color(0.65f, 1, 0.1f, 1);
	/// <summary>
	/// Violet Color.
	/// </summary>
	private Color violet = new Color(0.6f, 0.21f, 1, 1);
	/// <summary>
	/// Royal Blue Color.
	/// </summary>
	private Color royalBlue = new Color(0.1f, 0.48f, 1, 1);
	/// <summary>
	/// Orange Color.
	/// </summary>
	private Color orange = new Color(1, 0.49f, 0.1f, 1); 

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		GameController_v7.Instance.GetBlockManager ().SubscribeHollowBlockManager (this);
		this.skinIndex = -1;
		GenerateSkinPackage ();
		GetSkinPackage ().Shuffle (); // Shuffle the skin
	}

	/// <summary>
	/// Generates the skin packages.
	/// </summary>
	public void GenerateSkinPackage() {
		for (int i = 0; i < this.GetSkins ().Count; i++) {
			this.GetSkinPackage ().Add (
				new SkinPackage(this.GetSkins()[i], this.focusSkin, this.intangibleSkin, this.unliftableSkin, this.GetHighlights()[i],
					this.GetOutlineStartColors()[i], this.GetOutlineEndColors()[i]));
		}
	}

	/// <summary>
	/// Return a new instance of the current skin package
	/// </summary>
	/// <returns>The skin.</returns>
	public SkinPackage TakeSkin() {
		this.skinIndex++;
		if (this.skinIndex >= GetSkinPackage ().Count) {
			this.skinIndex = 0;
		}
		return (new SkinPackage(this.GetSkinPackage () [skinIndex]));
	}

	/// <summary>
	/// Gets the skin packages.
	/// </summary>
	/// <returns>The skin package.</returns>
	public List<SkinPackage> GetSkinPackage() {
		if (this.skinPackage == null) {
			this.skinPackage = new List<SkinPackage> ();
		}
		return this.skinPackage;
	}
	/// <summary>
	/// Gets the skins.
	/// </summary>
	/// <returns>The skins.</returns>
	public List<Material> GetSkins() {
		if (this.skins == null) {
			this.skins = new List<Material> ();
		}
		return this.skins;
	}
	/// <summary>
	/// Gets the stability number line highlight colors.
	/// </summary>
	/// <returns>The highlights.</returns>
	public List<Color> GetHighlights() {
		if (this.highlights == null) {
			this.highlights = new List<Color> ();
		}
		return this.highlights;
	}
	/// <summary>
	/// Gets stability number line starting outline colors.
	/// </summary>
	/// <returns>The outline start colors.</returns>
	public List<Color> GetOutlineStartColors() {
		if (this.outlineStartColors == null) {
			this.outlineStartColors = new List<Color> ();
		}
		return this.outlineStartColors;
	}
	/// <summary>
	/// Gets stability number line ending outline colors.
	/// </summary>
	/// <returns>The outline ending colors.</returns>
	public List<Color> GetOutlineEndColors() {
		if (this.outlineEndColors == null) {
			this.outlineEndColors = new List<Color> ();
		}
		return this.outlineEndColors;
	}
}
