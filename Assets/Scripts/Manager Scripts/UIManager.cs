using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    public GameObject deathMenu;
    public static bool isDead;

    public GameObject settingsMenu;
    public static bool isSettingsOn;

    public GameObject mainMenu;

    [SerializeField] private HealthSO healthSO;
    public Slider healthUI;
    public TextMeshProUGUI maxHealthCount;

    public TextMeshProUGUI enemyCount;

    [SerializeField]
    private SoulsSO soulsSO;
    public TextMeshProUGUI soulsText;

    [Header("Player Input & Actions")]
    [SerializeField] public PlayerControls playerControls;
    private InputAction playerPause;

    void Awake()
    {
        playerControls = new PlayerControls();
        isDead = false;
        isPaused = false;
        isSettingsOn = false;
    }

    void Update()
    {
        if (soulsSO != null)
        {
            soulsText.text = (":" + soulsSO.Value);
        }


        if (healthUI != null)
        {
            SetHealth(healthSO.CurrentHealthValue);
            healthUI.maxValue = healthSO.MaxHealthValue;
            maxHealthCount.text = ("Max Health: " + healthSO.MaxHealthValue);
        }

        if (enemyCount != null)
        {
            enemyCount.text = ("Enemies Left: " + LevelWin.enemiesLeft);
        }
    }

    private void OnEnable()
    {
        playerPause = playerControls.Gameplay.Pause;
        playerPause.Enable();
        playerPause.performed += Pause;

        PlayerController.OnPlayerDeath += EnableDeathMenu;
    }

    private void OnDisable()
    {
        playerPause.Disable();

        PlayerController.OnPlayerDeath -= EnableDeathMenu;
    }

    public void EnableDeathMenu()
    {
        deathMenu.SetActive(true);
        isDead = true;
        Time.timeScale = 0f;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        //If player isn't in the title screen then we can pause
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "TitleScreen")
        {
            //If player isn't dead then we can pause
            if (!isDead && !isPaused)
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    //Pauses game on button press
    public void PauseGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    //Resumes game on button press
    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);

        StartCoroutine(WaitTime());
        SceneManager.LoadScene("TitleScreen");
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(WaitTime());
        SceneManager.LoadScene("TitleScreen");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isDead = false;
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);

        StartCoroutine(WaitTime());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        healthSO.CurrentHealthValue = healthSO.MaxHealthValue;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator WaitTime()
    {
        //Instantiate(fadeIn, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);

    }

    public void SetHealth(float health)
    {
        healthUI.value = health;
    }

    public void SettingsMenu()
    {
        if (isSettingsOn)
        {
            settingsMenu.SetActive(false);
            isSettingsOn = false;

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }

            if (mainMenu != null)
            {
                mainMenu.SetActive(true);
            }
        }

        else
        {
            settingsMenu.SetActive(true);
            isSettingsOn = true;

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }

            if (mainMenu != null)
            {
                mainMenu.SetActive(false);
            }
        }
    }
}
