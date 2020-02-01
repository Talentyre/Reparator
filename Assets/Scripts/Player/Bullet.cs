using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IPoolable
{
    public float LifeTime = 3f;
    public float Speed = 100f;
    
    [HideInInspector]
    public Vector3 Direction { get; set; }

    private float _lifeTimer;


    // Update is called once per frame
    void Update()
    {
        _lifeTimer += Time.deltaTime;

        if (_lifeTimer >= LifeTime)
        {
            gameObject.SetActive(false);
            return;
        }
        
        transform.Translate(Direction * (Time.deltaTime * Speed));
    }

    public override void Reset()
    {
        _lifeTimer = 0f;
    }
}
