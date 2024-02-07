using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI element displaying the image for game completion notification.
/// </summary>
public class CongratulationsImage : MonoBehaviour {

    /// <summary>
    /// Reference to a player skin used to display in this element.
    /// </summary>
    [SerializeField] private GameController_v7.PlayerSkin skinType = GameController_v7.PlayerSkin.DEFAULT;

    /// <summary>
    /// Returns the player skin.
    /// </summary>
    /// <returns>The Player Skin.</returns>
    public GameController_v7.PlayerSkin GetSkin() {
        return this.skinType;
    }
}
