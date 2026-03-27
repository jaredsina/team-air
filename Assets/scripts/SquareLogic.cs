using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SquareLogic : MonoBehaviour
{
    public bool SquaresOff = false;
    
    public bool[,] PredeterminedLevel = new bool[5, 5]
    {
        { false, true, true, false, false },
        { true, true, true, false, true },
        { false, false, false, true, false },
        { true, false, false, false, false },
        { true, false, true, false, true }
    };
    public GameObject squarePrefab;
    public Button square;
    public int countclicks = 0;
    public int timer = 0;
    Color onsquare = Color.white;
    Color offsquare = Color.black;
    int width = 5;
    int height = 5;
    float cellSize = 1f;
    public bool[,] grid;
    public SpriteRenderer[,] gridSprites;
    //LightsOutSolver solver = new LightsOutSolver();
    public void Start()
    {
        CreateLevel();
    }
    public bool[,] CreateLevel()
    {
        grid = new bool[width, height];
        gridSprites = new SpriteRenderer[width, height];
        while (LightsOutSolver.IsSolvable(grid) == false)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject square = new GameObject("Square");
                    SpriteRenderer sr = square.AddComponent<SpriteRenderer>();
                    sr.sprite = squarePrefab.GetComponent<SpriteRenderer>().sprite;
                    BoxCollider2D bc = square.AddComponent<BoxCollider2D>();
                    bc.size = sr.sprite.bounds.size;
                    sr.color = Random.value > 0.4f ? offsquare : onsquare;
                    // sr.color = PredeterminedLevel.GetValue(x, y).Equals(true) ? onsquare : offsquare;
                    gridSprites[x, y] = sr;
                    grid[x, y] = sr.color == onsquare;
                    square.transform.position = new Vector3(x * cellSize*1.1f - (width * cellSize * 0.5f) + 0.3f, y * cellSize*1.1f - (height * cellSize * 0.5f) + 0.3f, 0);
                }
            }
        }
        return grid;
    }
    public void ClickSquare(int x, int y)
    {
        Toggle(x, y);
        Toggle(x - 1, y);
        Toggle(x + 1, y);
        Toggle(x, y - 1);
        Toggle(x, y + 1);
        CheckSquares();
        countclicks++;
    }
    public void Toggle(int x, int y)
    {
        
        if (x < 0 || x > width-1 || y < 0 || y > height-1) return;
        grid[x,y] = !grid[x,y];

        gridSprites[x,y].color = grid[x,y] ? onsquare : offsquare;
    }
    public bool CheckSquares()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y]) return false;
            }
        }
        Debug.Log("You won in " + (countclicks + 1) + " clicks!");
            return true;
        
    }
}