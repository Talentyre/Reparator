using System;
using UnityEngine;

public abstract class Prop : MonoBehaviour
{
	/// <summary> When the prop has just been seen. </summary>
	public static event System.Action SawByPlayer;
	/// <summary> When the prop has been revealed and starts running away. </summary>
	public static event System.Action Revealed;

	FSMSystem _fsm;
	public FSMSystem FSM => _fsm;

	Transform m_Transform;
	Transform _player;

	[Header ("References")]
	[SerializeField] Animator _animator = null;
	[SerializeField] SpriteRenderer _spriteRenderer = null;
	[SerializeField] GameObject _repairedSymbol = null;

	[Header ("Stats")]
	[SerializeField] float _moveSpeed = 5;
	[SerializeField] float _speedLostOnShot = 1.5f;
	[SerializeField] float _speedRecoverPerSecond = 0.5f;
	[SerializeField] float _playerRevealDistance = 2;
	[SerializeField] float _bulletInstantRevealDistance = 1;
	[SerializeField] float _chancesToBeFeared = 0.3f;
	public HideoutType HideoutType = HideoutType.NA;

	[Header ("Pathfinding")]
	public Room CurrentRoom = null;
	public Hideout CurrentHideout = null;

	void OnBulletHitCollider (Collider2D[] nearHitProps, Transform nearest, Vector2 hitPosition)
	{
		if (nearest.TryGetComponent (out Prop nearestProp))
		{
			if (nearestProp == this)
				OnShot ();
			else return;
		}

		foreach (var prop in nearHitProps)
			if (prop.TryGetComponent (out Prop p) && p == this)
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

		StateHiding hiding = new StateHiding (this, _moveSpeed, _speedLostOnShot, _speedRecoverPerSecond, _animator, _spriteRenderer);
		hiding.AddTransition (Transition.FoundHideout, StateID.Hidden);
		hiding.AddTransition (Transition.Caught, StateID.Caught);

		StateCaught caught = new StateCaught (this);
		caught.AddTransition(Transition.MiniGameLost, StateID.Hiding);
		caught.AddTransition(Transition.Repaired, StateID.Repaired);

		_fsm = new FSMSystem (hidden, revealing, hiding, caught);
	}

	public void DisableCollision ()
	{
		GetComponent<Collider2D> ().enabled = false;
		Invoke ("ReenableCollision", 1);
	}
	public void ReenableCollision ()
	{
		GetComponent<Collider2D> ().enabled = true;
	}

	public void DisableProp ()
	{
		_animator.SetTrigger ("Hide");
		_animator.SetFloat ("X", 0);
		_animator.SetFloat ("Y", 0);
		GetComponent<Collider2D> ().enabled = false;
	}

	public void OnNearShot (Vector2 shootPosition)
	{
		if (!(_fsm.CurrentState is StateHidden))
			return;

		if (UnityEngine.Random.Range (0f, 1f) < _chancesToBeFeared)
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

	public abstract void OnShotParticles ();

	public virtual void Awake ()
	{
		m_Transform = transform;
		_repairedSymbol.SetActive (false);
		_player = FindObjectOfType<PlayerController> ().transform;
		Bullet.HitCollider += OnBulletHitCollider;

		MakeFSM ();
	}

	public virtual void Update ()
	{
		_fsm.CurrentState.Reason (_player, m_Transform);
		_fsm.CurrentState.Act (_player, m_Transform);
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, _playerRevealDistance);
	}

	public void OnDead()
	{
		SetTransition (Transition.Repaired);
		DisableProp ();
		_repairedSymbol.SetActive (true);
		// TODO death anim, sprite and so on...
	}
}