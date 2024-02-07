using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skin package. Used on a ghost block (formerly hollow block) and a highlight bar to be used on the stability number line to associate the block with the portion.
/// </summary>
public class SkinPackage {
	/// <summary>
	/// Skin material used when block is tangible.
	/// </summary>
	private Material tangibleSkin;
	/// <summary>
	/// Skin material used when the block is focused.
	/// </summary>
	private Material focusSkin;
	/// <summary>
	/// Skin material used when the block is intangible.
	/// </summary>
	private Material intangibleSkin;
	/// <summary>
	/// Skin material used when the block is unliftable.
	/// </summary>
	private Material unliftableSkin;
	/// <summary>
	/// Highlight color used for the block's bar in the stability number line.
	/// </summary>
	private Color highlightColor;
	/// <summary>
	/// Starting color of the block's bar in the stability number line.
	/// </summary>
	private Color outlineStartColor;
	/// <summary>
	/// Ending color of the block's bar in the stability number line.
	/// </summary>
	private Color outlineEndColor;

	/// <summary>
	/// Initializes a new instance of the <see cref="SkinPackage"/> class.
	/// </summary>
	/// <param name="package">Package.</param>
	public SkinPackage (SkinPackage package) {
		this.tangibleSkin = new Material (package.GetTangibleSkin ());
		this.focusSkin = new Material (package.GetFocusSkin ());
		this.intangibleSkin = new Material (package.GetIntangibleSkin ());
		this.unliftableSkin = new Material (package.GetUnliftableSkin ());
		this.highlightColor = package.GetHighlightColor ();
		this.outlineStartColor = package.GetOutlineStartColor ();
		this.outlineEndColor = package.GetOutlineEndColor ();
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="SkinPackage"/> class.
	/// </summary>
	/// <param name="tangible">Tangible.</param>
	/// <param name="focus">Focus.</param>
	/// <param name="intangible">Intangible.</param>
	/// <param name="unliftable">Unliftable.</param>
	/// <param name="highlight">Highlight.</param>
	/// <param name="outlineStart">Outline start.</param>
	/// <param name="outlineEnd">Outline end.</param>
	public SkinPackage (Material tangible, Material focus, Material intangible, Material unliftable, Color highlight,
		Color outlineStart, Color outlineEnd) {
		this.tangibleSkin = tangible;
		this.focusSkin = focus;
		this.intangibleSkin = intangible;
		this.unliftableSkin = unliftable;
		this.highlightColor = highlight;
		this.outlineStartColor = outlineStart;
		this.outlineEndColor = outlineEnd;
	}

	/// <summary>
	/// Gets the tangible skin.
	/// </summary>
	/// <returns>The tangible skin.</returns>
	public Material GetTangibleSkin() {
		return this.tangibleSkin;
	}

	/// <summary>
	/// Gets the focus skin.
	/// </summary>
	/// <returns>The focus skin.</returns>
	public Material GetFocusSkin() {
		return this.focusSkin;
	}

	/// <summary>
	/// Gets the intangible skin.
	/// </summary>
	/// <returns>The intangible skin.</returns>
	public Material GetIntangibleSkin() {
		return this.intangibleSkin;
	}

	/// <summary>
	/// Gets the unliftable skin.
	/// </summary>
	/// <returns>The unliftable skin.</returns>
	public Material GetUnliftableSkin() {
		return this.unliftableSkin;
	}

	/// <summary>
	/// Gets the color of the highlight.
	/// </summary>
	/// <returns>The highlight color.</returns>
	public Color GetHighlightColor() {
		return this.highlightColor;
	}

	/// <summary>
	/// Gets the color of the outline start.
	/// </summary>
	/// <returns>The outline start color.</returns>
	public Color GetOutlineStartColor() {
		return this.outlineStartColor;
	}

	/// <summary>
	/// Gets the color of the outline end.
	/// </summary>
	/// <returns>The outline end color.</returns>
	public Color GetOutlineEndColor() {
		return this.outlineEndColor;
	}
}
