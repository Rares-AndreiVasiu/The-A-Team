using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour
{
    static PathRequestManager instance;

	Queue<PathRequest> myQueue = new Queue<PathRequest>();

	PathRequest currentPathRequest;
	
	Pathfinding pathfinding;

	bool flagBusy;

    struct PathRequest
	{
		public Vector3 pathBegin, endPath;
		
        public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> _callback)
		{
			pathBegin = start;
		
        	endPath = end;
		
        	callback = _callback;
		}
	}

	void Awake()
	{
		pathfinding = GetComponent<Pathfinding>();
        
        instance = this;
	}

	public static void RequestPath(Vector3 pathBegin, Vector3 endPath, Action<Vector3[], bool> callback)
	{
		PathRequest newRequest = new PathRequest(pathBegin, endPath, callback);
		
        instance.myQueue.Enqueue(newRequest);
		
        instance.TryProcessNext();
	}

    public void FinishedProcessingPath(Vector3[] track, bool flag)
	{
		currentPathRequest.callback(track, flag);

		flagBusy = false;
		
        TryProcessNext();
	}

	void TryProcessNext()
	{
        if(!flagBusy)
		{
            if (myQueue.Count > 0)
		    {
                currentPathRequest = myQueue.Dequeue();

                flagBusy = true;
                
                pathfinding.StartFindPath(currentPathRequest.pathBegin, currentPathRequest.endPath);
		    }
        }
	}
}
