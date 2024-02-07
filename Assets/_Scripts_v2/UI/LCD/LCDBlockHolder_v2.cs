using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDBlockHolder_v2 : MonoBehaviour {

	[SerializeField] private LCDBlock_v2 lcdBlockPrefab;
	private List<LCDBlock_v2> lcdBlocks;
	private int number = 0;
	[SerializeField] private float maxWidth;
	[SerializeField] private RectTransform maxLength;
	[SerializeField] private ParticleEffect particleEffectDecrease;
	private int currentIndex = 0;

	void Awake() {
		this.lcdBlocks = new List<LCDBlock_v2> ();
	}

	public List<LCDBlock_v2> GetLCDBlocks(){
		if(this.lcdBlocks == null) {
			this.lcdBlocks = new List<LCDBlock_v2> ();
		}
		return this.lcdBlocks;
	}
	public void PurgeBlocks() {
		foreach (LCDBlock_v2 lcdBlock in GetLCDBlocks())
			Destroy (lcdBlock.gameObject);

		lcdBlocks.Clear ();
	}

	public void SetParticleEffectDecrease(ParticleEffect effect) {
		this.particleEffectDecrease = effect;
	}
	public ParticleEffect GetParticleEffectDecrease() {
		return this.particleEffectDecrease;
	}
	public void SetInitialNumber (int number) {
		this.number = number;
	}

	public void SetBlockCount (int count, Transform holder) {
		PurgeBlocks ();

		this.currentIndex = 0;
		int currentNumber = this.number;

		this.maxLength = GameObject.FindObjectOfType<LCDMaxLength> ().GetComponent<RectTransform> ();
		this.maxWidth = maxLength.rect.width;
		Debug.Log ("<color=red>MAX WIDTH IS"+this.maxWidth+"</color>");



		float width = Mathf.Floor(this.maxWidth / count);
		Debug.Log ("<color=red>maxWidth IS"+maxWidth+"</color>");
		Debug.Log ("<color=red>count IS"+count+"</color>");
		Debug.Log ("<color=red>WIDTH IS"+width+"</color>");
		Vector2 blockPos = new Vector2 ();

		for (int i = 0; i < count; i++) {
			LCDBlock_v2 newBlock = Instantiate<LCDBlock_v2> (lcdBlockPrefab, this.gameObject.transform);
			newBlock.SetNumber (currentNumber);

			currentNumber += this.number;

			newBlock.SetSpriteWidth (width);
			newBlock.GetCollider2D ().enabled = false;

			float blockBounds = newBlock.GetSpriteWidth ();
			if (i == 0) {
				blockPos.x = holder.GetComponent<RectTransform>().rect.center.x+(width/2);
//				blockPos.x += blockBounds / 2;
				newBlock.UnhighlightSprite ();
				newBlock.ShowOutline ();
			}
			else {
				newBlock.gameObject.SetActive (false);
				newBlock.HighlightSprite ();
				newBlock.HideOutline ();
			}

			newBlock.transform.localPosition = blockPos;
			blockPos.x += blockBounds;

			lcdBlocks.Add (newBlock);
		}
	}

	public int IncreaseBlock(int stepCount) {
		if (lcdBlocks.Count > 0 && stepCount > 0) {
			stepCount--;

			this.lcdBlocks [currentIndex].HideOutline ();

			currentIndex++;
			if (currentIndex >= lcdBlocks.Count)
				currentIndex = lcdBlocks.Count - 1;
			this.lcdBlocks [currentIndex].gameObject.SetActive (true);
			this.lcdBlocks [currentIndex].ShowOutline ();
		}

		return stepCount;
	}

	public int DecreaseBlock(int stepCount) {
		if (lcdBlocks.Count > 0) {
			if (currentIndex > 0) {
				this.lcdBlocks [currentIndex].gameObject.SetActive (false);

				this.lcdBlocks [currentIndex].HideOutline ();

//				this.particleEffectDecrease.Play ();
				currentIndex--;
				stepCount++;
			}
			if (currentIndex < 0)
				currentIndex = 0;


			this.lcdBlocks [currentIndex].ShowOutline ();
		}

		return stepCount;
	}

	public int GetLatestDenominator() {
		if (lcdBlocks.Count > 0)
			return this.lcdBlocks [currentIndex].GetNumber ();
		else
			return 0;
	}
}
