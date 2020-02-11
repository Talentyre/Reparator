using UnityEngine;

public class VFXDestroy : IPoolable
{
	public override void Reset ()
	{

	}

	public void SelfDestroy () => gameObject.SetActive (false);
}