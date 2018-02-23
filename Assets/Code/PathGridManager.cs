using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGridManager : MonoBehaviour {

    public LayerMask obstacleMask;
    public Vector2 gridSize;

    public float halfNodeWidth;

    public Node[,] grid;
    
    public List<Node> path = new List<Node>();
    
    private void Start()
    {
        FillGrid();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
        
        if(grid == null)
        {
            FillGrid();
        }
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                if (grid[i, j].isBlocked)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                
                if(path != null)
                {
                    if(path.Contains(grid[i, j]))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                Gizmos.DrawWireCube(grid[i, j].position, new Vector3(0.9f, 0.5f, 0.9f));
            }
        }
    }
    
    public void FillGrid()
    {
        grid = new Node[(int)gridSize.x,(int)gridSize.y];

        float startPointX = ((gridSize.x / 2) - halfNodeWidth) - gridSize.x + 1;
        float startPointY = ((gridSize.y / 2) - halfNodeWidth) - gridSize.y + 1;

        Vector3 pos = new Vector3(startPointX, 0, startPointY);

        bool isBlocked = false;

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                isBlocked = Physics.CheckSphere(pos, halfNodeWidth, obstacleMask);
                grid[i, j] = new Node(isBlocked, pos, i, j);
                pos.z++;
            }

            pos.z = startPointY;
            pos.x++;
        }
    }
    
    public Node WorldToGridCoordinates(Transform transform)
    {
        int xPos = Mathf.FloorToInt(transform.position.x) + ((int)gridSize.x / 2);
        int yPos = Mathf.FloorToInt(transform.position.z) + ((int)gridSize.y / 2);
        
        return grid[xPos, yPos];
    }
    
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1 ; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if(checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}
