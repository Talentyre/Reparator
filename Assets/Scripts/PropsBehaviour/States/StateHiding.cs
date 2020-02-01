﻿using UnityEngine;

public class StateHiding : FSMState
{
	Prop _prop;
	float _moveSpeed;

	public StateHiding (Prop prop, float moveSpeed)
	{
		ID = StateID.Hiding;
		_prop = prop;
		_moveSpeed = moveSpeed;
	}

	public override void OnShot ()
	{
		Debug.Log ("Getting shot while running away.");
	}

	public override void Act (Transform player, Transform npc)
	{
		Debug.Log ("Running away.");
		// suivre waypoints
	}

	public override void Reason (Transform player, Transform npc)
	{
		// checker si la destination est atteinte
		//    checker si le joueur est proche ou non
		//    se cacher ou non etc
	}

	public override void OnTriggerEnter2D(Collider2D collider2D)
	{
		base.OnTriggerEnter2D(collider2D);

		GameController.Instance.SpawnMiniGame(_prop);
	}
}