using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpHeight;

    private Vector2 m_Rotation;
    private Vector2 m_Move;
    private Vector2 m_Jump;
    private Vector2 m_Select;

    public bool hasJumped;
    public bool isFalling;
    public float storedRot;

    public Rigidbody rbody;
    public Collider collider;
    public Animator animator;

    public PlayerInventory inventory;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
        animator.SetBool("IsMoving", true);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        m_Rotation = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        m_Jump = context.ReadValue<Vector2>();
        animator.SetBool("IsJumping", true);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        m_Select = context.ReadValue<Vector2>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasJumped && collision.collider.tag == "Ground")
        {
            hasJumped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.GetComponent<Item>().itemName);

        // Action required pickup
        //Debug.Log("Press E to pick up " + other.GetComponent<Item>().itemName);

        // Auto pickup
        // Destroy(other.gameObject);
        int count = 0;
        int firstEmpty = 0;
        bool firstEmptyFound = false;

        for(int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i] != null)
            {
                if (other.GetComponent<Item>().itemName == inventory.itemSlots[i].itemName) // check all slots for the item being picked up
                {
                    Debug.Log(i + 1);   // if you already have one, add the one picked up to that slot
                    return;
                }
                else
                {
                    count++;    // counting the slots that don't contain the item being picked up (this one counts filled slots)
                }
            }
            else
            {
                count++;        // counting the slots that don't contain the item being picked up (this one counts empty slots)
                if (!firstEmptyFound)   // keep track of the first empty slot that the player has
                {
                    firstEmptyFound = true;
                    firstEmpty = i;
                }
            }
        }

        if (count == inventory.itemSlots.Length)    // if no slots have this item, go back to the first empty slot
        {
            if (!firstEmptyFound)
            {
                Debug.Log("No room!");  // if there is no empty slot, display a message
            }
            else
            {
                Debug.Log("First empty slot: slot " + firstEmpty);  // if there is an empty slot, put this new item there
            }
        }
    }

    public void Update()
    {
        Move(m_Move);
        Rotate(m_Rotation);
        Jump(m_Jump);
        SelectItem(m_Select);

        // Debug.Log(rbody.velocity.y);
    }

    private void Move(Vector2 direction)    // TODO* change to velocity instead of transform.position
    {
        if (direction.sqrMagnitude < 0.01)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }
    
    private void Rotate(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
        {
            return;
        }
        m_Rotation.y = storedRot;
        if(direction.x == 1)
        {
            m_Rotation.y += Time.deltaTime * rotateSpeed;
        }
        else
        {
            m_Rotation.y += Time.deltaTime * rotateSpeed * -1;
        }

        Vector2 tempVec = new Vector2(0.0f, m_Rotation.y);
        transform.localEulerAngles = tempVec;
        storedRot = m_Rotation.y;
    }

    private void Jump(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
        {
            animator.SetBool("IsJumping", false);
            return;
        }
        if (!hasJumped)
        {
            rbody.AddForce(0, jumpHeight * direction.y, 0, ForceMode.Impulse);
            hasJumped = true;
            animator.SetBool("IsJumping", true);
        }
    }

    private void SelectItem(Vector2 direction)
    {
        
    }
}