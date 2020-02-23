using UnityEngine;

public class WarningSign : MonoBehaviour
{
	Animator _animator;
	SpriteRenderer _spriteRenderer;

	void Awake ()
	{
		Prop.SawByPlayer += OnPropSawByPlayer;
		Prop.Revealed += OnPropRevealed;

		_animator = GetComponent<Animator> ();
		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnPropRevealed ()
	{
		_spriteRenderer.enabled = false;
	}

	void OnPropSawByPlayer ()
	{
		_spriteRenderer.enabled = true;
		_animator.SetTrigger ("Warn");
	}

	private void OnDestroy ()
	{
		Prop.SawByPlayer -= OnPropSawByPlayer;
		Prop.Revealed -= OnPropRevealed;
	}
}