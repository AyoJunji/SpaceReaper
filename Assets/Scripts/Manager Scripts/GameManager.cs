using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private SoulsSO soulsSO;
    [SerializeField] private AbilitiesSO abilitiesSO;
    [SerializeField] private LevelHandler levelHandlerSO;
    [SerializeField] private HealthSO healthSO;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TitleScreen")
        {
            healthSO.CurrentHealthValue = healthSO.MaxHealthValue;
            soulsSO.Value = 0;
            abilitiesSO.CheckBubbleShield = false;
            abilitiesSO.CheckDash = false;
            abilitiesSO.CheckThrow = false;
            levelHandlerSO.CheckLevelOne = false;
            levelHandlerSO.CheckLevelTwo = false;
            levelHandlerSO.CheckLevelThree = false;
        }
    }

    public void StartGame()
    {

        SceneManager.LoadScene("Level 1");
    }
}
