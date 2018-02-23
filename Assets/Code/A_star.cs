using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_star : MonoBehaviour {

    public GameObject seeker;
    public GameObject target;

    PathGridManager grid;

    Node currentNode;

    private void Awake()
    {
        grid = GetComponent<PathGridManager>();
    }

    public int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);
    }

    public void FindPath(GameObject startPos, GameObject endPos) {
        
        Node startNode = grid.WorldToGridCoordinates(startPos.transform);
        Node endNode = grid.WorldToGridCoordinates(endPos.transform);

        //List<Node> openSet = new List<Node>();
        //List<Node> closedSet = new List<Node>();

        //Change the lists to HEAPS
        Heap<Node> openSet = new Heap<Node>(grid.grid.Length);
        Heap<Node> closedSet = new Heap<Node>(grid.grid.Length);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            //Node currentNode = openSet[0];

            //for (int i = 1; i < openSet.Count; i++)
            //{
            //    if ((openSet[i].fCost < currentNode.fCost)
            //            ||
            //        (openSet[i].fCost == currentNode.fCost)
            //            &&
            //        (openSet[i].hCost < currentNode.hCost))
            //    {
            //        currentNode = openSet[i];
            //    }
            //}
            
            //openSet.Remove(currentNode);

            //HEAP
            currentNode = openSet.RemoveFirst();

            closedSet.Add(currentNode);

            if(currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            List<Node> neighbours = grid.GetNeighbours(currentNode);
            
            foreach (Node neighbour in neighbours)
            {
                if (neighbour.isBlocked || closedSet.Contains(neighbour))
                    continue;

                int newMovementCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                if(newMovementCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCost;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    public void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        FindPath(seeker, target);
	}
}
