using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWin : MonoBehaviour
{
    [SerializeField] LevelHandler levelHandlerSO;
    [SerializeField] HealthSO healthSO;

    public static int enemiesLeft;
    public int enemyCountDebug;

    void Start()
    {
        healthSO.CurrentHealthValue = healthSO.MaxHealthValue;
    }

    void Update()
    {
        enemyCountDebug = enemiesLeft;
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
        Scene scene = SceneManager.GetActiveScene();

        if (enemiesLeft == 0)
        {
            if (scene.name == "Level 1")
            {
                levelHandlerSO.CheckLevelOne = true;
                SceneManager.LoadScene("HubShip");

            }

            if (scene.name == "Level 2")
            {
                levelHandlerSO.CheckLevelTwo = true;
                SceneManager.LoadScene("HubShip");

            }

            if (scene.name == "Level 3")
            {
                SceneManager.LoadScene("Win Scene");
            }
        }
    }
}
