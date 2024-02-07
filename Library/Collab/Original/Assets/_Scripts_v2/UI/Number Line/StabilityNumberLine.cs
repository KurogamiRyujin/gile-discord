using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityNumberLine : NumberLine {

	public const string IS_ADD = "add";
	public const string SUBTRACT = "subtract";
	public const string NUMERATOR = "numerator";
	public const string DENOMINATOR = "denominator";
	public const string COLOR = "color";

	[SerializeField] private int testDenom;
	[SerializeField] private int step = 2;

	[SerializeField] private int targetNumerator;
	[SerializeField] private int targetDenominator;

	[SerializeField] private bool isStable;
	[SerializeField] private float addNumerator;
	[SerializeField] private float addDenominator;
	[SerializeField] private bool callAdd;
	[SerializeField] private bool callSubtract;

	[SerializeField] private Highlight highlightPrefab;
	[SerializeField] private Transform highlightParent;
	[SerializeField] private RectTransform pointer;

	[SerializeField] private StabilityPointer stabilityPointer;
	[SerializeField] private TargetMarker targetMarker;


	[SerializeField] private bool isSubtracting;
	private List<Highlight> highlights;
	private Queue<Highlight> removalQueue;

	void Awake() {
		base.Awake();
		this.highlights = new List<Highlight> ();
		this.removalQueue = new Queue<Highlight> ();
		this.isStable = false;
		this.step = 5;
		EventBroadcaster.Instance.AddObserver (EventNames.ON_HOLLOW_STABILITY_UPDATE, HollowUpdate);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HOLLOW_STABILITY_UPDATE);
	}
	// Use this for initialization
	void Start () {
		this.Segment (testDenom);
		this.SetMaxPoint (testDenom);
//		this.AddMarker (testDenom, 2, Fraction.Notation.DECIMAL);

//		for (int i = 0; i < 3; i++) {
////			AddHighlight (pointer);
//			AddHighlight ();
//		}
		this.GetStabilityPointer ().SetLength (this.GetLineLength());
		this.GetTargetMarker ().PointTo (0, (float)this.testDenom);
//		this.GetStabilityPointer ().SetNumerator(0);
//		this.GetStabilityPointer ().SetDenominator((float)this.testDenom);
//		this.GetStabilityPointer ().UpdateLabel ();

		this.GetTargetMarker ().SetLength (this.GetLineLength());
		this.GetTargetMarker ().PointTo ((float)this.targetNumerator, (float)this.targetDenominator);
		this.StabilityCheck ();
	}

//	void Update() {
//		// Testing purposes only. TODO: remove
//		if (callAdd) {
//			this.callAdd = false;
//			this.callSubtract = false;
//			this.ManipulateStability (this.addNumerator, this.addDenominator, Random.ColorHSV, true);
//		} else if (callSubtract) {
//			this.callAdd = false;
//			this.callSubtract = false;
//			this.ManipulateStability (this.addNumerator, this.addDenominator, Random.ColorHSV, false);
//		}
//	}

	private void StabilityCheck () {
		int stabilityNum = this.stabilityPointer.GetFractionLabel ().GetNumerator ();
		int stabilityDenom = this.stabilityPointer.GetFractionLabel ().GetDenominator ();
		int targetNum = this.targetMarker.GetFractionLabel ().GetNumerator ();
		int targetDenom = this.targetMarker.GetFractionLabel ().GetDenominator ();

		if (stabilityNum / (float) stabilityDenom == targetNum / (float) targetDenom) {
			EventBroadcaster.Instance.PostEvent (EventNames.VULNERABLE_ENEMIES);
			EventBroadcaster.Instance.PostEvent (EventNames.STABLE_AREA);
			this.isStable = true;
		} else {
			if (this.isStable) {
				EventBroadcaster.Instance.PostEvent (EventNames.BREAK_AREA);
			}
			this.isStable = false;
			EventBroadcaster.Instance.PostEvent (EventNames.INVULNERABLE_ENEMIES);
			EventBroadcaster.Instance.PostEvent (EventNames.UNSTABLE_AREA);
		}
	}

	public void HollowUpdate(Parameters parameters) {
		float numerator = parameters.GetFloatExtra (NUMERATOR, 0f);
		float denominator = parameters.GetFloatExtra (DENOMINATOR, 1f);
		Color color = (Color) parameters.GetObjectExtra (COLOR);
		bool isAdd = parameters.GetBoolExtra (IS_ADD, true);
		this.ManipulateStability (numerator, denominator, color, isAdd);
		StabilityCheck ();
		
	}

	public float GetNumerator() {
		return this.GetStabilityPointer ().GetNumerator ();
	}

	public float GetDenominator() {
		return this.GetStabilityPointer ().GetDenominator ();
	}


	// Provides null check. Assign stabilityPointer in inspector but use this just to be safe
	public StabilityPointer GetStabilityPointer() {
		if (this.stabilityPointer == null) {
			this.stabilityPointer = GetComponentInChildren<StabilityPointer> ();
		}
		return this.stabilityPointer;
	}

	// Provides null check. Assign targetMarker in inspector but use this just to be safe
	public TargetMarker GetTargetMarker() {
		if (this.targetMarker == null) {
			this.targetMarker = GetComponentInChildren<TargetMarker> ();
		}
		return this.targetMarker;
	}

	public void ManipulateStability(float numerator, float denominator, Color color, bool isAdd) {
//		if (denominator == 0)
//			denominator = 1;
		
		if (isAdd) {
			this.AddStability (numerator, denominator, color);
		} else {
			this.SubtractStability (numerator, denominator, color);
		}
	}

	public void AddStability(float numerator, float denominator, Color color) {
		// The new stability will be the current stability + the added stability
		float[] addedStability = General.AddFractions(this.GetNumerator(), this.GetDenominator(), numerator, denominator);
		// Simplify (Reduced form)
		float[] newStability = General.SimplifyFraction(addedStability[0], addedStability[1]);

		float newNumerator = addedStability [0];
		float newDenominator = addedStability [1];


		this.GetStabilityPointer ().SetNumerator (newNumerator);
		this.GetStabilityPointer ().SetDenominator (newDenominator);

		// Where the new fraction point should be located in the numberline
		float endPosition = this.GetLineLength () * this.GetNumerator() / this.GetDenominator();

		// Added highlight's start position will be the end position of the last highlight
		float startPosition = this.GetHighlightEnd();

//		AddHighlight (numerator, denominator, startPosition, endPosition);
		StartCoroutine(AddHighlight (numerator, denominator, color, startPosition, endPosition));
	}

	public IEnumerator AddHighlight(float numerator, float denominator, Color color, float startPosition, float endPosition) {
		Highlight temp = Instantiate<Highlight> (highlightPrefab, this.highlightParent);

		temp.SetLength (this.GetLineLength ());
		temp.SetColor (color);
		temp.SetNumerator (numerator);
		temp.SetDenominator (denominator);
		temp.gameObject.transform.localPosition = Vector3.zero;

		temp.SetLineLength (startPosition, startPosition);

		this.highlights.Add (temp);
		float addedValue;
		while(temp.GetEnd() != endPosition) {
			addedValue = temp.GetEnd () + step;
			// Ensure addedValue does not go over endPosition
			if (addedValue > endPosition) {
				addedValue = endPosition;
			}


			temp.SetLineLength (temp.GetStart (), addedValue);
			this.GetStabilityPointer ().ChangeLocation (GetStabilityPointer ().GetLocalX () + step);
			this.GetStabilityPointer ().UpdateLabel (
				(int) (this.GetStabilityPointer ().GetLocalX () * this.GetStabilityPointer ().GetDenominator () / this.GetStabilityPointer ().GetLength ()),
				(int) GetDenominator ());
			
			yield return null;
		}
		temp.SetLineLength (startPosition, endPosition);

//		 Ensure that the stability pointer is pointing to the right value
		this.GetStabilityPointer ().PointTo (GetNumerator(), GetDenominator());
		StabilityCheck ();
		yield return null;
	}

	// You can only subtract a fraction whose value is in highlights
	public void SubtractStability(float numerator, float denominator, Color color) {
		
		int index = -1;
		for(int i = 0; i < highlights.Count; i++) {
			// Once found, exit the loop
//			if (highlights [i].IsMatch (numerator, denominator)) {
//				index = i;
//				i = highlights.Count + 1;
//			}
			if (highlights [i].IsMatch (numerator, denominator, color)) {
				index = i;
				i = highlights.Count + 1;
			}
		}

		if (index >= 0) {
			Highlight highlight = this.highlights [index];
			this.removalQueue.Enqueue (highlight);	
			if (!isSubtracting) {
				StartCoroutine (SubtractHighlight ());
			}

		}
//		else {
			// For error checking purposes only, TODO: remove
//			SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false);
//		}
	}

	// Move the pointer and the necessary highlight dependencies
	public IEnumerator SubtractHighlight() {
		this.isSubtracting = true;

		while (removalQueue.Count > 0) {
			Highlight highlight = this.removalQueue.Dequeue ();


			// Subtract highlight to get new stability value
			float[] subtractedResults = General.SubtractFractions(
				GetNumerator(), GetDenominator(),
				highlight.GetNumerator(), highlight.GetDenominator());

			// Simplify
//			float[] results = General.SimplifyFraction(subtractedResults[0], subtractedResults[1]);
			float[] results = subtractedResults;
			// Where the stability pointer should point to after animating
			float newNumerator = results [0];
			float newDenominator = results [1];

			// Start at the index next to the highlight
			int startIndex = this.highlights.IndexOf (highlight) + 1;

			float startPoint = highlight.GetStart ();
			float endPoint;


			// Loop until highlight startPoint = endPoint
			while(startPoint != highlight.GetEnd()) {
				endPoint = highlight.GetEnd () - step;
				if (endPoint < highlight.GetStart ()) {
					endPoint = highlight.GetStart ();
				}

				// Reduce the highlight length
				highlight.SetLineLength(highlight.GetStart(), endPoint);

				// Move back the highlights after it
				for (int i = startIndex; i < highlights.Count; i++) {
					this.highlights [i].SetLineLength (this.highlights [i].GetStart () - step,
						this.highlights [i].GetEnd () - step);
				}
				this.GetStabilityPointer ().ChangeLocation (GetStabilityPointer ().GetLocalX () - step);
				this.GetStabilityPointer ().UpdateLabel (
					(int) (this.GetStabilityPointer ().GetLocalX () * this.GetStabilityPointer ().GetDenominator () / this.GetStabilityPointer ().GetLength ()),
					(int) GetDenominator ());
				yield return null;
			}

			// Ensure that the stability pointer is pointing to the right value
			this.GetStabilityPointer ().PointTo (newNumerator, newDenominator);
			StabilityCheck ();
			this.highlights.Remove (highlight);
			yield return null;
		}

		this.isSubtracting = false;
	}

	public float GetHighlightEnd() {
		if (this.highlights.Count > 0) {
			return this.highlights [this.highlights.Count - 1].GetEnd ();
		} else {
			return 0f;
		}
	}
}
