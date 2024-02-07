using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFragmentPiece : MonoBehaviour {
	//NOTE: Specialized class for the tutorial. NOT MEANT TO BE USED IN OTHER SCENES.
	[SerializeField] private SkyFragmentPiece pieceReference;
	[SerializeField] private SkyBlock skyBlockReference;
	[SerializeField] private HollowBlockSkyPieceContainer hollowBlockSkyPieceContainer;
	[SerializeField] private int numValue, denValue;

	private SkyFragmentPiece skyFragmentPiece;

	void Start() {
		this.skyFragmentPiece = CreateSkyFragmentPiece ();
		for (int i = 0; i < 3; i++) {
			this.hollowBlockSkyPieceContainer.Absorb (CreateSkyFragmentPiece ());
		}
	}

	public SkyFragmentPiece CreateSkyFragmentPiece() {
		SkyFragmentPiece holder = SkyFragmentPiece.Instantiate (pieceReference, this.transform.position, Quaternion.identity);
		holder.gameObject.transform.SetParent (this.transform);
		holder.AlignToLocal (Vector3.zero);
		holder.Initialize (this.skyBlockReference, numValue, denValue);
		holder.ChangeColor (this.skyBlockReference.GetPieceColor (), this.skyBlockReference.GetPieceOutlineColor ());

		holder.gameObject.SetActive(true);

		return holder;
	}
}
