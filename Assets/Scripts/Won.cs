using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Won : MonoBehaviour
{

    public GameObject wonPanel;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            setTimer(0);
            wonPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    void setTimer(int time)
    {
        Timer playerTimer = this.GetComponent<Timer>();
        playerTimer.startTimer = time;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}