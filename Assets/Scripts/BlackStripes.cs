using UnityEngine;

public class BlackStripes : MonoBehaviour
{
	[SerializeField] Animator _animator = null;

	void Awake ()
	{
		Prop.SawByPlayer += OnPropSawByPlayer;
		Prop.Revealed += OnPropRevealed;
	}

	void OnPropRevealed ()
	{
		_animator.SetTrigger ("Out");
	}

	void OnPropSawByPlayer ()
	{
		_animator.SetTrigger ("In");
	}
}