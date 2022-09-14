using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private SoulsSO soulsSO;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TitleScreen")
        {
            soulsSO.Value = 0;
        }
    }

    public void StartGame()
    {

        SceneManager.LoadScene("GameScene");
    }
}
