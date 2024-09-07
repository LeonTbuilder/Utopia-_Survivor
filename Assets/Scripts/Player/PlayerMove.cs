using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody rb;
    private Vector3 moveInput;
    private ControlerEngine controlerEngine;
    private Animator animator;

    private void Awake()
    {
        controlerEngine = new ControlerEngine();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerSpeed = 4.5f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        if (moveInput == Vector3.zero)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
    }

    void PlayerInput()
    {
        moveInput = controlerEngine.Player.MoveSet.ReadValue<Vector2>();
        moveInput.Normalize();

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);

        if (movement != Vector3.zero)
        {
            float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = targetRotation;
        }
    }

    private void OnEnable()
    {
        controlerEngine.Player.Enable();
    }

    private void OnDisable()
    {
        controlerEngine.Player.Disable();
    }
}