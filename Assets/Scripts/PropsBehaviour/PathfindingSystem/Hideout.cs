using UnityEngine;
using UnityEditor;

public class Hideout : Node
{
	public HideoutType Type;

	public bool Available { get; set; } = true;
	public Room RelatedRoom { get; private set; }

	void Start ()
	{
		RelatedRoom = GameController.Instance.AStarMesh.GetRoomFromHideout (this);
	}

	public override void OnDrawGizmos ()
	{
		//Handles.Label (transform.position, Type.ToString ());
		base.OnDrawGizmos ();

		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere (transform.position, 1);
	}
}