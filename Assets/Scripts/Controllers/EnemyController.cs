using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController, IExecute
{
    protected PlayerView _player;
    private List<EnemyView> _enemyList = new List<EnemyView>();
    private Dictionary<EnemyStates, IEnemyState> _stateList = new Dictionary<EnemyStates, IEnemyState>();
    private int _positionIndex = 0;

    public PlayerView Player => _player;

    public EnemyController(MainController main) : base(main) 
    {
        _stateList.Add(EnemyStates.Idle, new EnemyIdleStateModel());
        _stateList.Add(EnemyStates.Downed, new EnemyDownedStateModel());
        _stateList.Add(EnemyStates.Dead, new EnemyDeadStateModel());
        _stateList.Add(EnemyStates.Moving, new EnemyMovingStateModel());
        _stateList.Add(EnemyStates.Finishing, new EnemyFinishingStateModel());
        _stateList.Add(EnemyStates.Connected, new EnemyConnectedStateModel());
        _stateList.Add(EnemyStates.MoveToConnect, new EnemyMoveToConnectStateModel());
    }

    public override void Initialize()
    {
        base.Initialize();
        _player = Main.GetController<PlayerController>().GetPlayer;
        
        GameEvents.Current.OnEnemyGetDamage += KillEnemy;
        GameEvents.Current.OnLevelStart += SetAllEnemiesStatesMoving;
        GameEvents.Current.OnLevelStart += OnLevelStart;
        GameEvents.Current.OnMemberFinish += EnemyFinishLevel;
        GameEvents.Current.OnMoveConnectedEnemy += MoveConnectedEnemy;
        GameEvents.Current.OnConnectEnemy += ConnectEnemy;
    }


    public override void Execute()
    {
        base.Execute();
        foreach (EnemyView enemy in _enemyList)
        {
            if (enemy == null)
            {
                RemoveEnemyFromList(enemy);
            }
            else
            {
                _stateList[enemy.State].Execute(enemy, this);
            }
        }
    }

    public void ConnectEnemy(EnemyView enemy)
    {
        if (enemy.State != EnemyStates.Dead)
        { 
            SetEnemyState(enemy, EnemyStates.MoveToConnect);
            
        }
    }

    public void MoveConnectedEnemy(Quaternion rotation, Vector3 translatePosition, float vectorSpeedMagnitude)
    {
        foreach (EnemyView enemy in _enemyList)
        {
            if (enemy.State == EnemyStates.Connected)
            {
                enemy.Rotation = rotation;
                enemy.Transform.Translate(translatePosition);                
                enemy.Magnitude = vectorSpeedMagnitude;
            }
        }
    }

    public void SetAllEnemiesStatesMoving()
    {
        foreach (EnemyView enemy in _enemyList)
        {
            if (enemy != null && enemy.State != EnemyStates.Dead)
            {
                enemy.SetState(EnemyStates.Moving);
            }
        }
    }
    
    public void SetEnemyState(EnemyView enemy, EnemyStates state)
    {
        enemy.SetState(state);
        if (enemy.State != EnemyStates.Dead)
        {
            enemy.Rigidbody.useGravity = false;
        }
        else
        {
            enemy.Rigidbody.useGravity = true;
        }
        switch (state)
        {
            case EnemyStates.Finishing:
                {
                    enemy.Rotation = Quaternion.LookRotation(Vector3.forward * -1f, Vector3.up);
                    enemy.Animator.SetTrigger($"Dance {Random.Range(0,6)}");
                }
                break;
            case EnemyStates.Connected:
                { 
                    
                }
                break;
            case EnemyStates.MoveToConnect:
                {
                    enemy.SetSquadPosition(new Vector3(Random.Range((int)-2, (int)2), 1, Random.Range((int)-2, (int)2)));
                }
                break;
            default: break;
        }
    }
    /// <summary>
    /// Add enemy to list 
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyToList(EnemyView enemy)
    {
        if (!_enemyList.Contains(enemy))
        {
            _enemyList.Add(enemy);
            SetEnemyState(enemy, EnemyStates.Idle);
        }
    }
    /// <summary>
    /// Remove enemy from list
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyFromList(EnemyView enemy)
    {
        if (_enemyList.Contains(enemy))
        {
            _enemyList.Remove(enemy);
        }
    }
    
    public void KillEnemy(EnemyView enemy)
    {
        if (_enemyList.Contains(enemy))
        {
            enemy.Animator.enabled = false;
            enemy.RagdollState(true);
            SetEnemyState(enemy, EnemyStates.Dead);
            enemy.tag = "DeadEnemy";
            GameEvents.Current.EnemyKilled(enemy);
        }
    }

    public void EnemyFinishLevel(EnemyView enemy)
    {
        SetEnemyState(enemy, EnemyStates.Finishing);
        enemy.Position += Vector3.forward + Vector3.left + Vector3.right * _positionIndex++;
    }

    public void OnLevelStart()
    {
        _positionIndex = 0;
    }
}