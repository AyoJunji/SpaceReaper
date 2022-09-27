using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShipTimer : MonoBehaviour
{
    [SerializeField] LevelHandler levelHandler;

    public float currentTime = 90f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {

            if (levelHandler.CheckLevelOne == true)
            {
                SceneManager.LoadScene("Level 2");
            }

            if (levelHandler.CheckLevelOne == true && levelHandler.CheckLevelTwo == true)
            {
                SceneManager.LoadScene("Level 3");
            }

            if (levelHandler.CheckLevelThree == true)
            {
                SceneManager.LoadScene("Win Scene");
            }
        }

        DisplayTime(currentTime);
    }


    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("Time Remaining: {0:00}:{1:00}", minutes, seconds);
    }
}
