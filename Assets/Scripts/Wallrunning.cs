using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{
    //
    public Transform orientation;

    //Wall running Variables
    private float wallDistance = 1f;
    private float minimiumJumpHeight = 1.5f;
    private bool wallLeft = false;
    private bool wallRight = false;
    private float wallGravity = 1f;
    private float wallJumpForce = 6f;
    Vector3 jumpDirection;

    //RigidBody
    private Rigidbody rb;

    //Raycast to detect
    RaycastHit leftWallRay;
    RaycastHit rightWallRay;

    //Audio
    public AudioClip jumpSound;
    private AudioSource playerAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        checkingWall();

        if(wallRun())
        {
            if (wallLeft)
            {
                startWallRun();
            }
            else if(wallRight)
            {
                startWallRun();
            }
            else
            {
                stopWallRun();
            }
        }
        else
        {
            stopWallRun();
        }

    }

    void checkingWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallRay, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallRay, wallDistance);
    }
    
    //The oppsite of checking Wall function
    private bool wallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimiumJumpHeight);
    }

    void startWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallGravity, ForceMode.Force);

        //Checking if the player jump and which it jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                jumpDirection = transform.up + leftWallRay.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(jumpDirection * wallJumpForce * 100, ForceMode.Force);
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }
            else if(wallRight)
            {
                jumpDirection = transform.up + rightWallRay.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(jumpDirection * wallJumpForce * 100, ForceMode.Force);
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }
        }
    }

    void stopWallRun()
    {
        rb.useGravity = true;
    }
}
