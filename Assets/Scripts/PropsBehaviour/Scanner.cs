using UnityEngine;

public class Scanner : Prop
{
	[Header ("Scanner related")]
	public GameObject OnScannerShotParticles;

	public override void OnShotParticles ()
	{
		var go = Instantiate(OnScannerShotParticles);
		go.transform.position = transform.position;
	}
}