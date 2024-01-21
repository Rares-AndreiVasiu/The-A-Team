using UnityEngine;

using System.Collections;

using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public bool flagGizmos;

    public LayerMask noWalking;
    
    public Vector2 gridWorldSize;

    public float nodeRadius;

    private float nodeDiameter;
    
    private Node[,] grid;
    
    private int sizeXGrid, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
    
        sizeXGrid = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
    
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    
        CreateGrid();
    }

    public int MaxSize
    {
        get { return sizeXGrid * gridSizeY; }
    }

    private void CreateGrid()
    {
        grid = new Node[sizeXGrid, gridSizeY];
    
        Vector3 leftBot = transform.position - 
        Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < sizeXGrid; ++ x)
        {
            for (int y = 0; y < gridSizeY; ++ y)
            {
                Vector3 worldPoint = leftBot + 
                Vector3.right * (x * nodeDiameter + nodeRadius) + 
                Vector3.forward * (y * nodeDiameter + nodeRadius);
    
                bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, noWalking);
    
                grid[x, y] = new Node(isWalkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; ++ x)
        {
            for (int y = -1; y <= 1; ++ y)
            {
                if(x == 0)
                {
                    if (y == 0)
                    {
                        continue;
                    }
                }

                int newX = node.gridX + x;
    
                int newY = node.gridY + y;

                if (newX >= 0 && newX < sizeXGrid &&
                 newY >= 0 && newY < gridSizeY)
                {
                    neighbours.Add(grid[newX, newY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentageFrX = (worldPosition.x / gridWorldSize.x) + 0.5f;
    
        float percentageFrY = (worldPosition.z / gridWorldSize.y) + 0.5f;
    
        percentageFrX = Mathf.Clamp01(percentageFrX);
    
        percentageFrY = Mathf.Clamp01(percentageFrY);

        int x = Mathf.RoundToInt((sizeXGrid - 1) * percentageFrX);
    
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentageFrY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, 
        new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && flagGizmos)
        {
            foreach (Node node in grid)
			{
				if (node.isWalkable)
				{
					Gizmos.color = Color.white;
				}
				else
				{
					Gizmos.color = Color.red;
				}

				Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
        }
    }
}
