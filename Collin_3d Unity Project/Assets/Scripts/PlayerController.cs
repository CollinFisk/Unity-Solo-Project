using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpheight = 10.0f;
    public float jumpDetectDistance = 1.0f;
    Ray jumpRay;
    Vector2 moveInput = Vector2.zero;
    PlayerInput input;
    Rigidbody Rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        Rb = GetComponent<Rigidbody>();
        jumpRay = new Ray();
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;
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
}