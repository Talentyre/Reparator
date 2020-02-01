using UnityEngine;

public class StateHidden : FSMState
{
	static float s_revealChanceOnPlayerNear = 0.2f;
	static float s_revealChanceOnPlayerNearIncr = 0.15f;

	Prop _prop;
	float _sqrRevealDistance;
	float _sqrbulletInstantRevealDistance;

	bool _playerIsNear;
	public bool PlayerIsNear
	{
		get { return _playerIsNear; }
		set
		{
			if (!_playerIsNear && value)
				TryReveal ();
			_playerIsNear = value;
		}
	}

	public StateHidden (Prop prop, float revealDistance, float bulletInstantRevealDistance)
	{
		ID = StateID.Hidden;
		_prop = prop;
		_sqrRevealDistance = revealDistance * revealDistance;
		_sqrbulletInstantRevealDistance = bulletInstantRevealDistance * bulletInstantRevealDistance;
	}

	void TryReveal ()
	{
		if (Random.Range (0.0f, 1.0f) < s_revealChanceOnPlayerNear + s_revealChanceOnPlayerNearIncr)
			_prop.Unhide (true);
		else
			s_revealChanceOnPlayerNear += s_revealChanceOnPlayerNearIncr;
	}

	public override void OnShot ()
	{
		_prop.Unhide (false);
	}

	public override void Act (Transform player, Transform npc)
	{
		PlayerIsNear = Vector3.SqrMagnitude (player.position - npc.position) <= _sqrRevealDistance;
	}

	public override void Reason (Transform player, Transform npc)
	{

	}
}