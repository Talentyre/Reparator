using UnityEngine;

public class Scanner : Prop
{
	[Header ("Scanner related")]
	public ObjectPool OnScannerShotParticles;

	public override void OnShotParticles ()
	{
		var go = OnScannerShotParticles.GetObject ();
		go.transform.position = transform.position;
	}
}