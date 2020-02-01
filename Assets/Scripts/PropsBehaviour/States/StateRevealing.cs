using UnityEngine;

public class StateRevealing : FSMState
{
	Prop _prop;

	public StateRevealing (Prop prop)
	{
		ID = StateID.Revealing;
		_prop = prop;
	}

	public override void Act (Transform player, Transform npc)
	{
		// rien
	}

	public override void Reason (Transform player, Transform npc)
	{
		// rien (attendre la fin de l'anim pour transition qui se fait dans la classe de base)
	}
}