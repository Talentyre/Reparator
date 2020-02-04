using System;
using UnityEngine;

[ExecuteInEditMode]
public class ShadowCastingLight : MonoBehaviour
{
	struct ObstacleDatas
	{
		public Vector2[] vertices;
		public float precision;

		public ObstacleDatas (Vector2[] _vertices, float _precision)
		{
			vertices = _vertices;
			precision = _precision;
		}
	}
	struct AngledVertex
	{
		public Vector3 vertex;
		public float angle;
		public Vector2 uv;
	}

	public LayerMask obstaclesMask;
	[Tooltip ("Colliders to consider, whatever their start range from the light.")]
	public Collider2D[] autoConsidered;
	[Space (15)]
	[Range (1, 100), Tooltip ("The range in which colliders are considered.")]
	public float range;
	[Range (0.001f, 0.1f)]
	public float boxPrecision = 0.001f;
	[Range (0.001f, 0.1f)]
	public float circlePrecision = 0.01f;
	enum CircleResolution
	{
		Four = 4,
		Eight = 8,
		Sixteen = 16,
		ThirtyTwo = 32
	}
	[SerializeField, Tooltip ("Circles that are close to the light will require higher resolution.")]
	CircleResolution circlesResolution = CircleResolution.Sixteen;

	Collider2D[] inRangeObstacles;
	Transform lightTransform;
	Vector3 lightPosition;
	Mesh mesh;
	ObstacleDatas[] obstaclesDatas;
	AngledVertex[] angledVertices;
	Vector3[] vertices;
	Vector2[] uvs;

	[Header ("Debug")] // Remove when building
	public bool showRange = true;
	public bool showCircleResolution = false;
	public bool showRays = false;
	public bool showDiagonals = false;
	Vector2[] allCirclesPoints;

	T[] ConcatenateArrays<T> (T[] first, T[] second)
	{
		T[] concatted = new T[first.Length + second.Length];
		Array.Copy (first, concatted, first.Length);
		Array.Copy (second, 0, concatted, first.Length, second.Length);
		return concatted;
	}

	ObstacleDatas GetObstacleDatas (Collider2D collider)
	{
		if (collider is BoxCollider2D)
			return GetBoxDatas ((BoxCollider2D)collider);
		else if (collider is CircleCollider2D)
			return GetCircleDatas ((CircleCollider2D)collider);
		else if (collider is CompositeCollider2D)
			return GetCompositeDatas ((CompositeCollider2D)collider);

		return new ObstacleDatas ();
	}
	ObstacleDatas GetBoxDatas (BoxCollider2D box)
	{
		Vector2[] corners = new Vector2[4];

		Vector2 boxPosition = box.transform.position;
		Quaternion boxAngle = box.transform.localRotation;
		Vector2 boxScale = box.transform.localScale;
		boxPosition += new Vector2 (box.offset.x * boxScale.x, box.offset.y * boxScale.y);
		Vector2 boxSize = box.size;

		Vector2 diagonal = new Vector2 (boxSize.x * boxScale.x, boxSize.y * boxScale.y) * 0.5f;
		Vector2 angledDiagonal = boxAngle * diagonal;
		Vector2 angledOppositeDiagonal = boxAngle * new Vector2 (-diagonal.x, diagonal.y);

		corners[0] = boxPosition + new Vector2 (+angledDiagonal.x, +angledDiagonal.y);
		corners[1] = boxPosition + new Vector2 (-angledDiagonal.x, -angledDiagonal.y);
		corners[2] = boxPosition + new Vector2 (+angledOppositeDiagonal.x, +angledOppositeDiagonal.y);
		corners[3] = boxPosition + new Vector2 (-angledOppositeDiagonal.x, -angledOppositeDiagonal.y);

		if (showDiagonals)
			foreach (Vector2 diag in corners)
				Debug.DrawLine (boxPosition, diag, Color.blue);

		return new ObstacleDatas (corners, boxPrecision);
	}
	ObstacleDatas GetCircleDatas (CircleCollider2D circle)
	{
		Vector2[] points = new Vector2[(int)circlesResolution / 2 + 1];

		Vector2 circlePosition = circle.transform.position;
		float circleRadius = circle.radius * circle.transform.localScale.x;

		for (int i = 0; i < points.Length; i++)
		{
			float angle = i * (360 / (float)circlesResolution) * Mathf.Deg2Rad;
			Vector2 circlePoint = new Vector3 (circleRadius * Mathf.Sin (angle), circleRadius * Mathf.Cos (angle));

			Vector2 direction = (Vector2)lightPosition - circlePosition;
			float theta = Vector2.Angle (new Vector2 (1, 0), direction);
			theta *= direction.y > 0 ? 1 : -1;
			circlePoint = circlePosition + (Vector2)(Quaternion.Euler (0, 0, theta) * circlePoint);

			points[i] = circlePoint;
		}

		if (showCircleResolution)
		{
			if (allCirclesPoints == null)
				allCirclesPoints = new Vector2[0];
			allCirclesPoints = ConcatenateArrays (allCirclesPoints, points);
		}

		return new ObstacleDatas (points, circlePrecision);
	}
	ObstacleDatas GetCompositeDatas (CompositeCollider2D composite)
	{
		Vector2[] points = new Vector2[composite.pointCount];

		for (int i = 0; i < composite.pathCount; i++)
		{
			Vector2[] path = new Vector2[composite.GetPathPointCount (i)];
			composite.GetPath (i, path);
			points = ConcatenateArrays (points, path);
		}

		return new ObstacleDatas (points, boxPrecision);
	}

	void GetInRangeObstacles ()
	{
		inRangeObstacles = Physics2D.OverlapCircleAll (lightTransform.position, range, obstaclesMask);
		inRangeObstacles = ConcatenateArrays (inRangeObstacles, autoConsidered);

		if (inRangeObstacles.Length == 0)
		{
			Debug.LogWarning ("No obstacle found to the light, disabling the script (click on this log to show the gameObject).", gameObject);
			enabled = false;
		}
	}

	void Setup ()
	{
		if (showCircleResolution)
			allCirclesPoints = null;

		lightPosition = lightTransform.position;

		obstaclesDatas = new ObstacleDatas[inRangeObstacles.Length];
		for (int i = 0; i < inRangeObstacles.Length; i++)
			obstaclesDatas[i] = GetObstacleDatas (inRangeObstacles[i]);

		Vector2[] allVertices = obstaclesDatas[0].vertices;
		for (int i = 1; i < inRangeObstacles.Length; i++)
			allVertices = ConcatenateArrays (allVertices, obstaclesDatas[i].vertices);

		angledVertices = new AngledVertex[allVertices.Length * 2];
		vertices = new Vector3[angledVertices.Length + 1];
		uvs = new Vector2[vertices.Length];

		vertices[0] = lightTransform.worldToLocalMatrix.MultiplyPoint3x4 (lightPosition);
		uvs[0] = new Vector2 (vertices[0].x, vertices[0].y);
	}

	void RaycastAllVertices ()
	{
		int h = 0;

		for (int i = 0; i < obstaclesDatas.Length; i++)
		{
			ObstacleDatas datas = obstaclesDatas[i];

			for (int j = 0; j < datas.vertices.Length; j++)
			{
				Vector2 ray = new Vector2 (datas.vertices[j].x - lightPosition.x, datas.vertices[j].y - lightPosition.y);
				Vector2 offset = new Vector2 (-ray.y, ray.x) * datas.precision;
				Vector2 rayLeft = ray + offset;
				Vector2 rayRight = ray - offset;

				float angle1 = Mathf.Atan2 (rayLeft.y, rayLeft.x);
				float angle2 = Mathf.Atan2 (rayRight.y, rayRight.x);

				RaycastHit2D hit1 = Physics2D.Raycast (lightPosition, rayLeft, 100, obstaclesMask);
				RaycastHit2D hit2 = Physics2D.Raycast (lightPosition, rayRight, 100, obstaclesMask);

				if (showRays)
				{
					Debug.DrawLine (lightTransform.position, hit1.point, Color.red);
					Debug.DrawLine (lightTransform.position, hit2.point, Color.green);
				}

				angledVertices[h * 2].vertex = lightTransform.worldToLocalMatrix.MultiplyPoint3x4 (hit1.point);
				angledVertices[h * 2].angle = angle1;
				angledVertices[h * 2].uv = new Vector2 (angledVertices[h * 2].vertex.x, angledVertices[h * 2].vertex.y);

				angledVertices[h * 2 + 1].vertex = lightTransform.worldToLocalMatrix.MultiplyPoint3x4 (hit2.point);
				angledVertices[h * 2 + 1].angle = angle2;
				angledVertices[h * 2 + 1].uv = new Vector2 (angledVertices[h * 2 + 1].vertex.x, angledVertices[h * 2 + 1].vertex.y);

				h++;
			}
		}
	}

	void ConstructLightMesh ()
	{
		Array.Sort (angledVertices, delegate (AngledVertex one, AngledVertex two)
		{
			return one.angle.CompareTo (two.angle);
		});

		for (int i = 0; i < angledVertices.Length; i++)
		{
			vertices[i + 1] = angledVertices[i].vertex;
			uvs[i + 1] = angledVertices[i].uv;
		}

		for (int i = 0; i < uvs.Length; i++)
			uvs[i] = new Vector2 (uvs[i].x + 0.5f, uvs[i].y + 0.5f);

		int[] triangles = { 0, 1, vertices.Length - 1 };
		for (int i = vertices.Length - 1; i > 0; i--)
			triangles = ConcatenateArrays (triangles, new int[] { 0, i, i - 1 });

		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
	}

	void Awake ()
	{
		lightTransform = transform;

#if UNITY_EDITOR
		MeshFilter meshFilter = GetComponentInChildren<MeshFilter> ();
		Mesh meshCopy = Instantiate (meshFilter.sharedMesh);
		mesh = meshCopy;
		meshFilter.mesh = meshCopy;
#else
		mesh = GetComponentInChildren<MeshFilter> ().mesh;
#endif

		GetInRangeObstacles ();
	}

	void Update ()
	{
		Setup ();
		RaycastAllVertices ();
		ConstructLightMesh ();
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		if (showRange)
			Gizmos.DrawWireSphere (transform.position, range);

		Gizmos.color = Color.red;
		if (showCircleResolution && allCirclesPoints != null)
			foreach (Vector3 p in allCirclesPoints)
				Gizmos.DrawSphere (p, 0.1f - 0.002f * (int)circlesResolution);
	}
}