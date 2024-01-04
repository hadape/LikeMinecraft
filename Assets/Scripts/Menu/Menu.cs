using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool gamePaused = false;
    [SerializeField]
    private GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1.0f;
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(Constants.MAIN);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(Constants.MAIN_MENU);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RespawnPlayer()
    {
        //TODO: event for placing player to active chunk
    }
}
