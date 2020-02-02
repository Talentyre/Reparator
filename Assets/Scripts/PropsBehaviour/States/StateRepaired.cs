using UnityEngine;

public class StateRepaired : FSMState
{
	Vector3 _posOnStateEntered;
	Prop _prop;

	public StateRepaired (Prop prop)
	{
		ID = StateID.Repaired;
		_prop = prop;
	}

	public override void OnShot ()
	{

	}

	public override void DoBeforeEntering ()
	{
		_posOnStateEntered = _prop.transform.position;
	}

	public override void Act (Transform player, Transform npc)
	{
		npc.transform.position = _posOnStateEntered;
	}

	public override void Reason (Transform player, Transform npc)
	{

	}
}