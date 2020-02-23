using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	public static Room PlayerCurrentRoom;

    [Header("Shooting")] public Transform TargetTransform;
    public ObjectPool[] BulletsPools;
    public ObjectPool FootStepSmokePool;
	public float ShootCooldown = 0.5f;

    [Header("Controls")] public float Speed = 5f;
    public float SmoothTime = .5f;
    
    private float _shootTimer;
	private Vector3 _previousMousePos;

    private Rigidbody2D _rigidBody2D;
	private Vector2 _movement = Vector2.zero;

    private Vector2 _velocity = Vector2.zero;
    private Vector3 _targetPosition;
    private bool _controlsActivated = true;

    private Animator _animator;

	public SpriteRenderer PlayerSprite;
    public Collider2D PropsHitbox;

    [Header("Audio")]
	public AudioSource shootAudioSource;
	public AudioSource stepsAudioSource;
	public AudioClip[] footSteps;

	void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator> ();
		Prop.SawByPlayer += DeactivateControls;
		Prop.Revealed += ActivateControls;
	}

    void Update()
    {
        if (_controlsActivated)
            UpdateControls();
    }

	private void OnDestroy ()
	{
		Prop.SawByPlayer -= DeactivateControls;
		Prop.Revealed -= ActivateControls;
	}

	private void UpdateControls()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");

		_animator.SetFloat ("Velocity", Mathf.Abs (_movement.x + _movement.y));

		if (Input.mousePosition != _previousMousePos)
		{
			var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_targetPosition = (new Vector3(screenToWorldPoint.x, screenToWorldPoint.y) - transform.position).normalized * 2;
		}
        
        if (Math.Abs (Input.GetAxis("TargetHorizontal")) > 0.001f || Math.Abs (Input.GetAxis("TargetVertical")) > 0.001f)
            _targetPosition = new Vector3(Input.GetAxis("TargetHorizontal"), Input.GetAxis("TargetVertical"));

		_previousMousePos = Input.mousePosition;
		TargetTransform.position = transform.position + _targetPosition.normalized * 1.5f;
		PlayerSprite.flipX = _targetPosition.x < 0;

		_shootTimer += Time.deltaTime;

        if ((Input.GetAxis("Shoot") < 0 || Input.GetButton("Shoot")) && _shootTimer > ShootCooldown)
        {
            _shootTimer = 0;
            Shoot();
        }
    }

    #region Public methods

	public void OnFootStep ()
	{
		var smoke = FootStepSmokePool.GetObject ();
		smoke.transform.position = transform.position - new Vector3 (0, 0.7f);

		stepsAudioSource.clip = footSteps[UnityEngine.Random.Range (0, footSteps.Length)];
		stepsAudioSource.Play ();
	}

	public void DeactivateControls()
    {
		PropsHitbox.enabled = false;
		_animator.SetFloat ("Velocity", 0);

		_controlsActivated = false;
		_rigidBody2D.velocity = Vector2.zero;
		_movement = Vector2.zero;
	}
    
    public void ActivateControls()
    {
		PropsHitbox.enabled = true;
        _controlsActivated = true;
    }
    
    #endregion

    #region Private methods

    private void Shoot()
    {
        var bullet = BulletsPools[UnityEngine.Random.Range (0, BulletsPools.Length)].GetObject ();
        bullet.transform.position = TargetTransform.position;
        bullet.GetComponent<Bullet>().Direction = TargetTransform.position - transform.position;

		shootAudioSource.pitch = UnityEngine.Random.Range (1.5f, 2.5f);
		shootAudioSource.Play ();

		_animator.SetTrigger ("Shoot");
		CamShaker.Instance.ShakeCam (0.4f, false);
    }

    private void FixedUpdate()
    {
		//var position = _rigidBody2D.position;
		//_rigidBody2D.position = Vector2.SmoothDamp (position, position + _movement * Speed, ref _velocity, SmoothTime);

		// new movement code (to move chairs)
		var vel = Vector2.SmoothDamp (_rigidBody2D.velocity, _movement * Speed, ref _velocity, SmoothTime);
		_rigidBody2D.velocity = vel;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.TryGetComponent (out Room r))
			PlayerCurrentRoom = r;
	}

	#endregion
}