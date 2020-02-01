using UnityEngine;

public class BlackStripes : MonoBehaviour
{
	[SerializeField] Animator _animator;

	void Awake ()
	{
		Prop.SawByPlayer += OnPropSawByPlayer;
		Prop.Revealed += OnPropRevealed;
	}

	void OnPropRevealed ()
	{
		_animator.SetTrigger ("In");
	}

	void OnPropSawByPlayer ()
	{
		_animator.SetTrigger ("Out");
	}
}