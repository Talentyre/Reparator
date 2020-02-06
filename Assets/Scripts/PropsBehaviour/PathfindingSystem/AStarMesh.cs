using System.Collections.Generic;
using UnityEngine;

public class AStarMesh : MonoBehaviour
{
	[Header ("Open Context Menu to get rooms")]
	public Room[] Rooms;
	[Header ("Open Context Menu to generate mesh")]
	public Node[] Mesh;

	[ContextMenu ("Generate Mesh")]
	void GenerateMesh ()
	{
		Mesh = FindObjectsOfType<Node> ();
	}
	[ContextMenu ("Get Rooms")]
	void GetRooms ()
	{
		Rooms = FindObjectsOfType<Room> ();
	}

	public Room GetRoomFromHideout (Hideout hideout)
	{
		foreach (Room r in Rooms)
			foreach (Hideout h in r.Hideouts)
				if (h == hideout)
					return r;
		return null;
	}

	public List<Node> GetRandomPathToHideout (Node currentNode, Room currentRoom, HideoutType type)
	{
		List<Node> hideouts = new List<Node> ();
		while (hideouts.Count < 3)
		{
			Room r = Rooms [Random.Range (0, Rooms.Length)];
			if (r == currentRoom)
				continue;

			Hideout h = r.GetRandomHideoutOfType (type);
			if (!h.Available || hideouts.Contains (h))
				continue;

			hideouts.Add (h);
		}

		return FindPath (currentNode, hideouts[Random.Range (0, 3)]);
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

		path.Add (start);
		path.Reverse ();
		return path;
	}

	List<Node> FindPath (Node start, Node end)
	{
		Heap<Node> openSet = new Heap<Node> (Mesh.Length);
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Add (start);

		foreach (Node n in Mesh)
			n.ResetNode ();

		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveFirst ();
			closedSet.Add (currentNode);

			if (currentNode == end)
				return Retrace (start, end);

			foreach (Node neighbour in currentNode.Neighbours)
			{
				if (closedSet.Contains (neighbour))
					continue;
				if (neighbour is Hideout && neighbour != end)
					continue;

				float costToNeighbour = currentNode.Cost + Vector3.SqrMagnitude (currentNode.Position - neighbour.Position);
				if (costToNeighbour < neighbour.Cost || !openSet.Contains (neighbour))
				{
					neighbour.Cost = costToNeighbour;
					neighbour.Heuristic = Vector3.SqrMagnitude (neighbour.Position - end.Position);
					neighbour.Parent = currentNode;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
				}
			}
		}

		return null;
	}
}