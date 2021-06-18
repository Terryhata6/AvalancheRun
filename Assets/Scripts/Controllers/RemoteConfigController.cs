using System.Collections;
using UnityEngine;
using Unity.RemoteConfig;


public class RemoteConfigController : BaseController
{
    private struct userAttrributes { } 
    private struct appAttributes { }
    private bool _fetchCompleted = false;
    public RemoteConfigController(MainController main) : base(main)
    {
        
    }

    public override void Initialize()
    {
        base.Initialize();
        ConfigManager.FetchCompleted += UpdateConfigParameters;
        FetchConfigs();
        
    }

    private void UpdateConfigParameters(ConfigResponse response)
    {
        _fetchCompleted = true;
        //Debug.Log($"ConfigUpdated - {ConfigManager.appConfig.GetBool("OverallShowAdvertisement")}");
        bool showAdvertise = ConfigManager.appConfig.GetBool("OverallShowAdvertisement");
        if (showAdvertise)
        {
            GameEvents.Current.UpdateIronSourceParameters(showAdvertise);            
        }
    }

    private void FetchConfigs()
    {
        ConfigManager.FetchConfigs<userAttrributes, appAttributes>(new userAttrributes(), new appAttributes());
    }
}
