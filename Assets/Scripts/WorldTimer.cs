using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    public float worldTimer;        // keeps track of real time for the purpose of in-game time
    public int intervalLength;      // number of IRL seconds before an interval of in-game time occurs
    public int gameTimeInterval;    // number of in-game minutes that pass each interval, MUST BE >=1 AND <=60
                                    // NOTE: to determine number of real-time minutes pass in an in-game 24 hours, use equation ((60/timeInterval)*24*timeSpeed)/60

    int currentHour;                // the current hour in game
    int currentMinute;              // the current minute in game

    public TMPro.TMP_Text timeText; // Unity UI text to show the current time


    public TimedAspect[] timedAspects;

    void Start()
    {
        worldTimer = 0;
        currentHour = 8;        // NOTE: can be adjusted to change start time of a day
        currentMinute = 0;

        if (currentMinute < 10)
        {
            timeText.text = "Time: " + currentHour + ":0" + currentMinute;
        }
        else
        {
            timeText.text = "Time: " + currentHour + ":" + currentMinute;
        }
    }

    void Update()
    {
        worldTimer += Time.deltaTime;

        if (worldTimer >= intervalLength)
        {
            currentMinute += gameTimeInterval;

            while (currentMinute >= 60)
            {
                currentHour++;
                currentMinute -= 60;
            }

            worldTimer = 0;

            if (currentMinute < 10)     // TODO* maybe adjust to not include 'Time:'
            {                           // TODO* adjust from 24hr clock/have the option for either
                timeText.text = "Time: " + currentHour + ":0" + currentMinute;
            }
            else
            {
                timeText.text = "Time: " + currentHour + ":" + currentMinute;
            }
        }

        for(int i = 0; i < timedAspects.Length; i++)
        {
            if (timedAspects[i].hasSlider)  // helps avoid extra errors within Unity for not assigning a slider
            {
                timedAspects[i].currentPercentSlider.value = timedAspects[i].currentCyclePercent;
            }
            
            if (timedAspects[i].isSun)
            {
                if (!timedAspects[i].goAgain)
                {
                    timedAspects[i].goAgain = true;
                }

                if (timedAspects[i].nightTime)
                {
                    if (timedAspects[i].currentTide >= timedAspects[i].dayStart && timedAspects[i].currentTide < timedAspects[i].dayEnd)
                    {
                        timedAspects[i].nightTime = false;
                        timedAspects[i].cycleSpeed = timedAspects[i].daySpeed;
                    }
                }
                else
                {
                    if (timedAspects[i].currentTide >= timedAspects[i].dayEnd)
                    {
                        timedAspects[i].nightTime = true;
                        timedAspects[i].cycleSpeed = timedAspects[i].nightSpeed;
                    }
                }

                if (timedAspects[i].nightTime)
                {
                    timedAspects[i].currentTide = timedAspects[i].currentCyclePercent * 720;
                }
                else
                {
                    timedAspects[i].currentTide = timedAspects[i].currentCyclePercent * 720;
                }
                
                Vector3 tempVec = new Vector3(timedAspects[i].currentTide, -30.0f, 0.0f);
                timedAspects[i].transform.rotation = Quaternion.Euler(tempVec);
            }

            if (timedAspects[i].isOcean)
            {
                if (!timedAspects[i].backAndForth)
                {
                    timedAspects[i].backAndForth = true;
                }
                timedAspects[i].currentTide = timedAspects[i].currentCyclePercent * (timedAspects[i].highTide - timedAspects[i].lowTide) + timedAspects[i].lowTide;
                Vector3 tempVec = new Vector3(0.0f, timedAspects[i].currentTide, 0.0f);
                timedAspects[i].transform.position = tempVec;
            }

            if (timedAspects[i].backAndForth)   // the aspect has a Back and Forth cycle
            {
                if (!timedAspects[i].cycleCompleted)
                {
                    timedAspects[i].currentCyclePercent += Time.deltaTime / (((60 / gameTimeInterval) * timedAspects[i].cycleSpeed) * intervalLength);
                    // currentCyclePercent is a fraction equaling Time.deltaTime divided by the number of IRL seconds
                    // required to reach the number of in-game time intervals needed to reach the cycleSpeed
                }
                else
                {
                    timedAspects[i].currentCyclePercent -= Time.deltaTime / (((60 / gameTimeInterval) * timedAspects[i].cycleSpeed) * intervalLength);
                }

                if (timedAspects[i].currentCyclePercent >= 1)
                {
                    timedAspects[i].cycleCompleted = true;
                }

                if (timedAspects[i].currentCyclePercent <= 0)
                {
                    timedAspects[i].cycleCompleted = false;
                }
            }
            else if (timedAspects[i].goAgain)
            {
                if (timedAspects[i].currentCyclePercent >= 1)
                {
                    timedAspects[i].cycleCompleted = true;
                }

                if (!timedAspects[i].cycleCompleted)
                {
                    timedAspects[i].currentCyclePercent += Time.deltaTime / (((60 / gameTimeInterval) * timedAspects[i].cycleSpeed) * intervalLength);
                    // currentCyclePercent is a fraction equaling Time.deltaTime divided by the number of IRL seconds
                    // required to reach the number of in-game time intervals needed to reach the cycleSpeed
                }
                else
                {
                    timedAspects[i].currentCyclePercent = 0;
                    timedAspects[i].cycleCompleted = false;
                }
            }
            else
            {
                if (!timedAspects[i].cycleCompleted)
                {
                    timedAspects[i].currentCyclePercent += Time.deltaTime / (((60 / gameTimeInterval) * timedAspects[i].cycleSpeed) * intervalLength);
                    // currentCyclePercent is a fraction equaling Time.deltaTime divided by the number of IRL seconds
                    // required to reach the number of in-game time intervals needed to reach the cycleSpeed
                }

                if (timedAspects[i].currentCyclePercent >= 1)
                {
                    timedAspects[i].cycleCompleted = true;
                }
            }
        }
    }
}