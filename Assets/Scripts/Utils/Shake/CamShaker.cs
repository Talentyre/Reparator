using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShaker : MonoBehaviour
{
	public static CamShaker Instance;
	Coroutine shakeCo;
	Shake shake;
	Vector3 shakePosOffset;
	Quaternion rotation;

	private void Awake ()
	{
		Instance = this;
		shake = new Shake (Shake.ShakeSettings.Default);
	}

	public void ShakeCam (float trauma, bool additiveTrauma = true)
	{
		if (additiveTrauma)
			shake.AddTrauma (trauma);
		else
			shake.SetTrauma (trauma);

		if (shakeCo == null)
			shakeCo = StartCoroutine (DoShake ());
	}

	IEnumerator DoShake ()
	{
		while (shake.Trauma > .0f)
		{
			var result = shake.ShakeTransform (transform);
			transform.rotation = result.Item2 * rotation;
			transform.position = transform.position + result.Item1;
			yield return null;
		}

		shakePosOffset = Vector3.zero;
		transform.rotation = rotation;

		shakeCo = null;
	}
}