using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : IInitialize
{
    protected bool _isActive = true;
    protected MainController _main;
    public MainController Main => _main;
    public BaseController(MainController main)
    {
        main.AddController(this);
        _main = main;
        //Debug.Log($"{this.GetType()} added in controller list");
    }
    public bool IsActive => _isActive;
    protected virtual void SetState(bool state)
    {
        _isActive = state;
    }

    #region IInitialize
    public virtual void Initialize()
    {
        
    }
    #endregion
    
    public virtual void Execute()
    {
        
    }
    public virtual void LateExecute() { }
    public virtual void Enable()
    {
        SetState(true);
    }
    public virtual void Disable()
    {
        SetState(false);
    }
}