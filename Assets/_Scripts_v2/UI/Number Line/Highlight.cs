using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

//	private Transform followPos;
	private LineRenderer highlight;
	[SerializeField] float numerator;
	[SerializeField] float denominator;
	[SerializeField] float length;

	[SerializeField] float endPoint;
	[SerializeField] float startPoint;
    [SerializeField] HollowBlock hollowBlock;
    //	[SerializeField] Color color;

    // Use this for initialization
    void Awake () {
		this.highlight = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
//	void Update () {
//		this.highlight.SetPosition (0, this.transform.position);
//		if (followPos != null)
//			this.highlight.SetPosition (this.highlight.positionCount - 1, followPos.position);
//		else
//			this.highlight.SetPosition (this.highlight.positionCount - 1, this.transform.position);
//	
//		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, 0f, 0f);
//	}

//	public void SetFollowPos(Transform followPos) {
//		this.followPos = followPos;
//	}

	public bool IsMatch(float testNumerator, float testDenominator) {
		if (this.numerator == testNumerator &&
		    this.denominator == testDenominator) {
			return true;
		} else {
			return false;
		}
	}
	public bool IsMatch(float testNumerator, float testDenominator, Color testColor) {
		// TODO consider color
		float r = General.Round (this.highlight.startColor.r, 2);
		float g = General.Round (this.highlight.startColor.g, 2);
		float b = General.Round (this.highlight.startColor.b, 2);


        float testR = General.Round(testColor.r, 2);
        float testG = General.Round(testColor.g, 2);
        float testB = General.Round(testColor.b, 2);

        //Debug.Log("<color=red>" + r + "_____" + testColor.r + "</color>");


        if (this.numerator == testNumerator &&
			this.denominator == testDenominator && //) {
			r == testR &&
			g == testG &&
			b == testB) {
			return true;
		} else {
			return false;
		}
	}
    public void SetHollowBlock(HollowBlock newHollowBlock) {
        this.hollowBlock = newHollowBlock;
    }
	public float GetNumerator() {
		return this.numerator;
	}

	public float GetDenominator() {
		if (this.denominator == 0)
			this.denominator = 1;
		return this.denominator;
	}

	public void SetLineLength(float start, float end) {
		this.GetLineRenderer ().SetPosition(0, new Vector3(start, 0f, 0f));
		this.GetLineRenderer ().SetPosition(1, new Vector3(end, 0f, 0f));
	}

	// Null check is just a safety measure, it should not enter it
	public LineRenderer GetLineRenderer() {
		if (this.highlight == null) {
			this.highlight = GetComponent<LineRenderer> ();
		}
		return this.highlight;
	}

	public float GetStart() {
		return highlight.GetPosition (0).x;
	}

	public float GetEnd() {
		return highlight.GetPosition (1).x;
	}

	public void SetNumerator(float value) {
		this.numerator = value;
	}

	public void SetDenominator(float value) {
		if (value == 0) {
			value = 1;
		}
		this.denominator = value;
	}

	public void SetLength(float value) {
		this.length = value;
	}

	public void SetColor(Color color) {
		this.highlight.startColor = color;
		this.highlight.endColor = color;
	}
	public Color GetColor() {
		return this.highlight.startColor;
	}
//	public Vector3 GetHighlightEndPointPos() {
//		return this.highlight.GetPosition (this.highlight.positionCount - 1);
//	}
}
