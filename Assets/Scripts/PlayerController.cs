using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject BulletPrefab;
    public Transform TargetTransform;

    public float ShootCooldown = 0.5f;

    private float _shootTimer;
    
    private Rigidbody2D _rigidBody2D;
    private Vector2 _movement = Vector2.zero;
    private Vector2 _targetMovement = Vector2.zero;

    private Vector2 _velocity = Vector2.zero;
    private float _smoothTime = .5f;
    
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");

        TargetTransform.position = transform.position + new Vector3(Input.GetAxis("TargetHorizontal"), Input.GetAxis("TargetVertical"));
        
        _shootTimer -= Time.deltaTime;
        if (Input.GetButton("Fire1") && _shootTimer < ShootCooldown)
        {
            _shootTimer = ShootCooldown;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, TargetTransform.position, Quaternion.identity);
        bullet.transform.forward = TargetTransform.localPosition;
    }

    private void FixedUpdate()
    {
        var position = _rigidBody2D.position;
        _rigidBody2D.position = Vector2.SmoothDamp(position, position+_movement, ref _velocity, _smoothTime);
    }
    
    
}
