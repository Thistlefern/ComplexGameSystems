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
    //public float burstSpeed;
    //public GameObject projectile;

    //private bool m_Charging;
    private Vector2 m_Rotation;
    //private Vector2 m_Look;
    private Vector2 m_Move;
    //private Vector2 m_Jump;

    public bool hasJumped;
    public float storedRot;

    public Rigidbody rbody;
    public Animator animator;

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
        animator.SetBool("IsJumping", true);
    }

    //public void OnLook(InputAction.CallbackContext context)
    //{
    //    m_Look = context.ReadValue<Vector2>();
    //}

    //public void OnFire(InputAction.CallbackContext context)
    //{
    //    switch (context.phase)
    //    {
    //        case InputActionPhase.Performed:
    //            if (context.interaction is SlowTapInteraction)
    //            {
    //                StartCoroutine(BurstFire((int)(context.duration * burstSpeed)));
    //            }
    //            else
    //            {
    //                Fire();
    //            }
    //            m_Charging = false;
    //            break;

    //        case InputActionPhase.Started:
    //            if (context.interaction is SlowTapInteraction)
    //                m_Charging = true;
    //            break;

    //        case InputActionPhase.Canceled:
    //            m_Charging = false;
    //            break;
    //    }
    //}

    //public void OnGUI()
    //{
    //    if (m_Charging)
    //        GUI.Label(new Rect(100, 100, 200, 100), "Charging...");
    //}

    public void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        //Look(m_Look);
        Move(m_Move);
        Rotate(m_Rotation);
        //Jump();
    }

    private void Move(Vector2 direction)
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
            var scaledMoveSpeed = jumpHeight * Time.deltaTime;
            // For simplicity's sake, we just keep movement in a single plane here. Rotate
            // direction according to world Y rotation of player.
            var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.up;
            transform.position += move * scaledMoveSpeed;
            hasJumped = true;
        }
    }

    //private void Look(Vector2 rotate)
    //{
    //    if (rotate.sqrMagnitude < 0.01)
    //        return;
    //    var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
    //    m_Rotation.y += rotate.x * scaledRotateSpeed;
    //    m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
    //    transform.localEulerAngles = m_Rotation;
    //}

    //private IEnumerator BurstFire(int burstAmount)
    //{
    //    for (var i = 0; i < burstAmount; ++i)
    //    {
    //        Fire();
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //private void Fire()
    //{
    //    var transform = this.transform;
    //    var newProjectile = Instantiate(projectile);
    //    newProjectile.transform.position = transform.position + transform.forward * 0.6f;
    //    newProjectile.transform.rotation = transform.rotation;
    //    const int size = 1;
    //    newProjectile.transform.localScale *= size;
    //    newProjectile.GetComponent<Rigidbody>().mass = Mathf.Pow(size, 3);
    //    newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
    //    newProjectile.GetComponent<MeshRenderer>().material.color =
    //        new Color(Random.value, Random.value, Random.value, 1.0f);
    //}
}