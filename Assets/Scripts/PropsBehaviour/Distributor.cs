using UnityEngine;

public class Distributor : Prop
{
	[Header ("Distributor related")]
	public GameObject OnDistributorShotParticles;

	public override void OnShotParticles ()
	{
		var go = Instantiate (OnDistributorShotParticles);
		go.transform.position = transform.position - new Vector3 (0, .5f);
	}
}