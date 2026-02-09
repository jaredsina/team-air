using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class QuitApp : MonoBehaviour
{
    public Button quit_button;
    public void QuitGame()
    {
        // quit app
        Application.Quit();
        Debug.Log("Game Quit");
    }
    void Start()
    {
        quit_button.onClick.AddListener(QuitGame);
    }
}