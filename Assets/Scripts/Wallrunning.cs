using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{
    //
    public Transform orientation;

    //Wall running Variables
    private float wallDistance = 0.5f;
    private float minimiumJumpHeight = 1.5f;
    private bool wallLeft = false;
    private bool wallRight = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        checkingWall();
    }

    void checkingWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, wallDistance);
    }
    
    //The oppsite of checking Wall function
    private bool wallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimiumJumpHeight);
    }
}
