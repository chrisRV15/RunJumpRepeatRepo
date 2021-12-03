using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private float bounceHeight = 100f;

    private void onCollisionEnter(Collision collision)
    {
        GameObject block = collision.gameObject;
        Rigidbody rb = block.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * bounceHeight, ForceMode.Impulse);

    }
}
