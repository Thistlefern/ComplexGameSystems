using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int curSpeed;
    public int walkSpeed;
    public int runSpeed;

    //public bool hasJumped;
    //public float jumpForce;
    // could implement a double jump if wanted

    public Rigidbody2D rbody;

    void Start()
    {
        curSpeed = walkSpeed;
        //hasJumped = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curSpeed = runSpeed;
        }
        else
        {
            curSpeed = walkSpeed;
        }

        //Vector3 tempVel = Vector3.zero;
        Vector2 tempVel = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            tempVel.y += curSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            tempVel.x -= curSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            tempVel.y -= curSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            tempVel.x += curSpeed;
        }

        rbody.velocity = tempVel;
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.Space) && !hasJumped)
        //{
        //    rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //    hasJumped = true;
        //}
        //if(rbody.velocity.y == 0)
        //{
        //    hasJumped = false;
        //}
    }
}