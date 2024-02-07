using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDBlockHolder : MonoBehaviour {

	[SerializeField] private LCDBlock lcdBlockPrefab;
	private List<LCDBlock> lcdBlocks;
	[SerializeField] private float maxWidth = 10.0f;

	void Awake() {
		this.lcdBlocks = new List<LCDBlock> ();
	}

	private void PurgeBlocks() {
		foreach (LCDBlock lcdBlock in this.lcdBlocks)
			Destroy (lcdBlock.gameObject);

		lcdBlocks.Clear ();
	}

	public void SetBlockCount (int count) {
		PurgeBlocks ();

		float width = maxWidth / count;
		Vector2 blockPos = new Vector2 ();

		for (int i = 0; i < count; i++) {
			LCDBlock newBlock = Instantiate<LCDBlock> (lcdBlockPrefab, this.gameObject.transform);
			newBlock.SetSpriteXScale (width);

			Bounds blockBounds = newBlock.GetSpriteBounds ();
			if (i == 0)
				blockPos.x += blockBounds.size.x / 2;

			newBlock.transform.localPosition = blockPos;
			blockPos.x += blockBounds.size.x;

			lcdBlocks.Add (newBlock);
		}
	}
}
