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
    }

    void Update()
    {
        soulsText.text = ("Souls: " + soulsSO.Value);
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

        SceneManager.LoadScene("TitleScreen");
    }


    public void GoToHubShip()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isDead = true;
        deathMenu.SetActive(false);
        pauseMenu.SetActive(false);

        SceneManager.LoadScene("HubShip");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isDead = false;
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
