using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainMenuManager : MonoBehaviour
{
    // This function will be called by the Start button
    public void StartGame()
    {
        // Loads your game scene. 
        // Make sure the name matches your game scene exactly!
        SceneManager.LoadScene("Running"); 
    }

    // Optional: A function to quit the game
    public void QuitGame()
    {
        Debug.Log("Game Exited!");
        Application.Quit();
    }
}