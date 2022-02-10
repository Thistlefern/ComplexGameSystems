using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;

public class PlayerController : MonoBehaviour
{
    // TODO* seperate from UI if possible
    // TODO* fix grossness later in script with crafting items, see comments

    public PlayerInput input;

    public bool gameIsPaused;

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
    public UI ui;
    public Crafting craftScript;

    public bool itemInRange;
    public GameObject item;
    public bool noRoom;
    int count;

    public Item[] resources;

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

    public void TestFunction()
    {
        Debug.Log("Use this for testing a function.");
    }

    public void PauseUnpause()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 1;
            gameIsPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gameIsPaused = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void InputJump(InputAction.CallbackContext obj)
    {
        if (!hasJumped && !ui.currentlyCrafting && !gameIsPaused)
        {
            rbody.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
            hasJumped = true;
            animator.SetBool("IsJumping", true);
        }
    }
    public void InputInteract(InputAction.CallbackContext obj)
    {
        if(item != null && !gameIsPaused)
        {
            if(item.GetComponent<Item>().type.ToString() == "Resource")
            {
                PickUpItem(item.GetComponent<Item>(), 1);
            }
        }
    }

    public void InputUseTool(InputAction.CallbackContext obj)
    {
        if (item != null && !gameIsPaused)
        {
            if (item.GetComponent<Item>().type.ToString() == "Source")
            {
                switch (item.GetComponent<Item>().resourceType.GetComponent<Item>().itemName)
                {
                    case "stone":
                        if (inventory.itemSlots[ui.selectedItem].itemName == "pickaxe")
                        {
                            int count = 0;
                            for (int j = 0; j < inventory.itemSlots.Length; j++)
                            {
                                if (inventory.itemSlots[j] != null)
                                {
                                    if (item.GetComponent<Item>().resourceType.GetComponent<Item>().itemName == inventory.itemSlots[j].itemName) // check all slots for the item being picked up
                                    {
                                        inventory.itemQuantities[j] += 3;
                                        itemInRange = false;
                                        Destroy(item);
                                        item = null;
                                        ui.UpdateSpritesAndQuantities(j);
                                        return;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    count++;
                                }
                                if (count == inventory.itemSlots.Length)
                                {
                                    PickUpItem(resources[0], 3);    // gross, if this was part of my actual system I would change this but it works for only having a few resources
                                    itemInRange = false;
                                    ui.UpdateSpritesAndQuantities(j);
                                }
                            }
                        }
                        break;
                    case "wood":
                        if (inventory.itemSlots[ui.selectedItem].itemName == "axe")
                        {
                            int count = 0;
                            for (int j = 0; j < inventory.itemSlots.Length; j++)
                            {
                                if (inventory.itemSlots[j] != null)
                                {
                                    if (item.GetComponent<Item>().resourceType.GetComponent<Item>().itemName == inventory.itemSlots[j].itemName) // check all slots for the item being picked up
                                    {
                                        inventory.itemQuantities[j] += 3;
                                        itemInRange = false;
                                        Destroy(item);
                                        item = null;
                                        ui.UpdateSpritesAndQuantities(j);
                                        return;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    count++;
                                }
                                if (count == inventory.itemSlots.Length)
                                {
                                    PickUpItem(resources[1], 3);    // gross, if this was part of my actual system I would change this but it works for only having a few resources
                                    itemInRange = false;
                                    ui.UpdateSpritesAndQuantities(j);
                                }
                            }
                        }
                        break;
                    case "grass":
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void InputSort(InputAction.CallbackContext obj)
    {
        if (!gameIsPaused)
        {
            inventory.Sort();
            for (int d = 0; d < inventory.maxItems; d++)
            {
                ui.UpdateSpritesAndQuantities(d);
            }
        }
    }

    public void InputCraft(InputAction.CallbackContext obj)
    {
        if(ui != null && !gameIsPaused)
        {
            if (ui.currentlyCrafting)
            {
                ui.craftingMenu.SetActive(false);
                ui.invPanel.SetActive(true);
                ui.pickupText.gameObject.SetActive(true);
                ui.currentlyCrafting = false;
            }
            else
            {
                ui.craftingMenu.SetActive(true);
                ui.invPanel.SetActive(false);
                ui.pickupText.gameObject.SetActive(false);
                ui.currentlyCrafting = true;
                ui.CheckRequirementsUI(ui.craftID);
            }
        }
    }

    public void InputNoUICraft(InputAction.CallbackContext obj)
    {
        if (!gameIsPaused)
        {
            craftScript.Craft(0);       // will always craft the item in slot 0, which in this case is an axe
        }
    }

    private void Start()
    {
        gameIsPaused = false;
        itemInRange = false;
        item = null;
        count = 0;
    }

    public void Update()
    {
        if (!gameIsPaused)
        {
            if(ui != null)
            {
                if (!ui.currentlyCrafting)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        itemInRange = true;
        item = other.gameObject;

        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i] != null)
            {
                if (other.GetComponent<Item>().itemName == inventory.itemSlots[i].itemName) // check all slots for the item being picked up
                {
                    inventory.slotToAddTo = i + 1;   // if you already have one, select the slot it is in
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
                if (!inventory.firstEmptyFound)   // keep track of the first empty slot that the player has
                {
                    inventory.firstEmptyFound = true;
                    inventory.firstEmpty = i;
                }
            }
        }

        if (count == inventory.itemSlots.Length)    // if no slots have this item, go back to the first empty slot
        {
            if (!inventory.firstEmptyFound)
            {
                inventory.invFull = true;
            }
            else
            {
                inventory.slotToAddTo = inventory.firstEmpty + 1;  // if there is an empty slot, select that slot
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange = false;
        item = null;
        inventory.slotToAddTo = 0;
        inventory.firstEmptyFound = false;
        count = 0;
    }

    public void PickUpItem(Item thing, int quant)
    {
        if (itemInRange && !gameIsPaused)
        {
            if (inventory.invFull)
            {
                Debug.Log("Inventory is full");
            }
            else
            {
                if (inventory.slotToAddTo != 0)
                {
                    for (int i = 0; i < inventory.allPossibleItems.Length; i++)
                    {
                        if (inventory.allPossibleItems[i].itemName == thing.itemName)
                        {
                            inventory.itemSlots[inventory.slotToAddTo - 1] = inventory.allPossibleItems[i];
                            inventory.itemQuantities[inventory.slotToAddTo - 1] += quant;
                        }
                    }
                }

                ui.AddItem();
                itemInRange = false;
                inventory.slotToAddTo = 0;
                inventory.firstEmptyFound = false;
                count = 0;
                Destroy(item);
                item = null;
            }
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
                ui.SelectUp();
            }
            else
            {
                ui.SelectDown();
            }
        }
    }
}