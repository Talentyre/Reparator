using UnityEngine;

public static class NearestObjectGetter
{
	public static Collider2D GetNearestCollider2D (Vector3 reference, Collider2D[] compared)
	{
		Collider2D bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = reference;

		foreach (Collider2D comparedObj in compared)
		{
			Vector3 directionToTarget = comparedObj.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = comparedObj;
			}
		}

		return bestTarget;
	}
}