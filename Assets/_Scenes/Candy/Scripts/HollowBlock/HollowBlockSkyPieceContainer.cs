using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HollowBlockSkyPieceContainer : MonoBehaviour {
	enum Alignment {
		RIGHT,
		LEFT
	}

	[SerializeField] private HollowBlock hollowBlock;
	[SerializeField] private HollowBlockHorizontalContainer container;
	[SerializeField] private Alignment childAlignment;
	//	[SerializeField] private List<SkyFragmentPiece> skyPieces;
	[SerializeField] private List<SkyFragmentPiece> skyPieces;


	private float stabilityNumerator = 1;
	private float stabilityDenominator = 1;

	void Awake() {
//		this.skyPieces = new List<SkyFragmentPiece> ();
		this.skyPieces = this.GetContainer ().GetComponentsInChildren<SkyFragmentPiece> ().ToList();
	}

	void Update() {
//		this.CheckAlignment ();
		this.UpdateSkyPieceList();
		this.UpdateCurrentStability ();
	}

	// Called on player death. Returns piece in initial position but remains detached
	public void Return() {
		foreach (SkyFragmentPiece piece in this.GetSkyPieces()) {
			piece.Return ();
		}
		UpdateSkyPieceList ();
		UpdateSkyPiecePositions ();
		UpdateCurrentStability ();
	}

	public void Absorb(SkyFragmentPiece skyPiece) {
		skyPiece.DisableRigidBody ();
		skyPiece.SetHollowBlock (this.GetHollowBlock());
		skyPiece.gameObject.transform.SetParent (this.GetContainer().gameObject.transform);
		skyPiece.SetPreviousParent (this.GetContainer().gameObject.transform);

		skyPiece.gameObject.transform.localPosition = Vector3.zero;
		skyPiece.gameObject.transform.eulerAngles = Vector3.zero;

		skyPiece.WearAttachedSkin ();

		UpdateSkyPieceList ();
		UpdateSkyPiecePositions ();
		UpdateCurrentStability ();
	}
	public void UpdateSkyPiecePositions() {
		if (this.skyPieces != null && this.skyPieces.Count > 0) {
			switch (this.childAlignment) {
			case Alignment.RIGHT:
				RightAlignChildren ();
				break;
			case Alignment.LEFT:
				LeftAlignChildren ();
				break;
			}
		}
	}
	public void Release() {
		foreach (SkyFragmentPiece piece in this.GetSkyPieces()) {
			piece.Release ();
		}
		EventBroadcaster.Instance.PostEvent (EventNames.SKY_FRAGMENT_PIECE_RELEASED);
		this.UpdateSkyPieceList ();
		this.UpdateCurrentStability ();
	}
	public void Break() {
		foreach (SkyFragmentPiece piece in this.GetSkyPieces()) {
			piece.Break ();
		}

		this.UpdateSkyPieceList ();
		this.UpdateCurrentStability ();
	}

	public void UpdateCurrentStability() {
		if (this.skyPieces != null && this.skyPieces.Count > 0) {
			float num1 = this.skyPieces [0].GetNumerator () * this.skyPieces [0].GetBlockSize ();
			float den1 = this.skyPieces [0].GetDenominator ();

			float num2;
			float den2;
			float[] results;
			for (int i = 1; i < this.skyPieces.Count; i++) {
				num2 = skyPieces [i].GetNumerator () * skyPieces [i].GetBlockSize ();
				den2 = skyPieces [i].GetDenominator ();

				results = General.AddFractions (num1, den1, num2, den2);
				num1 = results [0];
				den1 = results [1];
			}
			float[] simplifiedResults = General.SimplifyFraction (num1, den1);
			this.stabilityNumerator = simplifiedResults [0];
			this.stabilityDenominator = simplifiedResults [1];
		}
		else {
			this.stabilityNumerator = 0;
			this.stabilityDenominator = this.hollowBlock.GetDenominator ();
		}
	}

	public float GetStabilityNumerator() {
		return this.stabilityNumerator;
	}

	public float GetStabilityDenominator() {
		return this.stabilityDenominator;
	}

	public void LeftAlignChildren() {

		this.skyPieces [0].transform.localPosition =
			new Vector3 (-((this.skyPieces[0].GetWidthSingle()/2)-(this.GetHollowBlock().GetWidthPiece()/2)),
			0f,
			0f);

		if (this.skyPieces.Count > 1) {
			for (int i = 1; i < this.skyPieces.Count; i++) {
				this.skyPieces [i].transform.localPosition = 
					new Vector3 (
					this.skyPieces [i - 1].transform.localPosition.x +
						this.skyPieces [i - 1].GetWidthSingle (),
					0f,
					0f);
			}
		}
	}
	public void Show() {
		gameObject.SetActive (true);
		this.UpdateSkyPieceList ();
		this.UpdateCurrentStability ();
		foreach (SkyFragmentPiece piece in this.GetSkyPieces()) {
			piece.EnableCollider ();
		}
	}

	public void Hide() {
		foreach (SkyFragmentPiece piece in this.GetSkyPieces()) {
			piece.DisableCollider ();
		}
		gameObject.SetActive (false);
	}

	public void RightAlignChildren() {
		this.skyPieces [0].transform.localPosition =
			new Vector3 ((this.GetHollowBlock().GetWidthPiece()/2)-(this.skyPieces [0].GetWidthSingle()/2),
			0f,
			0f);

		if (this.skyPieces.Count > 1) {
			for (int i = 1; i < this.skyPieces.Count; i++) {
				this.skyPieces [i].transform.localPosition = 
					new Vector3 (
						this.skyPieces [i - 1].transform.localPosition.x -
						(this.skyPieces [i - 1].GetWidth()/2)-
						(this.skyPieces [i].GetWidthSingle ()/2),
						0f,
						0f);
			}
		}
	}

	public HollowBlock GetHollowBlock() {
		if (this.hollowBlock == null) {
			this.hollowBlock = GetComponentInParent<HollowBlock> ();
		}
		return this.hollowBlock;
	}

	public void UpdateSkyPieceList() {
		this.skyPieces = this.GetContainer ().GetComponentsInChildren<SkyFragmentPiece> ().ToList();
//		Debug.Log ("SKY PIECE COUNT IS "+this.skyPieces.Count);
	}

	public void CheckAlignment() {
		switch(this.childAlignment) {
		case Alignment.RIGHT:
			this.RightAlign ();
			break;
		case Alignment.LEFT:
			this.LeftAlign ();
			break;
		}
	}

	public void RightAlign() {
		float widthSum = 0;
		int count = skyPieces.Count - 1; // Exclude last element to properly align

		for(int i = 0; i < count; i++) {
			widthSum += this.skyPieces [i].GetComponent<RectTransform> ().rect.width;
		}

		this.ContainerRectTransform ().localPosition = new Vector3 (-widthSum, 0f, 0f);
	}

	public void LeftAlign() {
		this.ContainerRectTransform ().localPosition = Vector3.zero;
	}

	public HollowBlockHorizontalContainer GetContainer() {
		if (this.container == null) {
			this.container = GetComponentInChildren<HollowBlockHorizontalContainer> ();
		}
		return this.container;
	}

	public RectTransform ContainerRectTransform() {
		return this.GetContainer().GetComponent<RectTransform>();
	}

	public List<SkyFragmentPiece> GetSkyPieces() {
		return this.skyPieces;
	}
}
