using System;

using UnityEngine;

using System.Collections;

using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    private PathRequestManager managerRequirement;

    private Grid grille;
    
    bool flag = false;
    
    private void Awake()
    {
        managerRequirement = GetComponent<PathRequestManager>();

        grille = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 initialPos, 
                            Vector3 endPos)
    {
        StartCoroutine(FindPath(initialPos, endPos));
    }

    private IEnumerator FindPath(Vector3 posStart, 
                                Vector3 posEnd)
    {
    Vector3[] points = new Vector3[0];

    Node initialNode = grille.NodeFromWorldPoint(posStart);

    Node endNode = grille.NodeFromWorldPoint(posEnd);

    if (initialNode.IsWalkable && endNode.IsWalkable)
    {
        Heap<Node> open = new Heap<Node>(grille.MaxSize);

        HashSet<Node> closed = new HashSet<Node>();
            
        open.Add(initialNode);

        while (open.Count > 0)
        {
            Node current = open.RemoveFirst();

            closed.Add(current);

            if (current == endNode)
            {
                flag = true;

                break;
            }

            foreach (Node neighbour in grille.GetNeighbours(current))
            {
                if (!neighbour.IsWalkable 
                || closed.Contains(neighbour))
                {
                    continue;
                }

                int moveCostNeighbor = current.GCost + 
                    GetDistance(current, neighbour);
                    
                if (moveCostNeighbor < neighbour.GCost 
                    || !open.Contains(neighbour))
                {
                    neighbour.GCost = moveCostNeighbor;

                    neighbour.HCost = GetDistance(neighbour, endNode);
                        
                    neighbour.Parent = current;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                    else
                    {
                        open.UpdateItem(neighbour);
                    }
                }
            }
        }

        yield return null;

        if (flag)
        {
            points = RetracePath(initialNode, endNode);
        }

        managerRequirement.FinishedProcessingPath(points, flag);
    }

    private Vector3[] RetracePath(Node initialNode, Node endNode)
    {
        Node current = endNode;

        List<Node> track = new List<Node>();

        while (current != initialNode)
        {
            track.Add(current);
        
            current = current.Parent;
        }

        Vector3[] points = SimplifyPath(track);
        
        Array.Reverse(points);
        
        return points;
    }

    private Vector3[] SimplifyPath(List<Node> track)
    {
        Vector2 oldDIR = Vector2.zero;
        
        List<Vector3> points = new List<Vector3>();

        for (int i = 1; i < track.Count; i++)
        {
            Vector2 directionNew = new Vector2(track[i - 1].gridCoordinateX - track[i].gridCoordinateX, 
                                   track[i - 1].gridCoordinateY - track[i].gridCoordinateY);
        
            if (directionNew != oldDIR)
            {
                points.Add(track[i].WorldPosition);
            }
        
            oldDIR = directionNew;
        }

        return points.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridCoordinateX - nodeB.gridCoordinateX);
        
        int dstY = Mathf.Abs(nodeA.gridCoordinateY - nodeB.gridCoordinateY);

        // we consider the distance between two nodes to be 1, and the diagonal ones 
        // to have sqrt(2), from pythagora, and for the sake of simplicity we multiplied by ten each of them
        return 14 * Mathf.Min(dstX, dstY) +
               10 * Mathf.Abs(dstX - dstY);
    }
}
