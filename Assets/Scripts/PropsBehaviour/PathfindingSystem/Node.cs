using UnityEngine;
using RSTools.Extensions;

public class Node : MonoBehaviour
{
	public Transform Transform { get; private set; }
	public Vector2 Position { get { return Transform.position; } }

	public Node[] Neighbours;
	[HideInInspector] public Node Parent;

	[HideInInspector] public float Cost;
	[HideInInspector] public float Heuristic;
	public float FCost { get { return Cost + Heuristic; } }

	public bool ShowName;

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
		/*
		if (ShowName)
			UnityEditor.Handles.Label (transform.position - new Vector3 (0, 0.5f), transform.name);
*/
		Gizmos.color = Color.white;
		if (Neighbours != null && Neighbours.Length != 0)
			foreach (Node n in Neighbours)
				Gizmos.DrawLine (
					transform.position + (n.transform.position - transform.position).NormalNormalized (Vector3Extensions.Axis.XY, true) * 0.05f,
					n.transform.position - (transform.position - n.transform.position).NormalNormalized (Vector3Extensions.Axis.XY, true) * 0.05f);
	}
}