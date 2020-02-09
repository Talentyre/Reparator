using UnityEngine;

public class Bullet : IPoolable
{
	public static event System.Action<Collider2D[], Transform, Vector2> HitCollider;

    public float LifeTime = 3f;
    public float Speed = 100f;
	public float HitRadius = 2.5f;

    [HideInInspector]
    public Vector3 Direction { get; set; }

	public LayerMask HitLayerMask;
	public Transform Visual;
	public GameObject OnDestroyedVFX;

	public AudioClip OnHitClip;

    private float _lifeTimer;

    void Update()
    {
        _lifeTimer += Time.deltaTime;

        if (_lifeTimer >= LifeTime)
        {
            gameObject.SetActive(false);
			Instantiate (OnDestroyedVFX, transform.position - new Vector3 (0, 0, .3f), OnDestroyedVFX.transform.rotation);
			return;
        }
        
        transform.Translate(Direction.normalized * (Time.deltaTime * Speed));
		Visual.Rotate (0, 0, 720 * Time.deltaTime);
    }

    public override void Reset()
    {
        _lifeTimer = 0f;
		transform.rotation = Quaternion.identity;
    }

	void OnCollisionEnter2D (Collision2D collision)
	{
		var nearProps = Physics2D.OverlapCircleAll (transform.position, HitRadius, HitLayerMask);
		HitCollider?.Invoke (nearProps, collision.transform, transform.position);

		gameObject.SetActive (false);
		Instantiate (OnDestroyedVFX, transform.position - new Vector3(0,0,.3f), OnDestroyedVFX.transform.rotation);
		AudioManager.Instance.PlaySFX (OnHitClip, 0.5f);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		var nearProps = Physics2D.OverlapCircleAll (transform.position, HitRadius, HitLayerMask);
		HitCollider?.Invoke (nearProps, other.transform, transform.position);

		gameObject.SetActive (false);
		Instantiate (OnDestroyedVFX, transform.position - new Vector3 (0, 0, .3f), OnDestroyedVFX.transform.rotation);
		AudioManager.Instance.PlaySFX (OnHitClip, 0.5f);
	}
}