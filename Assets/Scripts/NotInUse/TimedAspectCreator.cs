using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAspectCreator : MonoBehaviour
{
    public GameObject prefab;
    public TimedAspect aspect;

    public void CreateTimedAspect(int cycleType)
    {
        Instantiate(prefab);
    }
}