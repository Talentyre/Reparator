﻿using UnityEngine;

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
	[SerializeField] SpriteRenderer _spriteRenderer = null;

	[Header ("Stats")]
	[SerializeField] float _moveSpeed = 5;
	[SerializeField] float _speedLostOnShot = 1.5f;
	[SerializeField] float _playerRevealDistance = 2;
	[SerializeField] float _bulletInstantRevealDistance = 1;
	public HideoutType HideoutType = HideoutType.NA;

	[Header ("Pathfinding")]
	public Room CurrentRoom = null;
	public Hideout CurrentHideout = null;

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

		StateHiding hiding = new StateHiding (this, _moveSpeed, _speedLostOnShot, _animator, _spriteRenderer);
		hiding.AddTransition (Transition.FoundHideout, StateID.Hidden);

		StateCaught caught = new StateCaught (this);
		caught.AddTransition(Transition.MiniGameLost, StateID.Hiding);
		 
		_fsm = new FSMSystem (hidden, revealing, hiding);
	}

	public void OnNearShot (Vector2 shootPosition)
	{
		Debug.Log ("near shot");
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

	public abstract void OnShotParticles ();

	public virtual void Awake ()
	{
		m_Transform = transform;
		_player = FindObjectOfType<PlayerController> ().transform;
		Bullet.HitCollider += OnBulletHitCollider;

		if (HideoutType == HideoutType.NA)
			Debug.LogWarning ("(Prop) NA Hideout type is not allowed.", gameObject);
		if (CurrentRoom == null)
			Debug.LogWarning ("(Prop) CurrentRoom var is null.", gameObject);
		if (CurrentHideout == null)
			Debug.LogWarning ("(Prop) CurrentHideout var is null.", gameObject);

		CurrentHideout.Available = false;
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
		// TODO death anim, sprite and so on...
	}
}