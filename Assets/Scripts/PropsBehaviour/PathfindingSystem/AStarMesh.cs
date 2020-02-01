using System.Collections.Generic;
using UnityEngine;

public class AStarMesh : MonoBehaviour
{
	[Header ("Open Context Menu to generate mesh")]
	public Node[] Mesh;

	[ContextMenu ("Generate Mesh")]
	void GenerateMesh ()
	{
		Mesh = FindObjectsOfType<Node> ();
	}

	List<Node> Retrace (Node start, Node end)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = end;

		while (currentNode != start)
		{
			path.Add (currentNode);
			currentNode = currentNode.Parent;
		}

		path.Reverse ();
		return path;
	}

	public List<Node> FindPath (Node start, Node end)
	{
		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Add (start);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet[0];

			for (int i = 1; i < openSet.Count; i++)
				if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].Heuristic < currentNode.Heuristic)
					currentNode = openSet[i];

			openSet.Remove (currentNode);
			closedSet.Add (currentNode);

			if (currentNode == end)
				return Retrace (start, end);

			foreach (Node neighbour in currentNode.Neighbours)
			{
				if (closedSet.Contains (neighbour))
					continue;

				float costToNeighbour = currentNode.Cost + Vector3.SqrMagnitude (currentNode.transform.position - neighbour.transform.position);
				if (costToNeighbour < neighbour.Cost || !openSet.Contains (neighbour))
				{
					neighbour.Cost = costToNeighbour;
					neighbour.Heuristic = Vector3.SqrMagnitude (neighbour.transform.position - end.transform.position);
					neighbour.Parent = currentNode;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
				}
			}
		}

		return null;
	}
}