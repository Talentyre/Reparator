using UnityEngine;
using RSTools.Extensions;

public class Node : MonoBehaviour, IHeapItem<Node>
{
	int _heapIndex;

	public int HeapIndex
	{
		get { return _heapIndex; }
		set { _heapIndex = value; }
	}

	public Transform Transform { get; private set; }
	public Vector2 Position { get { return Transform.position; } }

	public Node[] Neighbours;
	[HideInInspector] public Node Parent;

	[HideInInspector] public float Cost;
	[HideInInspector] public float Heuristic;
	public float FCost { get { return Cost + Heuristic; } }

	void Awake ()
	{
		Transform = transform;
	}

	public void ResetNode ()
	{
		Parent = null;
		Cost = 0;
		Heuristic = 0;
	}

	[ContextMenu ("Get Near Neighbours")]
	void GetNeighbours ()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll (transform.position, 2);
		var neighbours = new System.Collections.Generic.List<Node> ();
		foreach (var c in cols)
			if (c.TryGetComponent (out Node n))
				neighbours.Add (n);

		Neighbours = neighbours.ToArray ();
	}

	public virtual void OnDrawGizmos ()
	{
		Gizmos.color = Color.white;
		if (Neighbours != null && Neighbours.Length != 0)
			foreach (Node n in Neighbours)
				Gizmos.DrawLine (
					transform.position + (n.transform.position - transform.position).NormalNormalized (Vector3Extensions.Axis.XY, true) * 0.05f,
					n.transform.position - (transform.position - n.transform.position).NormalNormalized (Vector3Extensions.Axis.XY, true) * 0.05f);
	}

	public int CompareTo (Node other)
	{
		int compare = FCost.CompareTo (other.FCost);
		if (compare == 0)
			compare = Heuristic.CompareTo (other.Heuristic);

		return -compare;
	}
}