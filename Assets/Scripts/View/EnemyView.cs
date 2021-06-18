using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : BaseObjectView
{
    private static EnemyController _controller;

    #region Fields
    private bool _dead = false;
    [SerializeField] private EnemyStates _state = EnemyStates.Idle;
    private Rigidbody[] _ragBones;
    private Collider[] _colliders;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _finishPoint;
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _minMovementSpeed = 3.0f;
    [SerializeField] private float _maxMovementSpeed = 6.0f;
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _squadPosition;
    public float Magnitude;
    #endregion
    #region Access modifiers
    public Animator Animator => _animator;
    public EnemyStates State => _state;
    public bool IsDead => _dead;
    public GameObject FinishPoint => _finishPoint;
    public float MovementSpeed => _movementSpeed;
    public Rigidbody Rigidbody => _rigidbody;

    public Vector3 SquadPosition => _squadPosition;
    #endregion

    
    /*
    public void CheckDead()
    {
        if (IsDead)
        { 
            if (gameObject.tag == "Enemy")
            {
                gameObject.tag = "DeadEnemy";
            }
        }
    }*/

    public void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        RagdollState(false);
        _movementSpeed = Random.Range(_minMovementSpeed, _maxMovementSpeed);

    }
    public void Start()
    {
        FindMyController();
    }
    public void SetState(EnemyStates state)
    {
        _state = state;
    }
    public void SetSquadPosition(Vector3 pos)
    {
        _squadPosition = pos;
    }
    public void FindMyController()
    {
        if (_controller == null)
        {
            _controller = FindObjectOfType<MainController>().GetController<EnemyController>();
        }

        _controller.AddEnemyToList(this);
        
    }
    
    public void RagdollState(bool state)
    {        
        _ragBones = GetComponentsInChildren<Rigidbody>();
        foreach (var bone in _ragBones)
        {
            bone.isKinematic = !state;
        }

        _colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in _colliders)
        {
            collider.enabled = state;
        }
        GetComponent<CapsuleCollider>().enabled = !state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameEvents.Current.EnemyGetDamage(this);
        }
    }

    private void OnDestroy()
    {
        _controller.RemoveEnemyFromList(this);
    }
}