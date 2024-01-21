using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour
{
    public Transform target;

    public Color pathColor;

    public float iAmSpeed = 10;

    private Vector3[] pathPoints;

    private int index;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position,
                                         OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool flag)
    {
        if (flag)
        {
            index = 0;
            
            pathPoints = newPath;
            
            StopCoroutine("FollowPath");
            
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 current = pathPoints[0];

        while (true)
        {
            if (transform.position == current)
            {
                ++ index;

                if (index - pathPoints.Length >= 0)
                {
                    index = 0;

                    pathPoints = new Vector3[0];
                    
                    yield break;
                }

                current = pathPoints[index];
            }

            transform.position = Vector3.MoveTowards(transform.position, current,
                                iAmSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (pathPoints != null)
        {
            for (int i = index; i < pathPoints.Length; ++ i)
            {
                Gizmos.color = pathColor;

                Gizmos.DrawCube(pathPoints[i], Vector3.one);

                if (i == index)
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
