using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
public class DetectClicks : MonoBehaviour
{
    private int x;
    private int y;
    public SquareLogic squareLogic;
    private Collider2D cd;
    public void Init(SquareLogic sl, int ix, int iy)
    {
        cd = GetComponent<Collider2D>();
        squareLogic = sl;
        x = ix;
        y = iy;
    }
    void Update()
    {
        Vector3 mousePos;
        mousePos = Mouse.current.position.ReadValue();
        Vector3 worldpos3 = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        worldpos3.z = transform.position.z;
        Vector2 worldpos = new Vector2(worldpos3.x, worldpos3.y);
        Collider2D hit = Physics2D.OverlapPoint(worldpos);
        if (Mouse.current.leftButton.wasPressedThisFrame && hit != null && hit == cd)
        {
            squareLogic.ClickSquare(x, y);
        }
    }

}