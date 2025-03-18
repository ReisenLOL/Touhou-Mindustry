using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    private Node[,] nodes;
    private int width, height;

    public void InitializeGrid(bool[,] walkableGrid)
    {
        width = walkableGrid.GetLength(0);
        height = walkableGrid.GetLength(1);
        nodes = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new Node(x, y, walkableGrid[x, y]);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return nodes[x, y];
        }
        return null;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++) // Check adjacent tiles (up, down, left, right)
        {
            int nx = node.x + dx[i];
            int ny = node.y + dy[i];

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                neighbors.Add(nodes[nx, ny]);
            }
        }
        return neighbors;
    }
}
