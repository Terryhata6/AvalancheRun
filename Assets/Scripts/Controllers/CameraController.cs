using UnityEngine;



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

        GameEvents.Current.OnLevelStart += ActivateBaseCamera;
        GameEvents.Current.OnNextLevel += ActivateStartPositionCamera;
        GameEvents.Current.OnLevelRestart += ActivateStartPositionCamera;
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
        _camera.transform.position = _camera.PursuedObject.transform.position; 
    }

    private void ActivateBaseCamera()
    {
        GameEvents.Current.SetActiveCamera("45DegreeCamera");
    }
    private void ActivateStartPositionCamera()
    {
        GameEvents.Current.SetActiveCamera("Start Position Camera");
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
