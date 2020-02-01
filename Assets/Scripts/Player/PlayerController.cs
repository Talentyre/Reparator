using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Shooting")] public Transform TargetTransform;
    public ObjectPool BulletPool;
    public float ShootCooldown = 0.5f;

    [Header("Controls")] public float Speed = 5f;
    public float SmoothTime = .5f;
    
    private float _shootTimer;

    private Rigidbody2D _rigidBody2D;
    private Vector2 _movement = Vector2.zero;

    private Vector2 _velocity = Vector2.zero;
    private Vector3 _targetPosition;
    private bool _controlsActivated = true;


    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_controlsActivated)
            UpdateControls();
    }

    private void UpdateControls()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");

        _targetPosition = new Vector3(Input.GetAxis("TargetHorizontal"), Input.GetAxis("TargetVertical"));
        TargetTransform.position = transform.position + _targetPosition;

        _shootTimer += Time.deltaTime;

        if (Input.GetAxis("Shoot") < 0 && _shootTimer > ShootCooldown)
        {
            _shootTimer = 0;
            Shoot();
        }
    }

    #region Public methods

    public void DeactivateControls()
    {
        _controlsActivated = false;
    }
    
    public void ActivateControls()
    {
        _controlsActivated = true;
    }
    
    #endregion

    #region Private methods

    private void Shoot()
    {
        var bullet = BulletPool.GetObject();
        bullet.transform.position = TargetTransform.position;
        bullet.GetComponent<Bullet>().Direction = _targetPosition;
    }

    private void FixedUpdate()
    {
        var position = _rigidBody2D.position;
        _rigidBody2D.position = Vector2.SmoothDamp(position, position + _movement * Speed, ref _velocity, SmoothTime);
    }

    #endregion
}