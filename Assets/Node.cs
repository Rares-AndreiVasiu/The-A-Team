 using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

	public bool isWalkable;

	public Vector3 worldPosition;
	
	public int gridCoordinateX, gridY;

	public int gCost, hCost;
	
	public Node parent;
	
	int heapIndex;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		isWalkable = _walkable;

		worldPosition = _worldPos;
		
        gridCoordinateX = _gridX;
		
        gridY = _gridY;
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

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

	public int CompareTo(Node nodeToCompare)
	{
		int compare = fCost.CompareTo(nodeToCompare.fCost);

        switch (compare)
        {
            case 0:
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);

                return compare;
            
                break;
            }
            
            default:
            {
                return -compare;
            }
        }
	}
}
