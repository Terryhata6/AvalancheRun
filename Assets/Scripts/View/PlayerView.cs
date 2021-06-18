using UnityEngine;

public class PlayerView : BaseObjectView
{
    #region Fields
    [SerializeField] private PlayerState _state = PlayerState.Idle;
    
    [SerializeField, Range(3f, 10f)] private float _movementSpeed = 3.0f;  
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private GameObject _bombShield;
    [SerializeField] private bool _bombShieldReady = true;
    [SerializeField] private bool _canSlowTime = true;
    [SerializeField] private float _slowModeDuration = 3.0f;
    private float _animationBlend;
    private bool _attackReady = true;

    #endregion

    #region AccsessModifyers
    public bool BombShieldReady => _bombShieldReady;
    public bool CanSlowTime => _canSlowTime;
    public float MovementSpeed => _movementSpeed;
    public float AnimationBlend => _animationBlend;
    
    public PlayerState State => _state;
   
    public Animator Animator => _animator;    
    public bool AttackReady => _attackReady;

    public Rigidbody Rigidbody => _playerRigidbody;

    #endregion
    public void Awake()
    {
        if (_playerRigidbody == null)
            _playerRigidbody = GetComponent<Rigidbody>();

        SetRagdoll(false);
        
    }
    public void Start()
    {
        if (_canSlowTime)
        {
            _canSlowTime = false;
            Invoke("SetSlowTimeTrue", 3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagManager.Enemy))
        {
            GameEvents.Current.OnPlayerGetHit();
            GameEvents.Current.PlayerHiterName(collision.gameObject.name);
            Invoke("LevelFail", 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ToHorizontalCam"))
        {
            GameEvents.Current.SetActiveCamera("HorizontalCamera");
        }
        if (other.gameObject.CompareTag("Member"))
        {
            GameEvents.Current.ConnectEnemy(other.GetComponent<EnemyView>());
        }
    }

    public void OnDestroy()
    {
        
    }

    public void SetState(PlayerState state)
    {
        _state = state;
    }

    public void ActivateBombShield()
    {
        _bombShieldReady = false;
        _bombShield.SetActive(true);
        StartCoroutine(Utilits.BombShieldBehavior(_bombShield));
    }

    public void ActivateSlowMode()
    {
        _canSlowTime = false;
        StartCoroutine(Utilits.CountSlowMode(_slowModeDuration));
    }

    public void SetRagdoll(bool value)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody body in bodies)
        {
            body.isKinematic = !value;
            body.transform.GetComponent<Collider>().enabled = value;
        }
        GetComponent<CapsuleCollider>().enabled = !value;
        _playerRigidbody.isKinematic = true;
        _animator.enabled = !value;
    }

    public void LevelFail()
    {
        GameEvents.Current.LevelFailed();
    }

    public void SetSlowTimeAbilityAvailable(bool value)
    {
        _canSlowTime = value;
    }

    private void SetSlowTimeTrue()
    {
        SetSlowTimeAbilityAvailable(true);

    }
}