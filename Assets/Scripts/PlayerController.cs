using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int currentSpeed;
    public int walkSpeed;
    public int runSpeed;
    public Rigidbody rbody;
    public float jumpHeight;
    public Animator animator;
    public bool gameOver;
    //public bool gameStarted;

    public bool isMoving;
    public bool isRunning;
    public bool isFalling;
    public float stamina;
    readonly int staminaMax = 5;

    public bool grounded;

    public float timer;

    void Start()
    {
        //gameStarted = false;

        isFalling = false;
        stamina = staminaMax;

        grounded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {   
        // TODO adjust to fit current game

        //if (rbody.velocity.y < Mathf.Abs(0.01f))
        //{
        //    grounded = true;
        //    isFalling = false;
        //    if (collision.collider.name != "Plane")
        //    {
        //        audio.source.PlayOneShot(audio.sounds[2]);
        //    }
        //    else
        //    {
        //        audio.source.PlayOneShot(audio.sounds[0]);
        //    }
        //}
    }

    void FixedUpdate()
    {
        if (!gameOver)  // TODO && gameStarted
        {
            Vector3 tVel = Vector3.zero;
            float tempY = rbody.velocity.y;

            if (Input.GetKey(KeyCode.LeftShift) && isMoving && stamina != 0)
            {
                isRunning = true;
                currentSpeed = runSpeed;
                animator.SetBool("IsRunning", true);
            }
            else
            {
                isRunning = false;
                currentSpeed = walkSpeed;
                animator.SetBool("IsRunning", false);
            }

            if (Input.GetKey(KeyCode.W))
            {
                tVel += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                tVel -= transform.forward;
            }

            //if (Input.GetKey(KeyCode.W))
            //{
            //    tVel += transform.forward;
            //    isMoving = true;
            //    if (Input.GetKey(KeyCode.A))
            //    {
            //        transform.rotation = Quaternion.Euler(rotNW);
            //    }
            //    else if (Input.GetKey(KeyCode.D))
            //    {
            //        transform.rotation = Quaternion.Euler(rotNE);
            //    }
            //    else
            //    {
            //        transform.rotation = Quaternion.Euler(rotN);
            //    }
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    tVel += transform.forward;
            //    isMoving = true;
            //    if (Input.GetKey(KeyCode.A))
            //    {
            //        transform.rotation = Quaternion.Euler(rotSW);
            //    }
            //    else if (Input.GetKey(KeyCode.D))
            //    {
            //        transform.rotation = Quaternion.Euler(rotSE);
            //    }
            //    else
            //    {
            //        transform.rotation = Quaternion.Euler(rotS);
            //    }
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    isMoving = true;
            //    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            //    {
            //        tVel += transform.forward;
            //        transform.rotation = Quaternion.Euler(rotW);
            //    }
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    isMoving = true;
            //    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            //    {
            //        tVel += transform.forward;
            //        transform.rotation = Quaternion.Euler(rotE);
            //    }
            //}
            //if (Input.GetKey(KeyCode.Space) && grounded)
            //{
            //    isMoving = false;
            //    rbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            //    grounded = false;
            //}

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                isMoving = false;
            }


            if (isMoving)
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }

            if (rbody.velocity.y < 0)
            {
                isFalling = true;
            }

            tVel = tVel.normalized * currentSpeed;
            tVel.y = tempY;
            rbody.velocity = tVel;
        }
        else
        {
            rbody.velocity = new Vector3(0, 0);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            isMoving = false;
        }
    }

    private void Update()
    {
        if (!gameOver)
        {
            // TODO win state/scene loader here if needed

            timer += Time.deltaTime;

            if (isRunning)
            {
                stamina -= Time.deltaTime;
                if (stamina < 0)
                {
                    stamina = 0;
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    stamina += Time.deltaTime;
                    if (stamina > staminaMax)
                    {
                        stamina = staminaMax;
                    }
                }
            }
        }
    }
}