using UnityEngine;

public class Room : MonoBehaviour
{
	[Header ("Hideouts (in children)")]
	public Hideout[] Hideouts;

	void Awake ()
	{
		Hideouts = GetComponentsInChildren<Hideout> ();
	}
}