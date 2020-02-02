using System.Collections.Generic;
using UnityEngine;

public class StateHiding : FSMState
{
	Prop _prop;
	Animator _animator;
	SpriteRenderer _spriteRenderer;

	float _moveSpeed;
	public float MoveSpeed
	{
		get { return _moveSpeed; }
		private set
		{
			_moveSpeed = value < 0 ? 0 : value;
		}
	}

	float _baseMoveSpeed;
	float _speedLostOnShot;

	List<Node> _pathToFollow;
	Vector2 _nextDestination;
	int _nextDestinationIndex;

	public StateHiding (Prop prop, float moveSpeed, float speedLostOnShot, Animator animator, SpriteRenderer spriteRenderer)
	{
		ID = StateID.Hiding;
		_prop = prop;

		_baseMoveSpeed = moveSpeed;
		MoveSpeed = moveSpeed;
		_speedLostOnShot = speedLostOnShot;

		_animator = animator;
		_spriteRenderer = spriteRenderer;
	}

	public override void OnShot ()
	{
		CamShaker.Instance.ShakeCam (0.38f);
		_prop.OnShotParticles ();
		MoveSpeed -= _speedLostOnShot;
	}

	public override void DoBeforeEntering ()
	{
		_pathToFollow = GameController.Instance.AStarMesh.GetRandomPathToHideout (_prop.CurrentHideout, _prop.CurrentRoom, _prop.HideoutType);
		_nextDestinationIndex = 0;
		_nextDestination = _pathToFollow[_nextDestinationIndex].Position;

		_prop.CurrentHideout.Available = true;
		MoveSpeed = _baseMoveSpeed;
	}

	public override void Act (Transform player, Transform npc)
	{
		if (Vector3.Distance (_nextDestination, npc.transform.position) > 0.05f)
			MoveToNextWaypoint ();
		else
			OnWaypointReached ();
	}

	public override void Reason (Transform player, Transform npc)
	{
		if (_nextDestinationIndex == _pathToFollow.Count)
			OnDestinationReached ();
	}

	void MoveToNextWaypoint ()
	{
		_prop.transform.position = Vector3.MoveTowards (_prop.transform.position, _nextDestination, Time.deltaTime * MoveSpeed);

		Vector3 direction = (Vector2)_prop.transform.position - _nextDestination;
		var angleFromRight = Vector2.Angle (Vector2.right, direction);
		float x = angleFromRight < 45 ? 1 : angleFromRight > 135 ? -1 : 0;
		float y = (angleFromRight > 45 && angleFromRight < 135) ? -Mathf.Sign (direction.y) : 0;
		_animator.SetFloat ("X", x);
		_animator.SetFloat ("Y", y);
		_spriteRenderer.flipX = x < 0;
	}

	void OnWaypointReached ()
	{
		_nextDestinationIndex++;

		if (_nextDestinationIndex == _pathToFollow.Count)
			return;

		if (_nextDestinationIndex < _pathToFollow.Count)
			_nextDestination = _pathToFollow[_nextDestinationIndex].Position;

		//if (PlayerController.PlayerCurrentRoom == (_pathToFollow[_pathToFollow.Count - 1] as Hideout).RelatedRoom)
		//{
		//	_pathToFollow = GameController.Instance.AStarMesh.GetRandomPathToHideout (_pathToFollow[_nextDestinationIndex], _prop.CurrentRoom, _prop.HideoutType);
		//	_nextDestinationIndex = 0;
		//	_nextDestination = _pathToFollow[_nextDestinationIndex].Position;
		//}
	}

	void OnDestinationReached ()
	{
		(_pathToFollow[_pathToFollow.Count - 1] as Hideout).Available = false;
		_prop.CurrentHideout = _pathToFollow[_pathToFollow.Count - 1] as Hideout;
		_prop.CurrentRoom = _prop.CurrentHideout.RelatedRoom;
		_prop.SetTransition (Transition.FoundHideout);

		_animator.SetTrigger ("Hide");
		_animator.SetFloat ("X", 0);
		_animator.SetFloat ("Y", 0);
	}

	public override void OnTriggerEnter2D(Collider2D collider2D)
	{
		base.OnTriggerEnter2D(collider2D);
		GameController.Instance.SpawnMiniGame(_prop);
	}
}