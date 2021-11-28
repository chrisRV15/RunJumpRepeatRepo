using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    public float airMultiplier = 0.4f;
    float x;
    float z;
    public Transform orientation;

    Vector3 moveDirection;

    //Player modifications
    private float playerHeight = 2f;

    //Ground Dectection
    public bool isGrounded;
    public LayerMask whatIsGround;
    private float groundDistance = 0.4f;

    //Drag
    private float groundDrag = 6f;
    private float airDrag = 1f;

    //Camera
    public Transform playerCam;
    private float xRotation;
    public float sensitivity = 10f;
    private float sensMultiplier = 0.01f;
    private float yRotation;

    //Jump
    public float jumpForce = 10f;


    //RigidBody
    private Rigidbody rb;



    //Sliding

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        //Core Movement / Input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        moveDirection = orientation.forward * x + orientation.right * z;

        //Jumping cheking
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, whatIsGround);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        //Caliing drag control 
        dragControl();

        //Calling Look
        Look();
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    //Using Fixed update because is better for physics movement
    void FixedUpdate()
    {
        movingPlayer();
    }

    //Adding Force to the rigidbody
    void movingPlayer()
    { 
        if(isGrounded)
        {
        rb.AddForce(moveDirection.normalized* moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);

        }
    }

    //Controlling Drag
    void dragControl()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    //Jump Function
    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    

    //Looking Function
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRotation += mouseX * sensitivity * sensMultiplier;
        xRotation -= mouseY * sensitivity * sensMultiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    }
}

