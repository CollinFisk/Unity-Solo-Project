using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpheight = 10.0f;

    public Vector2 moveInput = Vector2.zero;
    PlayerInput input;
    Rigidbody Rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      input = GetComponent<PlayerInput>();
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}