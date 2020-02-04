using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {
 
    public Transform target;
    public float smoothTime = 0.3f;

	public BoxCollider2D boundsBox;

	private Camera _camera;
    private Vector3 velocity = Vector3.zero;
    private float _startZ;

	private void ClampToMapBorders (ref Vector3 newPosition)
	{
		if (boundsBox == null)
			return;

		var halfHeight = _camera.orthographicSize;
		var halfWidth = halfHeight * Screen.width / Screen.height;

		var xMin = boundsBox.bounds.min.x + halfWidth;
		var xMax = boundsBox.bounds.max.x - halfWidth;
		var yMin = boundsBox.bounds.min.y + halfHeight;
		var yMax = boundsBox.bounds.max.y - halfHeight;

		newPosition.x = Mathf.Clamp (newPosition.x, xMin, xMax);
		newPosition.y = Mathf.Clamp (newPosition.y, yMin, yMax);
	}

    private void Start()
    {
		_camera = GetComponent<Camera> ();
        _startZ = transform.position.z;
    }

    void FixedUpdate ()
	{
        var newPosition = Vector3.SmoothDamp (transform.position, target.position, ref velocity, smoothTime);
		ClampToMapBorders (ref newPosition);
        transform.position = new Vector3(newPosition.x, newPosition.y, _startZ);
    }
}
