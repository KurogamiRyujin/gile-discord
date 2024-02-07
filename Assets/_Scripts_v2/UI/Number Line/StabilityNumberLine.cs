using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special number line that serves as the room's stability meter.
/// </summary>
public class StabilityNumberLine : NumberLine {

    public const string IS_ADD = "add";
    public const string SUBTRACT = "subtract";
    public const string NUMERATOR = "numerator";
    public const string DENOMINATOR = "denominator";
    public const string COLOR = "color";
    public const string HOLLOW_BLOCK = "hollowBlock";
    public const string MAX_POINT = "maxPoint";

    [SerializeField] private int testDenom;
    [SerializeField] private int step = 2;

    /// <summary>
    /// Stability Numerator.
    /// Numerator value for the target fraction to stabilize the room.
    /// </summary>
    [SerializeField] private int targetNumerator;
    /// <summary>
    /// Stability Denominator.
    /// Denominator value for the target fraction to stabilize the room.
    /// </summary>
    [SerializeField] private int targetDenominator;

    /// <summary>
    /// Flag if the room is stabled.
    /// </summary>
    [SerializeField] private bool isStable;
    
    [SerializeField] private float addNumerator;
    [SerializeField] private float addDenominator;
    /// <summary>
    /// Flag if the change to the stability pointer will increase its value.
    /// </summary>
    [SerializeField] private bool callAdd;
    /// <summary>
    /// Flag if the change to the stability pointer will decrease its value.
    /// </summary>
    [SerializeField] private bool callSubtract;

    /// <summary>
    /// Reference to the highlight prefab.
    /// </summary>
    [SerializeField] private Highlight highlightPrefab;
    /// <summary>
    /// Reference point for the hightlight's position.
    /// </summary>
    [SerializeField] private Transform highlightParent;
    [SerializeField] private RectTransform pointer;

    /// <summary>
    /// Pointer to the room's current stability value.
    /// </summary>
    [SerializeField] private StabilityPointer stabilityPointer;
    /// <summary>
    /// Pointer to indicate the target point on the number line the stability pointer should point.
    /// </summary>
    [SerializeField] private TargetMarker targetMarker;

    /// <summary>
    /// Flag if the operation done is subtracting. Determines if the stability pointer should move left or right accordingly.
    /// 
    /// If it is true, then the stability is decreased so the stability pointer moves to the left. Otherwise, to the right.
    /// </summary>
    [SerializeField] private bool isSubtracting;

    //	[SerializeField] private int newDenominator;
    //	[SerializeField] private int newTargetNum;
    //	[SerializeField] private int newTargetDen;
    //	[SerializeField] private bool isValueChanged;

    /// <summary>
    /// List of all highlights on the stability number line.
    /// </summary>
    private List<Highlight> highlights;
    /// <summary>
    /// Queue to determine the order the highlights will be removed.
    /// </summary>
    private Queue<Highlight> removalQueue;
    /// <summary>
    /// Queue of the highlight objects.
    /// </summary>
    private Queue<HighlightObject> highlightObjects;
    /// <summary>
    /// Flag if the stability pointer is moving.
    /// </summary>
    private bool isManipulating;
    /// <summary>
    /// Flag if the highlights are being processed.
    /// </summary>
    private bool isProcessingHighlight;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
    void Awake() {
        base.Awake();
        this.AwakeFunctions();
        //this.highlights = new List<Highlight>();
        //this.removalQueue = new Queue<Highlight>();
        //this.highlightObjects = new Queue<HighlightObject>();
        //this.isStable = false;
        this.step = 9;
        EventBroadcaster.Instance.AddObserver(EventNames.ON_HOLLOW_STABILITY_UPDATE, HollowUpdate);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_HOLLOW_STABILITY_UPDATE_INSTANT, HollowUpdateInstant);
        EventBroadcaster.Instance.AddObserver(EventNames.DESTABILIZE, Destabilize);
        EventBroadcaster.Instance.AddObserver(EventNames.SET_MAX_POINT, SetMaxObserver);
        //EventBroadcaster.Instance.AddObserver(EventNames.RETRY, Reset);
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_HOLLOW_STABILITY_UPDATE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_HOLLOW_STABILITY_UPDATE_INSTANT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.SET_MAX_POINT);
        //EventBroadcaster.Instance.RemoveObserver(EventNames.RETRY);
    }

    /// <summary>
    /// Sets the max point of the number line.
    /// </summary>
    /// <param name="parameters">Parameter object containing the max point.</param>
    public void SetMaxObserver(Parameters parameters) {
        int newMaxPoint = parameters.GetIntExtra(MAX_POINT, 1);
        this.SetMaxPoint(newMaxPoint);
        Debug.LogError("MAX OBSERVER GOT " + this.GetMaxPoint());
    }

    /// <summary>
    /// Prompt that the room is unstable.
    /// Sets isStable to false.
    /// </summary>
    void Destabilize() {
        this.isStable = false;
    }

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start() {
        this.StartFunctions();
        ////		this.SetMaxPoint (testDenom);
        //if (this.GetMaxPoint() == 0) {
        //    this.SetMaxPoint(1);
        //}
        //this.SetMaxPoint(this.GetMaxPoint());
        //this.Segment(testDenom);
        ////		this.AddMarker (testDenom, 2, Fraction.Notation.DECIMAL);

        ////		for (int i = 0; i < 3; i++) {
        //////			AddHighlight (pointer);
        ////			AddHighlight ();
        ////		}
        //this.GetStabilityPointer().SetLength(this.GetLineLength());
        ////		this.GetTargetMarker ().PointTo (0, (float)this.testDenom);
        //this.GetTargetMarker().PointTo(0, (float)this.testDenom, this.GetMaxPoint());

        ////		this.GetStabilityPointer ().SetNumerator(0);
        ////		this.GetStabilityPointer ().SetDenominator((float)this.testDenom);
        ////		this.GetStabilityPointer ().UpdateLabel ();

        //this.GetTargetMarker().SetLength(this.GetLineLength());
        ////		this.GetTargetMarker ().PointTo ((float)this.targetNumerator, (float)this.targetDenominator);
        //this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());
        ////this.StabilityCheck();
        //this.StabilityCheckNoPrompt();
    }

    /// <summary>
    /// Function called in the Awake function.
    /// </summary>
    void AwakeFunctions() {
        this.highlights = new List<Highlight>();
        this.removalQueue = new Queue<Highlight>();
        this.highlightObjects = new Queue<HighlightObject>();
        this.isStable = false;
    }

    /// <summary>
    /// Function called in the Start function.
    /// </summary>
    void StartFunctions() {
        //		this.SetMaxPoint (testDenom);
        if (this.GetMaxPoint() == 0) {
            this.SetMaxPoint(1);
        }
        this.SetMaxPoint(this.GetMaxPoint());
        this.Segment(testDenom);
        //		this.AddMarker (testDenom, 2, Fraction.Notation.DECIMAL);

        //		for (int i = 0; i < 3; i++) {
        ////			AddHighlight (pointer);
        //			AddHighlight ();
        //		}
        this.GetStabilityPointer().SetLength(this.GetLineLength());
        //		this.GetTargetMarker ().PointTo (0, (float)this.testDenom);
        //this.GetTargetMarker().PointTo(0, (float)this.testDenom, this.GetMaxPoint());

        //		this.GetStabilityPointer ().SetNumerator(0);
        //		this.GetStabilityPointer ().SetDenominator((float)this.testDenom);
        //		this.GetStabilityPointer ().UpdateLabel ();

        this.GetTargetMarker().SetLength(this.GetLineLength());
        //		this.GetTargetMarker ().PointTo ((float)this.targetNumerator, (float)this.targetDenominator);
        this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());
        //this.StabilityCheck();
        this.StabilityCheckNoPrompt();
    }
    //	void Update() {
    //		if (this.isValueChanged) {
    //			this.isValueChanged = false;
    //			this.ChangeNumberLineValue ();
    //		}
    //	}

    //	public void ChangeNumberLineValue() {
    //		if (this.newDenominator > 0) {
    //			this.ChangeValue (this.newDenominator, this.newTargetNum, this.newTargetDen);
    //		}
    //	}

    // Use this to edit the numberline value
    //public void ChangeValue(int lineDen, int targetNum, int targetDen, int maxValue) {
    //    this.testDenom = lineDen;
    //    this.targetNumerator = targetNum;
    //    this.targetDenominator = targetDen;
    //    //if (maxValue == 0)
    //    //    maxValue = 1;
    //    this.SetMaxPoint(maxValue);
    //    this.Segment(testDenom);

    //    this.GetStabilityPointer().SetLength(this.GetLineLength());
    //    //		this.GetTargetMarker ().PointTo (this.targetNumerator, (float)this.targetDenominator);
    //    //this.GetTargetMarker().PointTo(this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());

    //    this.GetTargetMarker().SetLength(this.GetLineLength());
    //    //		this.GetTargetMarker ().PointTo ((float)this.targetNumerator, (float)this.targetDenominator);
    //    this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());
    //    //		StartCoroutine (ReconfigureHighlights ());
    //    this.StabilityCheck();
    //}
    //public void ChangeValueNoPrompt(int lineDen, int targetNum, int targetDen, int maxValue) {
    //    this.testDenom = lineDen;
    //    this.targetNumerator = targetNum;
    //    this.targetDenominator = targetDen;
    //    //if (maxValue == 0)
    //    //    maxValue = 1;
    //    this.SetMaxPoint(maxValue);
    //    this.Segment(testDenom);

    //    this.GetStabilityPointer().SetLength(this.GetLineLength());
    //    //		this.GetTargetMarker ().PointTo (this.targetNumerator, (float)this.targetDenominator);
    //    //this.GetTargetMarker().PointTo(this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());

    //    this.GetTargetMarker().SetLength(this.GetLineLength());
    //    //		this.GetTargetMarker ().PointTo ((float)this.targetNumerator, (float)this.targetDenominator);
    //    //this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint()+1);

    //    this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());
    //    //		StartCoroutine (ReconfigureHighlights ());
    //    this.StabilityCheckNoPrompt();
    //}

    /// <summary>
    /// Changes the value of the target marker.
    /// Adjusts the length of the number line to be a reference to the pointers' movements.
    /// </summary>
    /// <param name="lineDen">Length of the Number Line as a Denominator</param>
    /// <param name="targetNum">New Target Numerator</param>
    /// <param name="targetDen">New Target Denominator</param>
    public void ChangeValueNoPrompt(int lineDen, int targetNum, int targetDen) {
        this.testDenom = lineDen;
        this.targetNumerator = targetNum;
        this.targetDenominator = targetDen;
        //if (maxValue == 0)
        //    maxValue = 1;
        this.Segment(testDenom);

        this.GetStabilityPointer().SetLength(this.GetLineLength());
        //		this.GetTargetMarker ().PointTo (this.targetNumerator, (float)this.targetDenominator);
        //this.GetTargetMarker().PointTo(this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());

        this.GetTargetMarker().SetLength(this.GetLineLength());
        //		this.GetTargetMarker ().PointTo ((float)this.targetNumerator, (float)this.targetDenominator);
        //this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint()+1);

        this.GetTargetMarker().PointTo((float)this.targetNumerator, (float)this.targetDenominator, this.GetMaxPoint());
        //		StartCoroutine (ReconfigureHighlights ());
        this.StabilityCheckNoPrompt();
    }

    /// <summary>
    /// Changes the value of the target marker.
    /// Adjusts the length of the number line to be a reference to the pointers' movements.
    /// </summary>
    /// <param name="lineDen">Length of the Number Line as a Denominator</param>
    /// <param name="targetNum">New Target Numerator</param>
    /// <param name="targetDen">New Target Denominator</param>
    /// <param name="maxValue">Max Value on the Number Line</param>
    public void ChangeValueNoPrompt(int lineDen, int targetNum, int targetDen, int maxValue) {

        this.SetMaxPoint(maxValue);
        this.ChangeValueNoPrompt(lineDen, targetNum, targetDen);
    }

    /// <summary>
    /// Check if the stability value is the same as the target value.
    /// 
    /// If it is and the room was unstable, the room is stabilized and broadcasts events connected to the room stabilization. Otherwise, destabilization events are broadcast.
    /// </summary>
    private void StabilityCheck() {
        Debug.LogError("PROMPT STABILITY");
        float[] simpleStability = General.SimplifyFraction(this.stabilityPointer.GetFractionLabel().GetNumerator(), this.stabilityPointer.GetFractionLabel().GetDenominator());
        float[] simpleTarget = General.SimplifyFraction(this.targetMarker.GetFractionLabel().GetNumerator(), this.targetMarker.GetFractionLabel().GetDenominator());

        int stabilityNum = (int)simpleStability[0];
        int stabilityDenom = (int)simpleStability[1];
        int targetNum = (int)simpleTarget[0];
        int targetDenom = (int)simpleTarget[1];

        //		Debug.LogError("ENTERED STABILITY CHECK "+"STAB IS "+stabilityNum+"/"+stabilityDenom+"AND TARG IS "+targetNum+"/"+targetDenom);

        if (stabilityNum == targetNum && stabilityDenom == targetDenom) {
            //			Debug.LogError ("ENTERED STABILITY");
            EventBroadcaster.Instance.PostEvent(EventNames.VULNERABLE_ENEMIES);
            EventBroadcaster.Instance.PostEvent(EventNames.STABLE_AREA);
            EventBroadcaster.Instance.PostEvent(EventNames.RECORD_ON_AREA_STABLE);
            //			Debug.LogError ("POSTED STABILITY");
            this.isStable = true;
            this.GetTargetMarker().GetFractionLabel().Hide();
        }
        else {
            if (this.isStable) {
                EventBroadcaster.Instance.PostEvent(EventNames.BREAK_AREA);
            }
            this.isStable = false;
            EventBroadcaster.Instance.PostEvent(EventNames.INVULNERABLE_ENEMIES);
            EventBroadcaster.Instance.PostEvent(EventNames.UNSTABLE_AREA);
            this.GetTargetMarker().GetFractionLabel().Show();
        }
    }

    /// <summary>
    /// Coroutine removing all the highlights.
    /// </summary>
    /// <returns>None</returns>
    public IEnumerator ResetRoutine() {

        yield return StartCoroutine(this.RemoveAllHighlights());
        //this.AwakeFunctions();
        //this.removalQueue.Clear();
        //this.highlightObjects.Clear();
        //this.isStable = false;
        //this.StartFunctions();
        //yield return null;
    }

    /// <summary>
    /// Resets the stability pointer's position and value.
    /// </summary>
    public void ResetPointers() {

        this.GetStabilityPointer().SetNumerator(0f);
        this.GetStabilityPointer().SetDenominator(1f);
    }

    /// <summary>
    /// Stops coroutine processes.
    /// </summary>
    public void Reset() {
        StopAllCoroutines();
    }

    /// <summary>
    /// Coroutine for removing all highlights one at a time.
    /// </summary>
    /// <returns>None</returns>
    public IEnumerator RemoveAllHighlights() {
        if (this.highlights != null) {
            GameObject highlightObject;
            for (int i = highlights.Count - 1; i >= 0; i--) {
                highlightObject = this.highlights[i].gameObject;
                highlightObject.SetActive(false);
                Destroy(highlightObject);
            }
            //int i = 0;
            //foreach (Highlight highlight in highlights) {
            //    highlights.Remove(highlight);
            //    Destroy(highlight.gameObject);
            //    //Debug.Log("<color=magenta>Removed </color>"+i);
            //    //i++;
            //    yield return null;
            //}
        }
        this.highlights.Clear();
        yield return null;
    }

    /// <summary>
    /// Check if the stability value is the same as the target value.
    /// 
    /// If it is and the room was unstable, the room is stabilized and broadcasts events connected to the room stabilization. Otherwise, destabilization events are broadcast.
    /// </summary>
    private void StabilityCheckNoPrompt() {
        float[] simpleStability = General.SimplifyFraction(this.stabilityPointer.GetFractionLabel().GetNumerator(), this.stabilityPointer.GetFractionLabel().GetDenominator());
        float[] simpleTarget = General.SimplifyFraction(this.targetMarker.GetFractionLabel().GetNumerator(), this.targetMarker.GetFractionLabel().GetDenominator());

        Debug.LogError("NO PROMPT STABILITY");
        int stabilityNum = (int)simpleStability[0];
        int stabilityDenom = (int)simpleStability[1];
        int targetNum = (int)simpleTarget[0];
        int targetDenom = (int)simpleTarget[1];

        //		Debug.LogError("ENTERED STABILITY CHECK "+"STAB IS "+stabilityNum+"/"+stabilityDenom+"AND TARG IS "+targetNum+"/"+targetDenom);

        EventBroadcaster.Instance.PostEvent(EventNames.STABLE_AREA_NO_PROMPT);
        if (stabilityNum == targetNum && stabilityDenom == targetDenom) {
            //			Debug.LogError ("ENTERED STABILITY");
            EventBroadcaster.Instance.PostEvent(EventNames.VULNERABLE_ENEMIES);
            EventBroadcaster.Instance.PostEvent(EventNames.STABLE_AREA);
            EventBroadcaster.Instance.PostEvent(EventNames.RECORD_ON_AREA_STABLE);

            //			Debug.LogError ("POSTED STABILITY");
            this.isStable = true;
            this.GetTargetMarker().GetFractionLabel().Hide();
        }
        else {
            if (this.isStable) {
                EventBroadcaster.Instance.PostEvent(EventNames.STABLE_AREA_WITH_PROMPT);
                EventBroadcaster.Instance.PostEvent(EventNames.BREAK_AREA);
            }
            this.isStable = false;
            EventBroadcaster.Instance.PostEvent(EventNames.INVULNERABLE_ENEMIES);
            EventBroadcaster.Instance.PostEvent(EventNames.UNSTABLE_AREA);
            this.GetTargetMarker().GetFractionLabel().Show();
        }

        EventBroadcaster.Instance.PostEvent(EventNames.STABLE_AREA_WITH_PROMPT);
    }

    /// <summary>
    /// Checker if the room is stable.
    /// </summary>
    /// <returns></returns>
    public bool IsStable() {
        return this.isStable;
    }

    /// <summary>
    /// Update the stability.
    /// </summary>
    /// <param name="parameters">Parameter containing the ghost block which will affect the stability.</param>
    public void HollowUpdate(Parameters parameters) {
        float numerator = parameters.GetFloatExtra(NUMERATOR, 0f);
        float denominator = parameters.GetFloatExtra(DENOMINATOR, 1f);
        Color color = (Color)parameters.GetObjectExtra(COLOR);
        bool isAdd = parameters.GetBoolExtra(IS_ADD, true);
        HollowBlock block = (HollowBlock)parameters.GetObjectExtra(HOLLOW_BLOCK);
        this.ManipulateStability(numerator, denominator, color, isAdd, block);
        //		StabilityCheck ();

    }

    /// <summary>
    /// Update the stability.
    /// </summary>
    /// <param name="parameters">Parameter containing the ghost block which will affect the stability.</param>
    public void HollowUpdateInstant(Parameters parameters) {
        Debug.LogError("Entered instant");
        float numerator = parameters.GetFloatExtra(NUMERATOR, 0f);
        float denominator = parameters.GetFloatExtra(DENOMINATOR, 1f);
        Color color = (Color)parameters.GetObjectExtra(COLOR);
        bool isAdd = parameters.GetBoolExtra(IS_ADD, true);
        HollowBlock block = (HollowBlock)parameters.GetObjectExtra(HOLLOW_BLOCK);
        StartCoroutine(this.ManipulateStability(numerator, denominator, color, isAdd, block, true));
        //		StabilityCheck ();

    }
    
    /// <summary>
    /// Returns the numerator of the stability pointer.
    /// </summary>
    /// <returns>Stability Pointer Numerator</returns>
    public float GetNumerator() {
        return this.GetStabilityPointer().GetNumerator();
    }

    /// <summary>
    /// Returns the denominator of the stability pointer.
    /// </summary>
    /// <returns>Stability Pointer Denominator</returns>
    public float GetDenominator() {
        return this.GetStabilityPointer().GetDenominator();
    }


    /// <summary>
    /// Provides null check. Assign stabilityPointer in inspector but use this just to be safe.
    /// </summary>
    /// <returns>Stability Pointer</returns>
    public StabilityPointer GetStabilityPointer() {
        if (this.stabilityPointer == null) {
            this.stabilityPointer = GetComponentInChildren<StabilityPointer>();
        }
        return this.stabilityPointer;
    }

    /// <summary>
    /// Provides null check. Assign targetMarker in inspector but use this just to be safe.
    /// </summary>
    /// <returns>Target Marker</returns>
    public TargetMarker GetTargetMarker() {
        if (this.targetMarker == null) {
            this.targetMarker = GetComponentInChildren<TargetMarker>();
        }
        return this.targetMarker;
    }

    /// <summary>
    /// Coroutine which adjusts the highlights and stability pointer on the number line during a stability update.
    /// </summary>
    /// <param name="numerator">Numerator of affecting fraction</param>
    /// <param name="denominator">Denominator of affecting fraction</param>
    /// <param name="color">Highlight Color</param>
    /// <param name="isAdd">If it is addition operation.</param>
    /// <param name="block">Ghost Block affecting the stability.</param>
    /// <param name="isInstant">If the process should be done instantly.</param>
    /// <returns>None</returns>
    public IEnumerator ManipulateStability(float numerator, float denominator, Color color, bool isAdd, HollowBlock block, bool isInstant) {

        Debug.LogError("Entered manipulate");
        block.ProcessBlock();
        if (isAdd) {
            Debug.LogError("INSTANT ADD");
            //Debug.Log("<color=pink>INSTANT ADD</color>");
            HighlightObject highlight = new HighlightObject(numerator, denominator, color, isAdd, block);

            //float endPosition = this.GetLineLength() * numerator / denominator / GetMaxPoint();
            //float endPosition = (this.GetLineLength() / (GetMaxPoint() + 1)) * numerator / denominator;
            //float otherEndPosition = this.GetLineLength() * GetNumerator() / GetDenominator() / GetMaxPoint();
            //float startPosition = this.GetHighlightEnd();
            //Debug.Log("<color=pink>start position is </color>" + startPosition);
            //Debug.Log("<color=pink>end position is </color>"+endPosition);
            //Debug.Log("<color=pink>original end position was </color>" + otherEndPosition);
            // Added highlight's start position will be the end position of the last highlight
            yield return StartCoroutine(AddStabilityInstant(numerator, denominator, color));
            //this.AddHighlightInstant(numerator, denominator, color, startPosition, endPosition);
            //StartCoroutine(AddHighlight(numerator, denominator, color, startPosition, endPosition));
            block.ProcessingDone();
        }
        else {

            Debug.LogError("INSTANT SUBTRACT");
            //Debug.Log("<color=pink>INSTANT SUBTRACT</color>");
            HighlightObject highlight = new HighlightObject(numerator, denominator, color, isAdd, block);

            yield return StartCoroutine(SubtractStabilityInstant(numerator, denominator, color));
            block.ProcessingDone();
        }
        yield return null;
    }

    /// <summary>
    /// Addition operation stability update. Coroutine moving the stability pointer towards the right of the number line.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction</param>
    /// <param name="denominator">Denominator of the affecting fraction</param>
    /// <param name="color">Highlight Color to be added.</param>
    /// <returns>None</returns>
    public IEnumerator AddStabilityInstant(float numerator, float denominator, Color color) {

        Debug.Log("<color=orange>STAB Value: </color>" + numerator + " / " + denominator);
        // The new stability will be the current stability + the added stability
        float[] addedStability = General.AddFractions(this.GetNumerator(), this.GetDenominator(), numerator, denominator);
        // Simplify (Reduced form)
        float[] newStability = General.SimplifyFraction(addedStability[0], addedStability[1]);

        float newNumerator = addedStability[0];
        float newDenominator = addedStability[1];


        this.GetStabilityPointer().SetNumerator(newNumerator);
        this.GetStabilityPointer().SetDenominator(newDenominator);

        // Where the new fraction point should be located in the numberline
        //		float endPosition = this.GetLineLength () * this.GetNumerator() / this.GetDenominator();
        //float endPosition = this.GetLineLength() * numerator / denominator / GetMaxPoint();

        // Added highlight's start position will be the end position of the last highlight
        float startPosition = this.GetHighlightEnd();
        //float endPosition = startPosition+((this.GetLineLength()/(this.GetMaxPoint()+1)) * numerator / denominator);
        // ADDED MAX CHANGED but was working
        float endPosition = startPosition + ((this.GetLineLength() / (this.GetMaxPoint())) * numerator / denominator);
        Debug.LogError("INSTA ADD");
        Debug.LogError("Highlight size: " + this.highlights.Count);
        Debug.LogError("Line Length is: " + GetLineLength());
        Debug.LogError("Start Position: " + startPosition);
        Debug.LogError("End Position: " + endPosition);
        Debug.LogError("Line Length: " + this.GetLineLength());
        Debug.LogError("Fraction: " + numerator / denominator);
        Debug.LogError("Fraction WHOLE: " + newNumerator / newDenominator);
        Debug.LogError("Max Point: " + GetMaxPoint());

        //		AddHighlight (numerator, denominator, startPosition, endPosition);
        yield return StartCoroutine(AddHighlightInstant(numerator, denominator, color, startPosition, endPosition));
    }

    /// <summary>
    /// Subtraction operation stability update. Coroutine moving the stability pointer towards the left of the stability number line.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction.</param>
    /// <param name="denominator">Denominator of the affecting fraction.</param>
    /// <param name="color">Highlight color to be removed.</param>
    /// <returns>None</returns>
    public IEnumerator SubtractStabilityInstant(float numerator, float denominator, Color color) {

        int index = -1;
        for (int i = 0; i < highlights.Count; i++) {
            // Once found, exit the loop
            //			if (highlights [i].IsMatch (numerator, denominator)) {
            //				index = i;
            //				i = highlights.Count + 1;
            //			}
            if (highlights[i].IsMatch(numerator, denominator, color)) {
                index = i;
                i = highlights.Count + 1;
            }
            yield return null;
        }

        if (index >= 0) {
            Highlight highlight = this.highlights[index];
            //this.removalQueue.Enqueue(highlight);
            //if (!isSubtracting) {
            yield return StartCoroutine(SubtractHighlightInstant(highlight, index));
            //}
            Debug.Log("<color=red>ENTERED SUBTRACT INSTANT</color>");
        }
        yield return null;
        //yield return StartCoroutine(AddHighlightInstant(numerator, denominator, color, startPosition, endPosition));
    }

    /// <summary>
    /// Coroutine adding a new highlight.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction.</param>
    /// <param name="denominator">Denominator of the affecting fraction.</param>
    /// <param name="color">Highlight color to be added.</param>
    /// <param name="startPosition">X coordinate where the highlight starts.</param>
    /// <param name="endPosition">X coordindate where the highlight ends.</param>
    /// <returns>None</returns>
    public IEnumerator AddHighlightInstant(float numerator, float denominator, Color color, float startPosition, float endPosition) {
        Highlight temp = Instantiate<Highlight>(highlightPrefab, this.highlightParent);

        temp.SetLength(this.GetLineLength());
        temp.SetColor(color);
        temp.SetNumerator(numerator);
        temp.SetDenominator(denominator);
        temp.gameObject.transform.localPosition = Vector3.zero;

        //temp.SetLineLength(startPosition, startPosition);

        temp.SetLineLength(startPosition, endPosition);
        this.highlights.Add(temp);
        //float addedValue;
        //while (temp.GetEnd() != endPosition) {
        //    temp.SetLineLength(temp.GetStart(), endPosition);
        //    this.GetStabilityPointer().ChangeLocation(endPosition);
        //    //this.GetStabilityPointer().ChangeLocation(GetStabilityPointer().GetLocalX() + step);
        //    // this.GetStabilityPointer ().ChangeLocation ((GetStabilityPointer ().GetLocalX () + step)/(float)GetMaxPoint());
        //    this.GetStabilityPointer().UpdateLabel(
        //        (int)(this.GetStabilityPointer().GetLocalX() * this.GetStabilityPointer().GetDenominator() / this.GetStabilityPointer().GetLength()),
        //        (int)GetDenominator());

        //    yield return null;
        //}

        //		 Ensure that the stability pointer is pointing to the right value
        //		this.GetStabilityPointer ().PointTo (GetNumerator(), GetDenominator());

        //this.GetStabilityPointer().ChangeLocation(endPosition);
        // ADDED but already had +1
        //this.GetStabilityPointer().PointTo(GetNumerator(), GetDenominator(), this.GetMaxPoint()+1);
        // ADDED MAX CHANGE but was working (?)
        this.GetStabilityPointer().PointTo(GetNumerator(), GetDenominator(), this.GetMaxPoint());
        //this.GetStabilityPointer().PointTo(GetNumerator(), GetDenominator(), this.GetMaxPoint());
        //		StabilityCheck ();
        //this.isProcessingHighlight = false;
        yield return null;
    }

    /// <summary>
    /// Coroutine removing a highlight.
    /// </summary>
    /// <param name="temp">Highlight to be removed.</param>
    /// <param name="index">Index of the highlight.</param>
    /// <returns>None</returns>
    public IEnumerator SubtractHighlightInstant(Highlight temp, int index) {

        temp.SetLineLength(temp.GetStart(), temp.GetStart());
        //this.highlights.RemoveAt(index);

        // Subtract highlight to get new stability value
        float[] subtractedResults = General.SubtractFractions(
            GetNumerator(), GetDenominator(),
            temp.GetNumerator(), temp.GetDenominator());

        float[] results = subtractedResults;

        // Where the stability pointer should point to after animating
        float newNumerator = results[0];
        float newDenominator = results[1];

        // Ensure that the stability pointer is pointing to the right value
        //			this.GetStabilityPointer ().PointTo (newNumerator, newDenominator);
        // ADDED +1
        //this.GetStabilityPointer().PointTo(newNumerator, newDenominator, GetMaxPoint()+1);
        // ADDED MAX CHANGE but was working (?)
        this.GetStabilityPointer().PointTo(newNumerator, newDenominator, GetMaxPoint());
        int lastIndex = highlights.Count - 1;

        // Add stepcount to last index or ensure it points to proper place
        //if (lastIndex >= 0) {
        //    //				this.highlights [lastIndex].SetLineLength (highlights [lastIndex].GetStart(), highlights [lastIndex].GetEnd());
        //    this.highlights[lastIndex].SetLineLength(highlights[lastIndex].GetStart(),
        //        this.GetStabilityPointer().GetLocalX());
        //}

        this.highlights.RemoveAt(index);
        Destroy(temp.gameObject);
        yield return null;
    }

    /// <summary>
    /// Update the stability with the affecting fraction. Start the coroutines handling the stability pointer and highlight update.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction.</param>
    /// <param name="denominator">Denominator of affecting fraction.</param>
    /// <param name="color">Highlight Color associated with affecting ghost block.</param>
    /// <param name="isAdd">If the update is from an addition operation.</param>
    /// <param name="block">Affecting Ghost Block</param>
    public void ManipulateStability(float numerator, float denominator, Color color, bool isAdd, HollowBlock block) {
        //		if (denominator == 0)
        //			denominator = 1;
        this.highlightObjects.Enqueue(new HighlightObject(numerator, denominator, color, isAdd, block));
        if (!isManipulating) {
            this.isManipulating = true;
            StartCoroutine(ManipulateRoutine());
        }
        //		if (isAdd) {
        //			this.AddStability (numerator, denominator, color);
        //		} else {
        //			this.SubtractStability (numerator, denominator, color);
        //		}
    }

    /// <summary>
    /// Coroutine updating the highlights.
    /// </summary>
    /// <returns></returns>
    IEnumerator ManipulateRoutine() {
        HighlightObject highlightObject;

        while (this.highlightObjects.Count > 0) {
            this.isProcessingHighlight = true;
            highlightObject = this.highlightObjects.Dequeue();
            if (highlightObject.IsAdd()) {
                Debug.LogError("ADDSTAB");
                yield return StartCoroutine(this.AddStability(highlightObject.GetNumerator(), highlightObject.GetDenominator(), highlightObject.GetColor()));
            }
            else {
                Debug.LogError("SUBSTAB");
                yield return StartCoroutine(this.SubtractStability(highlightObject.GetNumerator(), highlightObject.GetDenominator(), highlightObject.GetColor()));
            }
            while (isProcessingHighlight) {
                yield return null;
            }
            yield return null;
        }
        this.isManipulating = false;
        this.StabilityCheck();
        yield return null;
    }

    /// <summary>
    /// Addition operation stability update. Coroutine moving the stability pointer towards the right of the number line.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction</param>
    /// <param name="denominator">Denominator of the affecting fraction</param>
    /// <param name="color">Highlight Color to be added.</param>
    /// <returns>None</returns>
    public IEnumerator AddStability(float numerator, float denominator, Color color) {
        // The new stability will be the current stability + the added stability
        float[] addedStability = General.AddFractions(this.GetNumerator(), this.GetDenominator(), numerator, denominator);
        // Simplify (Reduced form)
        float[] newStability = General.SimplifyFraction(addedStability[0], addedStability[1]);

        float newNumerator = addedStability[0];
        float newDenominator = addedStability[1];


        this.GetStabilityPointer().SetNumerator(newNumerator);
        this.GetStabilityPointer().SetDenominator(newDenominator);

        // Where the new fraction point should be located in the numberline
        //		float endPosition = this.GetLineLength () * this.GetNumerator() / this.GetDenominator();

        // ADDED MAX CHANGE but was working <!!!>
        //float endPosition = this.GetLineLength() * this.GetNumerator() / this.GetDenominator() / GetMaxPoint();

        float startPosition = this.GetHighlightEnd();
        float endPosition = startPosition+((this.GetLineLength()/GetMaxPoint()) * numerator / denominator);
        // WAS WORKING
        //float endPosition = (this.GetLineLength() / (this.GetMaxPoint() + 1)) * numerator / denominator;
        Debug.Log("End Position: " + endPosition);
        Debug.Log("Line Length: " + this.GetLineLength());
        Debug.Log("Fraction: " + this.GetNumerator() / this.GetDenominator());
        Debug.Log("Max Point: " + GetMaxPoint());
        // Added highlight's start position will be the end position of the last highlight
        //float startPosition = this.GetHighlightEnd();

        //		AddHighlight (numerator, denominator, startPosition, endPosition);
        yield return StartCoroutine(AddHighlight(numerator, denominator, color, startPosition, endPosition));
    }

    /// <summary>
    /// Coroutine adding a new highlight.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction.</param>
    /// <param name="denominator">Denominator of the affecting fraction.</param>
    /// <param name="color">Highlight color to be added.</param>
    /// <param name="startPosition">X coordinate where the highlight starts.</param>
    /// <param name="endPosition">X coordindate where the highlight ends.</param>
    /// <returns>None</returns>
    public IEnumerator AddHighlight(float numerator, float denominator, Color color, float startPosition, float endPosition) {
        Highlight temp = Instantiate<Highlight>(highlightPrefab, this.highlightParent);

        temp.SetLength(this.GetLineLength());
        temp.SetColor(color);
        temp.SetNumerator(numerator);
        temp.SetDenominator(denominator);
        temp.gameObject.transform.localPosition = Vector3.zero;

        temp.SetLineLength(startPosition, startPosition);

        this.highlights.Add(temp);
        float addedValue;
        while (temp.GetEnd() != endPosition) {
            addedValue = temp.GetEnd() + step;
            // Ensure addedValue does not go over endPosition
            if (addedValue > endPosition) {
                addedValue = endPosition;
            }


            temp.SetLineLength(temp.GetStart(), addedValue);
            this.GetStabilityPointer().ChangeLocation(GetStabilityPointer().GetLocalX() + step);
            //this.GetStabilityPointer().ChangeLocation((GetStabilityPointer().GetLocalX() + step) / (float)GetMaxPoint());
            this.GetStabilityPointer().UpdateLabel(
                (int)(this.GetStabilityPointer().GetLocalX() * this.GetStabilityPointer().GetDenominator() / this.GetStabilityPointer().GetLength()),
                (int)GetDenominator());

            yield return null;
        }
        temp.SetLineLength(startPosition, endPosition);

        //		 Ensure that the stability pointer is pointing to the right value
        //		this.GetStabilityPointer ().PointTo (GetNumerator(), GetDenominator());
        // ADDED +1
        //this.GetStabilityPointer().PointTo(GetNumerator(), GetDenominator(), this.GetMaxPoint()+1);
        // ADDED MAX CHANGE but was working
        this.GetStabilityPointer().PointTo(GetNumerator(), GetDenominator(), this.GetMaxPoint());
        //		StabilityCheck ();
        this.isProcessingHighlight = false;
        yield return null;
    }

    /// <summary>
    /// Subtraction operation stability update. Coroutine moving the stability pointer towards the left of the stability number line.
    /// You can only subtract a fraction whose value is in highlights.
    /// </summary>
    /// <param name="numerator">Numerator of the affecting fraction.</param>
    /// <param name="denominator">Denominator of the affecting fraction.</param>
    /// <param name="color">Highlight color to be removed.</param>
    /// <returns>None</returns>
    public IEnumerator SubtractStability(float numerator, float denominator, Color color) {

        int index = -1;
        for (int i = 0; i < highlights.Count; i++) {
            // Once found, exit the loop
            //			if (highlights [i].IsMatch (numerator, denominator)) {
            //				index = i;
            //				i = highlights.Count + 1;
            //			}
            if (highlights[i].IsMatch(numerator, denominator, color)) {
                index = i;
                i = highlights.Count + 1;
            }
            yield return null;
        }

        if (index >= 0) {
            Highlight highlight = this.highlights[index];
            this.removalQueue.Enqueue(highlight);
            if (!isSubtracting) {
                yield return StartCoroutine(SubtractHighlight());
            }

        }
        //		else {
        // For error checking purposes only, TODO: remove
        //			SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false);
        //		}
        yield return null;
    }
    
    /// <summary>
    /// Coroutine removing a highlight. Move the pointer and the necessary highlight dependencies.
    /// </summary>
    /// <param name="temp">Highlight to be removed.</param>
    /// <param name="index">Index of the highlight.</param>
    /// <returns>None</returns>
    public IEnumerator SubtractHighlight() {
        this.isSubtracting = true;

        while (removalQueue.Count > 0) {
            Highlight highlight = this.removalQueue.Dequeue();


            // Subtract highlight to get new stability value
            float[] subtractedResults = General.SubtractFractions(
                GetNumerator(), GetDenominator(),
                highlight.GetNumerator(), highlight.GetDenominator());

            // Simplify
            //			float[] results = General.SimplifyFraction(subtractedResults[0], subtractedResults[1]);
            float[] results = subtractedResults;
            // Where the stability pointer should point to after animating
            float newNumerator = results[0];
            float newDenominator = results[1];

            // Start at the index next to the highlight
            int startIndex = this.highlights.IndexOf(highlight) + 1;

            float startPoint = highlight.GetStart();
            float endPoint;
            //			float initialEndPoint = highlight.GetEnd ();
            float subStartPoint;
            float subEndPoint;


            // Loop until highlight startPoint = endPoint
            while (startPoint != highlight.GetEnd()) {
                endPoint = highlight.GetEnd() - step;
                if (endPoint < highlight.GetStart()) {
                    endPoint = highlight.GetStart();
                }

                // Reduce the highlight length
                //				highlight.SetLineLength(highlight.GetStart(), endPoint);
                highlight.SetLineLength(startPoint, endPoint);

                // Move back the highlights after it
                for (int i = startIndex; i < highlights.Count; i++) {
                    //if (i == startIndex) {
                    //    subStartPoint = endPoint;
                    //}
                    //else {
                        subStartPoint = this.highlights[i - 1].GetEnd();
                    //}

                    subEndPoint = this.highlights[i].GetEnd() - step;


                    if (subEndPoint < subStartPoint) {
                        subEndPoint = subStartPoint;
                    }



                    this.highlights[i].SetLineLength(subStartPoint, subEndPoint);
                    //					this.highlights [i].SetLineLength (this.highlights [i].GetStart () - step,
                    //						this.highlights [i].GetEnd () - step);
                    yield return null;
                }


                //				// If the removed highlight was not the last instance, add step count to the last instance. Fixes bug
                //				if (highlight != highlights [lastIndex]) {
                //					this.highlights [lastIndex].SetLineLength (highlights [lastIndex].GetStart (), highlights [lastIndex].GetEnd () + step);
                //				}

                //				this.GetStabilityPointer ().ChangeLocation (GetStabilityPointer ().GetLocalX () - step);
                this.GetStabilityPointer().ChangeLocation(GetStabilityPointer().GetLocalX() - step);

                this.GetStabilityPointer().UpdateLabel(
                    (int)(this.GetStabilityPointer().GetLocalX() * this.GetStabilityPointer().GetDenominator() / this.GetStabilityPointer().GetLength()),
                    (int)GetDenominator());
                yield return null;
            }



            // Ensure that the stability pointer is pointing to the right value
            //			this.GetStabilityPointer ().PointTo (newNumerator, newDenominator);
            // ADDED +1
            //this.GetStabilityPointer().PointTo(newNumerator, newDenominator, GetMaxPoint()+1);
            // ADDED MAX CHANGE but was working
            this.GetStabilityPointer().PointTo(newNumerator, newDenominator, GetMaxPoint());
            int lastIndex = highlights.Count - 1;

            // Add stepcount to last index or ensure it points to proper place
            if (lastIndex >= 0) {
                //				this.highlights [lastIndex].SetLineLength (highlights [lastIndex].GetStart(), highlights [lastIndex].GetEnd());
                this.highlights[lastIndex].SetLineLength(highlights[lastIndex].GetStart(),
                    this.GetStabilityPointer().GetLocalX());
            }

            //			StabilityCheck ();
            this.highlights.RemoveAt(startIndex - 1);
            Destroy(highlight.gameObject);
            //this.highlights.Remove(highlight);
            //Destroy(highlight.gameObject); // ADDED
            yield return null;
        }

        this.isSubtracting = false;
        this.isProcessingHighlight = false;
        yield return null;
    }

    public float GetHighlightEnd() {
        if (this.highlights.Count > 0) {
            return this.highlights[this.highlights.Count - 1].GetEnd();
        }
        else {
            return 0f;
        }
    }

    public int GetTargetNumerator() {
        return this.targetNumerator;
    }

    public int GetTargetDenominator() {
        return this.targetDenominator;
    }

    public bool IsManipulating() {
        return this.isManipulating;
    }

    // Move the pointer and the necessary highlight dependencies
    //public IEnumerator ReconfigureHighlights() {

    //    List<Highlight> oldHighlightList = new List<Highlight>(this.highlights);
    //    Debug.Log("<color = red>OLD HIGHLIGHT SIZE IS " + oldHighlightList.Count + "</color>");

    //    // Remove all highlight entry
    //    for (int i = 0; i < this.highlights.Count; i++) {
    //        ManipulateStability(this.highlights[i].GetNumerator(), this.highlights[i].GetDenominator(), this.highlights[i].GetColor(), false);
    //    }

    //    // Add all highlight entries to be proportional
    //    for (int i = 0; i < oldHighlightList.Count; i++) {
    //        ManipulateStability(
    //            oldHighlightList[i].GetNumerator(),
    //            oldHighlightList[i].GetDenominator(),
    //            oldHighlightList[i].GetColor(), true);
    //    }
    //    while (this.isManipulating) {
    //        yield return null;
    //    }
    //    this.StabilityCheck();
    //}
}
