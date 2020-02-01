using UnityEngine;
using System.Linq;

public abstract class Prop : MonoBehaviour
{
	/// <summary> When the prop has just been seen. </summary>
	public static event System.Action SawByPlayer;
	/// <summary> When the prop has been revealed and starts running away. </summary>
	public static event System.Action Revealed;

	FSMSystem _fsm;
	Transform m_Transform;
	Transform _player;

	[Header ("References")]
	[SerializeField] Animator _animator = null;

	[Header ("Stats")]
	[SerializeField] float _moveSpeed = 5;
	[SerializeField] float _playerRevealDistance = 2;
	[SerializeField] float _bulletInstantRevealDistance = 1;

	[Header ("Pathfinding")]
	[SerializeField] Room _currentRoom = null;

	void OnBulletHitCollider (Collider2D[] nearHitProps, Transform nearest, Vector2 hitPosition)
	{
		if (nearest.TryGetComponent (out Prop nearestProp) && nearestProp == this)
		{
			OnShot ();
			return;
		}

		foreach (var prop in nearHitProps)
			if (prop.TryGetComponent (out Prop p))
				p.OnNearShot (hitPosition);
	}

	public void SetTransition (Transition transition)
	{
		_fsm.PerformTransition (transition);
	}

	void MakeFSM ()
	{
		StateHidden hidden = new StateHidden (this, _playerRevealDistance, _bulletInstantRevealDistance);
		hidden.AddTransition (Transition.SawByPlayer, StateID.Revealing);
		hidden.AddTransition (Transition.Revealed, StateID.Hiding);

		StateRevealing revealing = new StateRevealing (this);
		revealing.AddTransition (Transition.Revealed, StateID.Hiding);

		StateHiding hiding = new StateHiding (this, _moveSpeed);
		hiding.AddTransition (Transition.FoundHideout, StateID.Hidden);

		StateCaught caught = new StateCaught (this);
		caught.AddTransition(Transition.MiniGameLost, StateID.Hiding);
		
		_fsm = new FSMSystem (hidden, revealing, hiding);
	}

	public void OnNearShot (Vector2 shootPosition)
	{
		if (!(_fsm.CurrentState is StateHidden))
			return;

		Unhide (Vector2.Distance (shootPosition, transform.position) >= _bulletInstantRevealDistance);
	}

	public void OnShot ()
	{
		_fsm.CurrentState.OnShot ();
	}

	public virtual void Unhide (bool instantly)
	{
		if (instantly)
		{
			SetTransition (Transition.SawByPlayer);
			SawByPlayer?.Invoke ();
			_animator.SetTrigger ("Unhide");
		}
		else
		{
			SetTransition (Transition.Revealed);
			Revealed?.Invoke ();
		}
	}

	public virtual void Reveal ()
	{
		SetTransition (Transition.Revealed);
		Revealed?.Invoke ();
	}

	public virtual void Awake ()
	{
		m_Transform = transform;
		_player = FindObjectOfType<PlayerController> ().transform;
		Bullet.HitCollider += OnBulletHitCollider;

		if (_currentRoom == null)
			Debug.LogWarning ("(Prop) CurrentRoom var is null", gameObject);

		MakeFSM ();
	}

	public virtual void Update ()
	{
		_fsm.CurrentState.Reason (_player, m_Transform);
		_fsm.CurrentState.Act (_player, m_Transform);
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, _playerRevealDistance);
	}

	public void OnDead()
	{
		// TODO death anim, sprite and so on...
	}
}