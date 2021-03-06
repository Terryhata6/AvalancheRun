using System.Collections.Generic;
using System;
using UnityEngine;


public class CollectableController : BaseController, IExecute
{
    private List<CollectableView> _collectables = new List<CollectableView>();
    private SaveDataRepo _save;

    private int _bank;
    private int _money;
    private int _rewardScale = 4;
    private float _rotationSpeed = 200f;


    public int Bank => _bank;
    public int Money => _money;

    public CollectableController(MainController main) : base(main)
    {
        _save = new SaveDataRepo();
        _bank = _save.LoadInt(SaveKeyManager.Bank);
        _money = 0;
    }

    public override void Initialize()
    {
        base.Initialize();

        GameEvents.Current.OnAddMoney += AddMoney;
        GameEvents.Current.OnRemoveMoney += RemoveMoney;
        GameEvents.Current.OnLevelStart += StartLevel;
        GameEvents.Current.OnRewardMoney += EndLevelReward;
    }

    public override void Execute()
    {
        base.Execute();

        GameEvents.Current.GetCurrentMoney(_money);
        GameEvents.Current.GetBank(_bank);

        if (_collectables.Count >= 1)
        {
            foreach (CollectableView kostyaDollar in _collectables)
            {
                kostyaDollar.transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void AddView(CollectableView view)
    {
        if (!_collectables.Contains(view))
        {
            _collectables.Add(view);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"Object {view} was already added to update list");
#endif
        }
    }

    public void RemoveView(CollectableView view)
    {
        if (_collectables.Contains(view))
        {
            _collectables.Remove(view);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"Object {view} was already removed from update list");
#endif
        }
    }

    private void AddMoney(int value)
    {
        _money += value;
        _bank += value;
        _save.SaveData(_bank, SaveKeyManager.Bank);
    }

    private void RemoveMoney(int value)
    {
        _bank -= value;
        _save.SaveData(_bank, SaveKeyManager.Bank);
    }

    private void StartLevel()
    {
        _money = 0;
    }

    private void EndLevelReward()
    {
        _bank += _money * (_rewardScale - 1);
        _money *= _rewardScale;
        _save.SaveData(_bank, SaveKeyManager.Bank);
    }
}