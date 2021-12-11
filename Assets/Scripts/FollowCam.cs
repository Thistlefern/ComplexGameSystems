using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    void Start()
    {

    }

    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}