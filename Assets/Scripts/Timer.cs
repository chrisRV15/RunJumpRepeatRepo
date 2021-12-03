using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    private float timep;
    private float sec;
    private float min;
    public float startTimer;

    void Start()
    {
        startTimer = 1;
    }
    void Update()
    {
        if (startTimer > 0)
        {
            timep += Time.deltaTime;
            min = (int)(timep / 60);
            sec = (timep % 60);


            timer.text = min.ToString("00") + ":" + sec.ToString("f2");
        }
    }


}