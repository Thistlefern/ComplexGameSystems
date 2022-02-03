using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;

public class PlayerController : MonoBehaviour
{
    public PlayerInput input;

    public float moveSpeed;
    public float rotateSpeed;
    public float jumpHeight;

    private Vector2 m_Rotation;
    private Vector2 m_Move;
    private float m_Select;

    public bool hasJumped;
    public bool isFalling;
    public float storedRot;

    public Rigidbody rbody;
    public new Collider collider;
    public Animator animator;

    public PlayerInventory inventory;
    public UI uI;
    public Crafting craftScript;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
        animator.SetBool("IsMoving", true);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        m_Rotation = context.ReadValue<Vector2>();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        m_Select = context.ReadValue<float>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasJumped && collision.collider.CompareTag("Ground"))
        {
            hasJumped = false;
            animator.SetBool("IsJumping", false);
        }
    }

    private void OnEnable()
    {
        input.currentActionMap["Jump"].performed += InputJump;  // TODO* maybe change all input to this later but it works for now (remember to clean up if you add things here)
        input.currentActionMap["Interact"].performed += InputInteract;
        input.currentActionMap["Sort"].performed += InputSort;
        input.currentActionMap["Craft"].performed += InputCraft;
        input.currentActionMap["NoUICraft"].performed += InputNoUICraft;
    }
    private void OnDisable()
    {
        input.currentActionMap["Jump"].performed -= InputJump;
        input.currentActionMap["Interact"].performed -= InputInteract;
        input.currentActionMap["Sort"].performed -= InputSort;
        input.currentActionMap["Craft"].performed -= InputCraft;
        input.currentActionMap["NoUICraft"].performed -= InputNoUICraft;
    }

    public void InputJump(InputAction.CallbackContext obj)
    {
        if (!hasJumped && !uI.currentlyCrafting)
        {
            rbody.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
            hasJumped = true;
            animator.SetBool("IsJumping", true);
        }
    }
    public void InputInteract(InputAction.CallbackContext obj)
    {
        inventory.PickUpItem();
    }

    public void InputSort(InputAction.CallbackContext obj)
    {
        inventory.Sort();
    }

    public void InputCraft(InputAction.CallbackContext obj)
    {
        if(uI != null)
        {
            if (uI.currentlyCrafting)
            {
                uI.craftingMenu.SetActive(false);
                uI.invPanel.SetActive(true);
                uI.pickupText.gameObject.SetActive(true);
                uI.currentlyCrafting = false;
            }
            else
            {
                uI.craftingMenu.SetActive(true);
                uI.invPanel.SetActive(false);
                uI.pickupText.gameObject.SetActive(false);
                uI.currentlyCrafting = true;
                uI.CheckRequirementsUI(craftScript.craftID);
            }
        }
    }

    public void InputNoUICraft(InputAction.CallbackContext obj)
    {
        craftScript.Craft();
    }

    private void Start()
    {
        for(int i = 0; i < inventory.craftableItems.Length; i++)
        {
            inventory.craftableItems[i].player = inventory; // TODO figure out why this is here
        }
    }

    public void Update()
    {
        if(uI != null)
        {
            if (!uI.currentlyCrafting)
            {
                Move(m_Move);
                Rotate(m_Rotation);
                Select(m_Select);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
        else
        {
            Move(m_Move);
            Rotate(m_Rotation);
            Select(m_Select);
        }
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

    private void Select(float direction)
    {
        if(direction != 0)
        {
            if(direction < 0)
            {
                uI.SelectDown();
            }
            else
            {
                uI.SelectUp();
            }
        }
    }
}