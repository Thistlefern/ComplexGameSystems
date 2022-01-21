using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class NaughtyComponent : MonoBehaviour
{
    [Dropdown("StringValues")]
    public string stringValue;

    private List<string> StringValues { get { return new List<string>() { "Go Until", "Back and Forth" }; } }   // Add any other types of cycles here
}