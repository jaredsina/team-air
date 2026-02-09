using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButton : MonoBehaviour
{
    public Button MainMenu;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start Menu");
        Debug.Log("Main Menu Loaded");
    }
    void Start()
    {
        MainMenu.onClick.AddListener(LoadMainMenu);
    }
}