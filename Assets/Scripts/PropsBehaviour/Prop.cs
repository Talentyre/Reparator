using UnityEngine;

public abstract class Prop : MonoBehaviour
{
	/// <summary> When the prop has just been seen. </summary>
	public static event System.Action SawByPlayer;
	/// <summary> When the prop has been revealed and starts running away. </summary>
	public static event System.Action Revealed;

	FSMSystem _fsm;
	Transform m_Transform;
	Transform _player;

	[SerializeField] Animator _animator = null;
	[SerializeField] float _moveSpeed = 5;
	[SerializeField] float _revealDistance = 2;

	public void SetTransition (Transition transition)
	{
		_fsm.PerformTransition (transition);
	}

	void MakeFSM ()
	{
		StateHidden hidden = new StateHidden (this, _revealDistance);
		hidden.AddTransition (Transition.SawByPlayer, StateID.Revealing);

		StateRevealing revealing = new StateRevealing (this);
		revealing.AddTransition (Transition.Revealed, StateID.Hiding);

		StateHiding hiding = new StateHiding (this, _moveSpeed);
		hiding.AddTransition (Transition.FoundHideout, StateID.Hidden);

		_fsm = new FSMSystem (hidden, revealing, hiding);
	}

	public virtual void OnSawByPlayer ()
	{
		_animator.SetTrigger ("Reveal");
		SawByPlayer?.Invoke ();
	}

	public virtual void OnRevealed ()
	{
		SetTransition (Transition.Revealed);
		Revealed?.Invoke ();
	}

	public virtual void Awake ()
	{
		m_Transform = transform;
		_player = FindObjectOfType<PlayerController> ().transform;

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
		Gizmos.DrawWireSphere (transform.position, _revealDistance);
	}
}