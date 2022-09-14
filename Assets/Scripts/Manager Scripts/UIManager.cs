using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    public GameObject deathMenu;
    public static bool isDead;

    public GameObject settingsMenu;
    public static bool isSettingsOn;
    public GameObject mainMenu;

    public GameObject fadeIn;

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
            soulsText.text = ("Souls: " + soulsSO.Value);
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
        if (scene.name != "TitleScreen" && scene.name != "HubShip")
        {
            //If player isn't dead then we can pause
            if (!isDead)
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


    public void GoToHubShip()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isDead = true;
        deathMenu.SetActive(false);
        pauseMenu.SetActive(false);

        StartCoroutine(WaitTime());
        SceneManager.LoadScene("HubShip");
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
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator WaitTime()
    {
        Instantiate(fadeIn, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);

    }

    public void SettingsMenu()
    {
        if (isSettingsOn)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            isSettingsOn = false;
        }
        else
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
            isSettingsOn = true;
        }
    }
}
