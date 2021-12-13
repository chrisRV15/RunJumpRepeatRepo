using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public AudioClip jumpSound;
    private AudioSource playerAudio;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter(Collision other)
    {
      if(other.gameObject.tag == "Player")
        {
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }
}
