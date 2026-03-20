using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class Settings : MonoBehaviour
{
    public Button settings_button;
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
        Debug.Log("Settings Opened");
    }
    void Start()
    {
        settings_button.onClick.AddListener(OpenSettings);
    }
}