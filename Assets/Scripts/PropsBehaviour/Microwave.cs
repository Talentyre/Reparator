using UnityEngine;

public class Microwave : Prop
{
	public ObjectPool OnMicrowaveShotParticles;

	public override void OnShotParticles ()
	{
		var go = OnMicrowaveShotParticles.GetObject ();
		go.transform.position = transform.position;
	}
}