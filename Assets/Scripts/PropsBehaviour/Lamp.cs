using UnityEngine;

public class Lamp : Prop
{
	[Header ("Lamp related")]
	public ObjectPool OnLampShotParticles;

	public override void OnShotParticles ()
	{
		var go = OnLampShotParticles.GetObject ();
		go.transform.position = transform.position;
	}
}