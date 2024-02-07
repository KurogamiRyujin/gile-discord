using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour to control display of notification on game completion.
/// </summary>
public class CongratulationsCanvas : MonoBehaviour {
    /// <summary>
    /// Reference to UI element under this game object.
    /// </summary>
    [SerializeField] private CongratulationsChild congratulations;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
    void Awake() {
        EventBroadcaster.Instance.AddObserver(EventNames.SHOW_CONGRATULATIONS, Show);
    }

    /// <summary>
    /// Standard Unity Function. Called once in the component's lifetime to jumpstart behaviour.
    /// </summary>
    void Start() {
        this.Hide();
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.SHOW_CONGRATULATIONS);
    }

    /// <summary>
    /// Show the UI.
    /// </summary>
    public void Show() {
        SoundManager.Instance.Play(AudibleNames.Room.ITEM_GET, false);
        this.GetCongratulations().LoadSkin();
        this.GetCongratulations().gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the UI.
    /// </summary>
    public void Hide() {
        this.GetCongratulations().gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns the UI child element.
    /// </summary>
    /// <returns>The UI Child element.</returns>
    private CongratulationsChild GetCongratulations() {
        if (this.congratulations == null) {
            this.congratulations = GetComponentInChildren<CongratulationsChild>();
        }
        return this.congratulations;
    }
}
