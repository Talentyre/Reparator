using UnityEngine;

public class Lamp : Prop
{
	[Header ("Lamp related")]
	public GameObject OnLampShotParticles;

	public override void OnShotParticles ()
	{
		var go = Instantiate(OnLampShotParticles, transform);
		go.transform.position = transform.position;
	}
}