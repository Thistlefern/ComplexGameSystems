using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedAspect : MonoBehaviour
{
    public float currentCyclePercent;   // the current percentage of how far through the cycle the aspect is (0 = start, 1 = end)

    public bool cycleCompleted;         // has the cycle completed? on a back and forth, this will shift between true and false when the cycle turns over
    [Tooltip("How many hours until a cycle change")]
    public float cycleSpeed;            // how many hours until a cycle change

    public bool hasSlider;              // will this aspect use a UI slider?
    public Slider currentPercentSlider; // Unity UI slider to show the current percentage of the cycle completed (will go up and down on a back and forth aspect)

    public bool isOcean;
    public float lowTide;
    public float highTide;
    public float currentTide;

    public bool grows;

    public bool backAndForth;

    void Start()
    {
        currentCyclePercent = 0;
        cycleCompleted = false;
        // dropdown.stringValue = "Go Until";
    }
}