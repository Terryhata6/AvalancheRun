﻿using UnityEngine;



public class CameraController : BaseController, IExecute, ILateExecute
{
    private CameraView _camera;
    private Vector3 _position;
    public CameraController(MainController main) : base(main)
    {

    }
    public override void Initialize()
    {
        base.Initialize();

        GameEvents.current.OnLevelStart += ActivateBaseCamera;
        GameEvents.current.OnNextLevel += ActivateStartPositionCamera;
        GameEvents.current.OnLevelRestart += ActivateStartPositionCamera;
    }
    public override void Execute()
    {
        

    }
    public void SetCameraChanging()
    {
        _camera.SetObjectChangingState(true);
    }

    public override void LateExecute()
    {
        if (_camera.ObjectChanging)
        {
            if (Vector3.Distance(_camera.PursuedObject.transform.position, _camera.transform.position) < _camera.StopChangingDistance)
            {
                _camera.SetObjectChangingState(false);
            }
        }
        if (!_camera.ObjectChanging)
        {
            _camera.transform.position = _camera.PursuedObject.transform.position;
        }
        else
        {
            _position.x = _camera.PursuedObject.transform.position.x; // сохраняем Z координату камеры
            _position.y = _camera.PursuedObject.transform.position.y; // сохраняем Z координату камеры
            _position.z = _camera.PursuedObject.transform.position.z; // сохраняем Z координату камеры
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _position, _camera.Smooth);
        }
    }

    private void ActivateBaseCamera()
    {
        GameEvents.current.SetActiveCamera("BaseVirtualCamera");
    }
    private void ActivateStartPositionCamera()
    {
        GameEvents.current.SetActiveCamera("Start Position Camera");
    }

    public void SetCamera(CameraView camera)
    {
        if (_camera == null)
        {
            _camera = camera;
        }
    }
    public void SetPursuedObject(GameObject obj)
    {
        _camera.SetPursuedObject(obj);
    }

}
