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
    public float reducedHeight;

    //Ground Dectection
    public bool isGrounded;
    public LayerMask whatIsGround;
    private float groundDistance = 0.4f;
    public Transform groundChecking;

    //Drag
    private float groundDrag = 3f;
    private float airDrag = 1f;

    //Camera
    public Transform playerCam;
    private float xRotation;
    public float sensitivity = 10f;
    private float sensMultiplier = 0.01f;
    private float yRotation;

    //Jump
    public float jumpForce = 10f;


    //RigidBody / Collider
    private Rigidbody rb;
    private CapsuleCollider collider;

    //Slopes
    RaycastHit slope;
    Vector3 slopeDirection;


    //Dash
    public float buncyHeight;


    //Sliding
    public float slidespeed = 10f;
    public bool isSliding;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        collider = GetComponent<CapsuleCollider>();
        playerHeight = collider.height;

    }

    void Update()
    {
        //Core Movement / Input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        moveDirection = orientation.forward * x + orientation.right * z;

        //Jumping cheking
        isGrounded = Physics.CheckSphere(groundChecking.position, groundDistance, whatIsGround);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        //Caliing drag control 
        dragControl();

        //Slope Checking
        slopeDirection = Vector3.ProjectOnPlane(moveDirection, slope.normal);

        //Calling Look
        Look();
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);

        //Slide checking
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Sliding();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            goUp();
        }
    }

    //Using Fixed update because is better for physics movement
    void FixedUpdate()
    {
        movingPlayer();
    }

    //Adding Force to the rigidbody
    void movingPlayer()
    { 
        if(isGrounded && !onSlope())
        {
            isSliding = true;
            rb.AddForce(moveDirection.normalized* moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            isSliding = false;
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);

        }
        else if (isGrounded && onSlope())
        {
            isSliding = true;
            rb.AddForce(slopeDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
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
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    //Slope function
    private bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slope, playerHeight / 2 + 0.5f))
        {
            if (slope.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            return false;
    }

    //Sliding function
    void Sliding()
    {
        if (isSliding)
        {
            collider.height = reducedHeight;
            rb.AddForce(moveDirection.normalized * slidespeed, ForceMode.VelocityChange);
        }
    }

    //After sliding
    void goUp()
    {
        collider.height = playerHeight;
    }
    
    //Jump Pad
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("JumpPad"))
        {
            rb.AddForce(transform.up * buncyHeight, ForceMode.Impulse);
        }
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

