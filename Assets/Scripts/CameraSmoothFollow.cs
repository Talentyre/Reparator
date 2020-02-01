using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {
 
    public Transform target;
    public float smoothTime = 0.3f;
 
    private Vector3 velocity = Vector3.zero;
    private float _startZ;

    private void Start()
    {
        _startZ = transform.position.z;
    }

    void FixedUpdate () {
        Vector3 goalPos = target.position;
        var newPosition = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, _startZ);
    }
}
