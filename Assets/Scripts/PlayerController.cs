using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // TODO** seperate from UI if possible

    public PlayerInput input;

    public bool gameIsPaused;

    Vector3 resetPos = new Vector3(0.0f, 5.0f, 0.0f);
    public bool reseting;

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
    public GameObject dropPos;      // where an item prefab is dropped when DropItem is called

    public PlayerInventory inventory;
    public UI ui;
    public Crafting craftScript;
    public int quantToDrop;         // what quantity of an item should drop when DropItem is called?

    public bool itemInRange;
    public GameObject item;
    public bool noRoom;

    public new Camera camera;

    public GraphicRaycaster graphicRaycaster;

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

        if (collision.collider.CompareTag("Ground"))
        {
            reseting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int count = 0;

        if (other.CompareTag("Ocean"))
        {
            rbody.position = resetPos;
            reseting = true;
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsJumping", false);
        }
        else
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

    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange = false;
        item = null;
        inventory.slotToAddTo = 0;
        inventory.firstEmptyFound = false;
    }

    private void OnEnable()
    {
        input.currentActionMap["Jump"].performed += InputJump;  // TODO** maybe change all input to this later but it works for now (remember to clean up if you add things here)
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

    public void TestFunction(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            Debug.Log("Use this for testing a function.");
        }
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
        if(item != null && !gameIsPaused && itemInRange)
        {
            if(item.GetComponent<Item>().type.ToString() == "Resource")
            {
                inventory.PickUpItem(item.GetComponent<Item>(), 1);
                ui.AddItem();
                itemInRange = false;
                inventory.slotToAddTo = 0;
                inventory.firstEmptyFound = false;
                Destroy(item);
                item = null;
            }
        }
    }

    public void InputDropItem(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            for(int i = 0; i < inventory.allPossibleItems.Length; i++)
            {
                if(inventory.itemSlots[ui.selectedItem] != null)
                {
                    if(inventory.allPossibleItems[i].itemName == inventory.itemSlots[ui.selectedItem].itemName)
                    {
                        for(int j = 0; j < quantToDrop; j++)
                        {
                            Instantiate(inventory.allPossibleItems[i], dropPos.transform.position, transform.rotation);
                        }
                    }
                }
            }

            inventory.DropItem(ui.selectedItem, quantToDrop);
            ui.UpdateSpritesAndQuantities(ui.selectedItem);
        }
    }

    public void InputUseTool()
    {
        //Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        //Physics.Raycast(ray, out RaycastHit hit);
        //Debug.Log(hit.transform.name);

        //Debug.Log(graphicRaycaster.gameObject.GetComponentInChildren<GameObject>().name);


        string correctTool = "";

        if (item != null && !gameIsPaused)
        {
            if (item.GetComponent<Item>().type.ToString() == "Source")
            {
                if(inventory.itemSlots[ui.selectedItem] != null && item.GetComponent<Item>().resourceType != null)
                {
                    for (int i = 0; i < inventory.craftableItems.Length; i++)
                    {
                        if (inventory.craftableItems[i].GetComponent<Item>().specialty.ToString() == item.GetComponent<Item>().resourceType.GetComponent<Item>().itemName)
                        {
                            correctTool = inventory.craftableItems[i].GetComponent<Item>().itemName;
                        }
                    }

                    if (inventory.itemSlots[ui.selectedItem].itemName.ToString() == correctTool)
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
                                    inventory.firstEmptyFound = false;
                                    inventory.slotToAddTo = 0;
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
                                inventory.PickUpItem(item.GetComponent<Item>().resourceType.GetComponent<Item>(), 3);
                                itemInRange = false;
                                Destroy(item);
                                item = null;
                                inventory.firstEmptyFound = false;
                                inventory.slotToAddTo = 0;
                                for(int i = 0; i < inventory.itemSlots.Length; i++)
                                {
                                    ui.UpdateSpritesAndQuantities(i);
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Wrong tool");
                        // TODO* wrong tool indication in UI
                    }
                }
            }
        }
    }

    public void InputSort(InputAction.CallbackContext obj)
    {
        if (!gameIsPaused)
        {
            inventory.Sort(inventory.maxItems);
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
                ui.backpackUI.SetActive(false);
                ui.hotbarUI.SetActive(true);
                ui.pickupText.gameObject.SetActive(true);
                ui.currentlyCrafting = false;
            }
            else
            {
                ui.craftingMenu.SetActive(true);
                ui.backpackUI.SetActive(true);
                ui.hotbarUI.SetActive(false);
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
        reseting = false;
        itemInRange = false;
        item = null;
    }

    public void Update()
    {
        if (!gameIsPaused && !reseting)
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
    private void Move(Vector2 direction)    // TODO** change to velocity instead of transform.position
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