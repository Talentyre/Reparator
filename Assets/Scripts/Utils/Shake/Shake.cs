using UnityEngine;

public enum CoordinateAxes : byte
{
	X = 1,
	Y = 2,
	Z = 4,
	XY = X | Y,
	XZ = X | Z,
	YZ = Y | Z,
	XYZ = X | Y | Z
}

public class Shake
{
	[System.Serializable]
	public struct ShakeSettings
	{
		public float speed;
		public float radius;
		public CoordinateAxes posAxes;

		public CoordinateAxes rotAxes;
		public float rotXMax;
		public float rotYMax;
		public float rotZMax;

		public static ShakeSettings Default
		{
			get
			{
				return new ShakeSettings()
				{
					speed = 30,
					radius = 1,
					posAxes = CoordinateAxes.XY,
					rotAxes = CoordinateAxes.XYZ,
					rotXMax = 15f,
					rotYMax = 15f,
					rotZMax = 15f
				};
			}
		}
	}

	public float Trauma { get; private set; }
	public ShakeSettings Settings { get; private set; }

	public Shake()
	{
		Settings = ShakeSettings.Default;
	}

	public Shake( ShakeSettings settings )
	{
		Settings = settings;
	}

	public void SetSettings( ShakeSettings settings )
	{
		Settings = settings;
	}

	public void SetTrauma (float newTrauma)
	{
		Trauma = Mathf.Clamp01(newTrauma);
	}

	public void AddTrauma (float newTrauma)
	{
		Trauma = Mathf.Clamp01 (Trauma + newTrauma);
	}

	public System.Tuple<Vector3, Quaternion> ShakeTransform (Transform transform)
	{
		var offsetPos = Vector3.zero;

		if((Settings.posAxes & CoordinateAxes.X) == CoordinateAxes.X)
			offsetPos += transform.right * (Mathf.PerlinNoise(Time.time * Settings.speed, 0f) - .5f) * 2f;
		if((Settings.posAxes & CoordinateAxes.Y) == CoordinateAxes.Y)
			offsetPos += transform.up * (Mathf.PerlinNoise( 0f, (Time.time + 5f) * Settings.speed) - .5f) * 2f;
		if((Settings.posAxes & CoordinateAxes.Z) == CoordinateAxes.Z)
			offsetPos += transform.forward * (Mathf.PerlinNoise( 0f, (Time.time + 10f) * Settings.speed) - .5f) * 2f;

		offsetPos *= Settings.radius * Trauma * Trauma;

		var offsetRot =
			Quaternion.Euler(
				(Settings.rotAxes & CoordinateAxes.X) != CoordinateAxes.X ? 0 : (Mathf.PerlinNoise( Time.time * Settings.speed, 0f) - .5f) * 2f * Settings.rotXMax * Trauma * Trauma,
				(Settings.rotAxes & CoordinateAxes.Y) != CoordinateAxes.Y ? 0 : (Mathf.PerlinNoise( Time.time * Settings.speed + 2, 0f) - .5f) * 2f * Settings.rotYMax * Trauma * Trauma,
				(Settings.rotAxes & CoordinateAxes.Z) != CoordinateAxes.Z ? 0 : (Mathf.PerlinNoise( Time.time * Settings.speed + 4, 0f) - .5f) * 2f * Settings.rotZMax * Trauma * Trauma );

		Trauma -= Time.deltaTime;
		if(Trauma < .0f)
			Trauma = .0f;

		return new System.Tuple<Vector3, Quaternion>( offsetPos, offsetRot );
	}
}