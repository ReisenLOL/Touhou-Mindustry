using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int x, y;
    public int gCost, hCost;
    public Node parent;

    public Node(int x, int y, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
        parent = null;
    }
    public int fCost => gCost + hCost;
}

