using UnityEngine;

public class Lamp : Prop
{
	[Header ("Lamp related")]
	public GameObject OnLampShotParticles;

	public override void OnShotParticles ()
	{
		var go = Instantiate(OnLampShotParticles);
		go.transform.position = transform.position;
	}
}