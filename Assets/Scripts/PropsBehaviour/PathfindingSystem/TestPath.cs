using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPath : MonoBehaviour
{
	public Node start;
	public Node end;

	public AStarMesh aStarMesh;
	List<Node> path;

	void Start()
    {
		path = new List<Node> ();
		path = aStarMesh.FindPath (start, end);
	}

	void OnDrawGizmos ()
	{
		if (path != null)
		{
			Gizmos.color = Color.yellow;

			for (int i = 0; i < path.Count - 1; i++)
			{
				Gizmos.DrawLine (path[i].transform.position, path[i + 1].transform.position);
			}
		}
	}
}
