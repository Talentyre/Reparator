using UnityEngine;

public class StateCaught : FSMState
{
	public StateCaught (Prop prop)
	{
		ID = StateID.Caught;
	}

	public override void OnShot ()
	{
		
	}

	public override void Act (Transform player, Transform npc)
	{
		
	}

	public override void Reason (Transform player, Transform npc)
	{
		
	}
}