using UnityEngine;

public class StateHidden : FSMState
{
	Prop _prop;
	float _sqrRevealDistance;

	public StateHidden (Prop prop, float revealDistance)
	{
		ID = StateID.Hidden;
		_prop = prop;
		_sqrRevealDistance = revealDistance * revealDistance;
	}

	public override void DoBeforeLeaving ()
	{
		_prop.OnSawByPlayer ();
	}

	public override void Act (Transform player, Transform npc)
	{
		// attendre
		// random interaction si le joueur met sa vie
		// shake si le joueur approche?
	}

	public override void Reason (Transform player, Transform npc)
	{
		if (Vector3.SqrMagnitude (player.position - npc.position) <= _sqrRevealDistance)
			_prop.SetTransition (Transition.SawByPlayer);
	}
}