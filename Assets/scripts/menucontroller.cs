using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        // Replace "GameScene" with the exact name of your main game scene
        SceneManager.LoadScene("Gamescene");
    }
}
