using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{
    
    public bool isBlocked;
    public Vector3 position;

    public int gridX, gridY;

    public int gCost;
    public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }

    int heapIndex;

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public Node parent;

    
    
    public Node(bool isBlocked, Vector3 position, int x, int y)
    {
        this.isBlocked = isBlocked;
        this.position = position;
        gridX = x;
        gridY = y;
    }

    
    public int CompareTo(Node node)
    {
        int iComp = fCost.CompareTo(node.fCost);
        if (iComp == 0)
        {
            iComp = hCost.CompareTo(node.hCost);
        }
        return ~iComp;
    }
}
