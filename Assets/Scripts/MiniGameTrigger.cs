using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.TryGetComponent (out Prop p))
		{
			if (!(p.FSM.CurrentState is StateHiding))
				return;

			GameController.Instance.SpawnMiniGame (p);
			p.SetTransition (Transition.Caught);
		}
	}
}