using UnityEngine;

public class Node
{
    public Vector2 position;
    public int gridX, gridY;
    public int gCost;
    public int fCost => gCost;
    public Node parent;

    public Node(Vector2 pos, int x, int y)
    {
        position = pos;
        gridX = x;
        gridY = y;
        gCost = int.MaxValue;
    }
}
