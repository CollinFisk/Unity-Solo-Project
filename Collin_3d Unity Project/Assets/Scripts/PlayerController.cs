using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpheight = 10.0f;
    public float jumpDetectDistance = 1.0f;
    public float interactDistance = 5f;
    
    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    Vector2 moveInput = Vector2.zero;

    public Weapon currentWeapon;

    public Transform weaponSlot;
    Camera playerCam;
    PlayerInput input;
    Rigidbody Rb;
    GameObject pickupObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        Rb = GetComponent<Rigidbody>();
        jumpRay = new Ray();
        playerCam = Camera.main;

        interactRay = new Ray();
        weaponSlot = playerCam.transform.GetChild(0);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
            {
                pickupObj = interactHit.collider.gameObject;
            }
            else pickupObj = null;
        }
        else
            pickupObj = null;
        Vector3 tempMove = Rb.linearVelocity;

        tempMove.x = moveInput.x * speed;
        tempMove.z = moveInput.y * speed;

        Rb.linearVelocity = tempMove;
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        if (Physics.Raycast(jumpRay.origin, jumpRay.direction, jumpDetectDistance))
            Rb.AddForce(transform.up * jumpheight, ForceMode.Impulse);

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pickupObj.tag == "Weapon")
            {
                pickupObj.GetComponent<Weapon>().equip(this);

                pickupObj = null;
             
            }
            else if (currentWeapon)
                Reload();
            
        }
    }
    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading) ;
                 currentWeapon.reload();
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            if (context.ReadValueAsButton())
                attacking = true;
            else
                attacking = false;
        }

        else if (context.ReadValueAsButton())
            currentWeapon.fire();
    }
}