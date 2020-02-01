using UnityEditor;

public class Hideout : Node
{
	public HideoutType Type;

	public override void OnDrawGizmos ()
	{
		Handles.Label (transform.position, Type.ToString ());
		base.OnDrawGizmos ();
	}
}