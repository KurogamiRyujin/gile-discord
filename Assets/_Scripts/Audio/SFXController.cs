using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller that handles all sound effects
/// </summary>
public class SFXController : MonoBehaviour {
    /// <summary>
    /// Reference to the slider in the Options Popup 
    /// </summary>
    [SerializeField] Slider slider;
    /// <summary>
    /// Reference to the SoundManager script
    /// </summary>
    SoundManager soundManager;

    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start() {
        soundManager = FindObjectOfType<SoundManager>();
        UpdateSliderValue(1);
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Called on slider value change
    /// </summary>
    public void OnSliderValueChanged() {
        UpdateSliderValue(slider.value);
    }

    /// <summary>
    /// Updates the value of the slider
    /// </summary>
    /// <param name="sliderValue">Value to be assigned to the slider</param>
    void UpdateSliderValue(float sliderValue) {
        Debug.LogError("UPDATED SFX " + sliderValue);
        slider.value = sliderValue;
        foreach(SFX sfx in soundManager.sfxList) {
            if(sfx != null)
                sfx.GetAudioSource().volume = sliderValue;
        }
    }
}
