using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraView : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private int _basePriority;
    private Vector3 _startPosition;

    public void Start()
    {
        _startPosition = transform.position;

        _camera = GetComponent<CinemachineVirtualCamera>();
        _basePriority = _camera.Priority;
        GameEvents.Current.OnSettingActiveCamera += SetActiveCamera;
    }
    
    public void SetActiveCamera(string name)
    {
        if (this.gameObject.name == name)
        {
            _camera.Priority = 100;
        }
        else 
        {
            _camera.Priority = _basePriority;
        }
    }

    private void OnDestroy()
    {
        GameEvents.Current.OnSettingActiveCamera -= SetActiveCamera;
        
    }
}
