using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class StartPuzzles : MonoBehaviour
{
    public Button start_button;
    public void StartGame()
    {
        SceneManager.LoadScene("PuzzleScene");
        Debug.Log("Puzzles initiated");
    }
    void Start()
    {
        start_button.onClick.AddListener(StartGame);
    }
}
