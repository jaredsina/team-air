using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// https://medium.com/@yalcinnomercann/slash-like-a-pro-building-sword-attack-mechanics-in-unity-2d-game-devlog-5-b5769dc335e1
public class BossPathfinding2D : MonoBehaviour
{
    [Header("Grid")]
    public int gridSizeX = 40;
    public int gridSizeY = 40;
    public float cellSize = 1f;
    public Vector2 gridOffset = new Vector2(-20, -20);

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float repathRate = 0.5f;
    public float stopDistance = 2.5f;

    [Header("Target")]
    public Transform player;

    private Node[,] grid;
    private List<Node> currentPath;
    private int pathIndex;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        CreateGrid();
        StartCoroutine(ChasePlayer());
    }

    void Update()
    {
        MoveAlongPath();
    }


    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 pos = new Vector2(
                    x * cellSize + cellSize * 0.5f,
                    y * cellSize + cellSize * 0.5f
                ) + gridOffset;

                grid[x, y] = new Node(pos, x, y);
            }
        }
    }


    IEnumerator ChasePlayer()
    {
        while (true)
        {
            if (player != null &&
                Vector2.Distance(transform.position, player.position) > stopDistance)
            {
                currentPath = FindPath(transform.position, player.position);
                pathIndex = 0;
            }

            yield return new WaitForSeconds(repathRate);
        }
    }


    List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = GetNodeFromWorld(startPos);
        Node targetNode = GetNodeFromWorld(targetPos);

        foreach (Node node in grid)
        {
            node.gCost = int.MaxValue;
            node.parent = null;
        }

        startNode.gCost = 0;

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node current = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < current.fCost)
                    current = openList[i];
            }

            openList.Remove(current);
            closedList.Add(current);

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor))
                    continue;

                int newCost = current.gCost + GetDistance(current, neighbor);

                if (newCost < neighbor.gCost)
                {
                    neighbor.gCost = newCost;
                    neighbor.parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null;
    }


    void MoveAlongPath()
    {
        if (player == null) return;

        if (currentPath != null && pathIndex < currentPath.Count)
        {
            Vector2 target = currentPath[pathIndex].position;
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, target) < 0.05f)
                pathIndex++;
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }
    }


    Node GetNodeFromWorld(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOffset.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - gridOffset.y) / cellSize);

        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        return grid[x, y];
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int nx = node.gridX + x;
                int ny = node.gridY + y;

                if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY)
                    neighbors.Add(grid[nx, ny]);
            }
        }

        return neighbors;
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        return 10 * (dx + dy);
    }
}
