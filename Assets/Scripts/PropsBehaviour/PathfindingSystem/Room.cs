using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	[Header ("Open Context Menu to get Hideouts")]
	public Hideout[] Hideouts;

	public Hideout GetHideoutOfType (HideoutType type)
	{
		List<Hideout> hideouts = new List<Hideout> ();
		foreach (var h in Hideouts)
			if (h.Type == type)
				hideouts.Add (h);

		return hideouts[Random.Range (0, hideouts.Count)];
	}

	[ContextMenu ("Get Hideouts (in children)")]
	void GetHideouts ()
	{
		Hideouts = GetComponentsInChildren<Hideout> ();
	}
}