using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour
{
    public Transform destination;
    public Color pathColor;

    public float movementSpeed = 10;

    private Vector3[] pathPoints;

    private int currentTargetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, destination.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            pathPoints = newPath;
            currentTargetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = pathPoints[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                currentTargetIndex++;
                if (currentTargetIndex >= pathPoints.Length)
                {
                    currentTargetIndex = 0;
                    pathPoints = new Vector3[0];
                    yield break;
                }
                currentWaypoint = pathPoints[currentTargetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movementSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (pathPoints != null)
        {
            for (int i = currentTargetIndex; i < pathPoints.Length; i++)
            {
                Gizmos.color = pathColor;

                Gizmos.DrawCube(pathPoints[i], Vector3.one);

                if (i == currentTargetIndex)
                {
                    Gizmos.DrawLine(transform.position, pathPoints[i]);
                }
                else
                {
                    Gizmos.DrawLine(pathPoints[i - 1], pathPoints[i]);
                }
            }
        }
    }
}
