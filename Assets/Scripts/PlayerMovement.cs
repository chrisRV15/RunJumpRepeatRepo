using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    public float moveSpeed = 4500f;
    private float movementMultiplier = 10f;
    float x;
    float z;
    public bool isGrounded;
    public Transform orientation;
    public LayerMask WhatGround;

    Vector3 moveDirection;

    //Camera
    public Transform playerCam;
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    private float desiredX;

    //RigidBody
    private Rigidbody rb;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.15f;
    public float jumpForce = 550f;
    bool jumping;

    //Sliding
    private Vector3 normalVector = Vector3.up;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        r
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Core Movement
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        jumping = Input.GetKeyDown(KeyCode.Space);

        Vector3 moveDirection = orientation.forward * z + orientation.right * x;

        //Calling other functions
        Look();

    }

    //
    void FixedUpdate()
    {
        Movement();
    }


    void Movement()
    {

        //multipliers for movement
        float multiplier = 1f;
        float multiplierV = 1f;

        //Adding Forces
        rb.AddForce(orientation.transform.forward * z * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);

        //Gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Ready to jump
        if (readyToJump && jumping)
        {
            Jump();
        }

        //In air
        if (!isGrounded)
        {
            multiplier = 0.05f;
            multiplierV = 0.05f;
        }

    }

    //Looking with the camera
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Looking for current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;



        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        transform.rotation = Quaternion.Euler(0, desiredX, 0);
    }

    //Jumping function
    private void Jump()
    {
        if (isGrounded && readyToJump)
        {
            readyToJump = false;

            //JumpForces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //While falling
            Vector3 veloc = rb.velocity;
            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(veloc.x, 0, veloc.z);
            }
            else if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(veloc.x, veloc.y / 2, veloc.z);
            }
            //Reseting y velocity

            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void resetJump()
    {
        readyToJump = true;
    }

}

