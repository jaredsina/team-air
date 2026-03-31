using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class NewGame : MonoBehaviour
{
    public Button NewGame_button;
    public SquareLogic sl;
    public void New_Game()
    {
        sl.CreateLevel();
        Debug.Log("New Game Started");
    }
    void Start()
    {
        NewGame_button.onClick.AddListener(New_Game);
    }
}