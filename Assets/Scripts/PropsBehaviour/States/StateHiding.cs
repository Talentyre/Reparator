using System.Collections.Generic;
using UnityEngine;

public class StateHiding : FSMState
{
	Prop _prop;
	float _moveSpeed;

	List<Node> _pathToFollow;
	Vector2 _nextDestination;
	int _nextDestinationIndex;

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

	public override void DoBeforeEntering ()
	{
		_pathToFollow = GameController.Instance.AStarMesh.GetRandomPathToHideout (_prop.CurrentHideout, _prop.CurrentRoom, _prop.HideoutType);
		_nextDestinationIndex = 0;
		_nextDestination = _pathToFollow[_nextDestinationIndex].Position;
	}

	public override void Act (Transform player, Transform npc)
	{
		if (Vector3.Distance (_nextDestination, npc.transform.position) > 0.05f)
		{
			npc.position = Vector3.MoveTowards (npc.position, _nextDestination, Time.deltaTime * _moveSpeed);
		}
		else
		{
			_nextDestinationIndex++;
			if (_nextDestinationIndex == _pathToFollow.Count - 1)
			{
				_prop.SetTransition (Transition.FoundHideout);
				return;
			}

			_nextDestination = _pathToFollow[_nextDestinationIndex].Position;
		}
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