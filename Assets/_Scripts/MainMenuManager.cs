using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        }

        SceneManager.LoadScene("Running");
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited!");
        Application.Quit();
    }
}