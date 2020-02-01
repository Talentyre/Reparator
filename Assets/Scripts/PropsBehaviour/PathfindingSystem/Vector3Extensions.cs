namespace RSTools
{
	namespace Extensions
	{
		using Math = System.Math;
		using UnityEngine;

		public static class Vector3Extensions
		{
			public enum Axis
			{
				XY,
				YZ,
				XZ
			}

			#region With

			/// <summary>
			/// Set the x component of the vector.
			/// </summary>
			public static Vector3 WithX (this Vector3 v, float x)
			{
				return new Vector3 (x, v.y, v.z);
			}

			/// <summary>
			/// Set the y component of the vector.
			/// </summary>
			public static Vector3 WithY (this Vector3 v, float y)
			{
				return new Vector3 (v.x, y, v.z);
			}

			/// <summary>
			/// Set the z component of the vector.
			/// </summary>
			public static Vector3 WithZ (this Vector3 v, float z)
			{
				return new Vector3 (v.x, v.y, z);
			}

			/// <summary>
			/// Set the x component of the vector.
			/// </summary>
			public static Vector3Int WithX (this Vector3Int v, int x)
			{
				return new Vector3Int (x, v.y, v.z);
			}

			/// <summary>
			/// Set the y component of the vector.
			/// </summary>
			public static Vector3Int WithY (this Vector3Int v, int y)
			{
				return new Vector3Int (v.x, y, v.z);
			}

			/// <summary>
			/// Set the z component of the vector.
			/// </summary>
			public static Vector3Int WithZ (this Vector3Int v, int z)
			{
				return new Vector3Int (v.x, v.y, z);
			}

			#endregion

			#region Increment

			/// <summary>
			/// Increments all components of the vector.
			/// </summary>
			public static Vector3 IncrementAll (this Vector3 v, float incr)
			{
				return new Vector3 (v.x + incr, v.y + incr, v.z + incr);
			}

			/// <summary>
			/// Increments all components of the vector.
			/// </summary>
			public static Vector3Int IncrementAll (this Vector3Int v, int incr)
			{
				return new Vector3Int (v.x + incr, v.y + incr, v.z + incr);
			}

			/// <summary>
			/// Increments the x component of the vector.
			/// </summary>
			public static Vector3 IncrementX (this Vector3 v, float incr)
			{
				return new Vector3 (v.x + incr, v.y, v.z);
			}

			/// <summary>
			/// Increments the y component of the vector.
			/// </summary>
			public static Vector3 IncrementY (this Vector3 v, float incr)
			{
				return new Vector3 (v.x, v.y + incr, v.z);
			}

			/// <summary>
			/// Increments the z component of the vector.
			/// </summary>
			public static Vector3 IncrementZ (this Vector3 v, float incr)
			{
				return new Vector3 (v.x, v.y, v.z + incr);
			}

			/// <summary>
			/// Increments the x component of the vector.
			/// </summary>
			public static Vector3Int IncrementX (this Vector3Int v, int incr)
			{
				return new Vector3Int (v.x + incr, v.y, v.z);
			}

			/// <summary>
			/// Increments the y component of the vector.
			/// </summary>
			public static Vector3Int IncrementY (this Vector3Int v, int incr)
			{
				return new Vector3Int (v.x, v.y + incr, v.z);
			}

			/// <summary>
			/// Increments the z component of the vector.
			/// </summary>
			public static Vector3Int IncrementZ (this Vector3Int v, int incr)
			{
				return new Vector3Int (v.x, v.y, v.z + incr);
			}

			#endregion

			#region Abs

			/// <summary>
			/// Set all vector components to their absolute value.
			/// </summary>
			public static Vector3 AbsAll (this Vector3 v)
			{
				return new Vector3 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z));
			}

			/// <summary>
			/// Set all vector components to their absolute value.
			/// </summary>
			public static Vector3Int AbsAll (this Vector3Int v)
			{
				return new Vector3Int (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z));
			}

			/// <summary>
			/// Set the vector's x component to its absolute value.
			/// </summary>
			public static Vector3 AbsX (this Vector3 v)
			{
				return new Vector3 (Math.Abs (v.x), v.y, v.z);
			}

			/// <summary>
			/// Set the vector's y component to its absolute value.
			/// </summary>
			public static Vector3 AbsY (this Vector3 v)
			{
				return new Vector3 (v.x, Math.Abs (v.y), v.z);
			}

			/// <summary>
			/// Set the vector's z component to its absolute value.
			/// </summary>
			public static Vector3 AbsZ (this Vector3 v)
			{
				return new Vector3 (v.x, v.y, Math.Abs (v.z));
			}

			/// <summary>
			/// Set the vector's x component to its absolute value.
			/// </summary>
			public static Vector3Int AbsX (this Vector3Int v)
			{
				return new Vector3Int (Math.Abs (v.x), v.y, v.z);
			}

			/// <summary>
			/// Set the vector's y component to its absolute value.
			/// </summary>
			public static Vector3Int AbsY (this Vector3Int v)
			{
				return new Vector3Int (v.x, Math.Abs (v.y), v.z);
			}

			/// <summary>
			/// Set the vector's z component to its absolute value.
			/// </summary>
			public static Vector3Int AbsZ (this Vector3Int v)
			{
				return new Vector3Int (v.x, v.y, Math.Abs (v.z));
			}

			#endregion

			#region Clamp

			/// <summary>
			/// Clamps all vector's components between two values.
			/// </summary>
			public static Vector3 ClampAll (this Vector3 v, float min, float max)
			{
				return new Vector3 (Mathf.Clamp (v.x, min, max), Mathf.Clamp (v.y, min, max), Mathf.Clamp (v.z, min, max));
			}

			/// <summary>
			/// Clamps all vector's components between two values.
			/// </summary>
			public static Vector3Int ClampAll (this Vector3Int v, int min, int max)
			{
				return new Vector3Int (Mathf.Clamp (v.x, min, max), Mathf.Clamp (v.y, min, max), Mathf.Clamp (v.z, min, max));
			}

			/// <summary>
			/// Clamps vector between values get in two other vectors.
			/// </summary>
			public static Vector3 ClampAll (this Vector3 v, Vector3 min, Vector3 max)
			{
				return new Vector3 (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y), Mathf.Clamp (v.z, min.z, max.z));
			}

			/// <summary>
			/// Clamps vector between values get in two other vectors.
			/// </summary>
			public static Vector3Int ClampAll (this Vector3Int v, Vector3Int min, Vector3Int max)
			{
				return new Vector3Int (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y), Mathf.Clamp (v.z, min.z, max.z));
			}

			/// <summary>
			/// Clamps vector's x component between two values.
			/// </summary>
			public static Vector3 ClampX (this Vector3 v, float min, float max)
			{
				return new Vector3 (Mathf.Clamp (v.x, min, max), v.y, v.z);
			}

			/// <summary>
			/// Clamps vector's y component between two values.
			/// </summary>
			public static Vector3 ClampY (this Vector3 v, float min, float max)
			{
				return new Vector3 (v.x, Mathf.Clamp (v.y, min, max), v.z);
			}

			/// <summary>
			/// Clamps vector's y component between two values.
			/// </summary>
			public static Vector3 ClampZ (this Vector3 v, float min, float max)
			{
				return new Vector3 (v.x, v.y, Mathf.Clamp (v.z, min, max));
			}

			/// <summary>
			/// Clamps vector's x component between two values.
			/// </summary>
			public static Vector3Int ClampX (this Vector3Int v, int min, int max)
			{
				return new Vector3Int (Mathf.Clamp (v.x, min, max), v.y, v.z);
			}

			/// <summary>
			/// Clamps vector's y component between two values.
			/// </summary>
			public static Vector3Int ClampY (this Vector3Int v, int min, int max)
			{
				return new Vector3Int (v.x, Mathf.Clamp (v.y, min, max), v.z);
			}

			/// <summary>
			/// Clamps vector's y component between two values.
			/// </summary>
			public static Vector3Int ClampZ (this Vector3Int v, int min, int max)
			{
				return new Vector3Int (v.x, v.y, Mathf.Clamp (v.z, min, max));
			}

			#endregion

			#region Normals

			/// <summary>
			/// Returns the vector's normal, specifying the rotation axis and if it should be rotated clockwise or not.
			/// Use GetNormalNormalized to get the normalized normal.
			/// </summary>
			public static Vector3 Normal (this Vector3 v, Axis axis, bool clockwise)
			{
				Vector3 n = v;

				switch (axis)
				{
					case Axis.XY:
						n.x = n.x + n.y;
						n.y = n.x - n.y;
						n.x = n.x - n.y;
						n.y *= -1;
						break;

					case Axis.YZ:
						n.y = n.y + n.z;
						n.z = n.y - n.z;
						n.y = n.y - n.z;
						n.z *= -1;
						break;

					case Axis.XZ:
						n.x = n.x + n.z;
						n.z = n.x - n.z;
						n.x = n.x - n.z;
						n.z *= -1;
						break;
				}

				return clockwise ? n : -n;
			}

			/// <summary>
			/// Returns the vector's normal normalized, specifying the rotation axis and if it should be rotated clockwise or not.
			/// </summary>
			public static Vector3 NormalNormalized (this Vector3 v, Axis axis, bool clockwise)
			{
				return v.Normal (axis, clockwise).normalized;
			}

			#endregion

			#region In Viewport checks

			/// <summary>
			/// Returns true if the given Vector3 is in viewport. Given Vector should be converted from world to viewport space.
			/// </summary>
			public static bool IsInViewport (this Vector3 v)
			{
				return v.z > 0 && v.x > 0 && v.x < 1 && v.y > 0 && v.y < 1;
			}

			/// <summary>
			/// Returns true if the given Vector3 is in viewport and closer to the camera than a maximum distance.
			/// Given Vector should be converted from world to viewport space.
			/// </summary>
			public static bool IsInViewport (this Vector3 v, float maxDistance)
			{
				return v.z > 0 && v.z < maxDistance && v.x > 0 && v.x < 1 && v.y > 0 && v.y < 1;
			}

			/// <summary>
			/// Returns true if the given Vector3 is in viewport, allowing a margin and closer to the camera than a maximum distance.
			/// Given Vector should be converted from world to viewport space. Margin should be between something like -0.5 and 0.5.
			/// </summary>
			public static bool IsInViewport (this Vector3 v, float maxDistance, float margin)
			{
				return v.z > 0 && v.z < maxDistance && v.x > -margin && v.x < 1 + margin && v.y > -margin && v.y < 1 + margin;
			}

			/// <summary>
			/// Returns true if the given Vector3 is in viewport, allowing a X margin and a Y margin and closer to the camera than a maximum distance.
			/// Given Vector should be converted from world to viewport space. Margins should be between something like -0.5 and 0.5.
			/// </summary>
			public static bool IsInViewport (this Vector3 v, float maxDistance, float marginX, float marginY)
			{
				return v.z > 0 && v.z < maxDistance && v.x > -marginX && v.x < 1 + marginX && v.y > -marginY && v.y < 1 + marginY;
			}

			#endregion
		}
	}
}