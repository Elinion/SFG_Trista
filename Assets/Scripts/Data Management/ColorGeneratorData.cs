using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorGeneratorData {

    /* 
     * When the tube is played, it changes its color. Set this property to
     * false to allow for deterministic sequences. 
     */
    public bool generateRandomColor;

    /*
     * Bombs can appear on tubes. When they reach the tip of the tubes,
     * they destroy the closest tile.
     */
    public bool getsABomb;

    /*
     * Two tubes can be tied together. Playing either one will also
     * trigger the other one.
     */
    public bool isTiedToOpposingTube;

    /*
     * If generateRandomColor is set to false, the color will be set
     * as defined by fixedColor.
     */
    public ColorManager.Colors fixedColor;
}
