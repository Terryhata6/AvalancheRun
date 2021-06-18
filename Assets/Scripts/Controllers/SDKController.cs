using System.Collections;
using UnityEngine;
using Unity.RemoteConfig;
using Facebook.Unity;
using GameAnalyticsSDK;

public class SDKController : BaseController
{
    #region IronSource

    private static bool isRewardAdvertismentEnbaled = false;
    public static bool IsRewardAdvertismentEnbaled => isRewardAdvertismentEnbaled; //Check this bool for understanding we are using Advertisement now
    public static IGetReward RewardInstance = null;
    private bool _isISInitialised = false; //base false
    private string _ISIOSAppKey = "f5a151b9";
    private string _ISAndroidAppKey = "f5a151b9";
    private string _currentAppKey = null;
    private float _startTime;
    #endregion

    #region GAFields
    private SaveDataRepo _saveData;
    private int LevelNumber = -1;
    private int LevelDifficulty = -1;
    private int LevelOverall = -1;

    #endregion
    private float _lastInterstitialTime;
    public SDKController(MainController main) : base(main)
    {
        _saveData = new SaveDataRepo();
    }

    public override void Initialize()
    {
        base.Initialize();
        _lastInterstitialTime = Time.time;
        SubscribeInterstitialEvents();
        SubscribeRewardedEvents();
        GameEvents.Current.OnUpdateIronSourceParameters += InitializeIronSource;
        GameEvents.Current.OnNextLevel += StartInterstitialOnLevelEnding;
        GameEvents.Current.OnLevelRestart += StartInterstitialOnLevelEnding;
        GameEvents.Current.OnLevelComplete += OnLevelCompleteEvent;
        GameEvents.Current.OnLevelStart += OnLevelStartEvent;
        GameEvents.Current.OnLevelFailed += OnLevelFailEvent;
        FacebookInitialize();
        GameAnalyticsInitialize();
    }
    
    #region GameAnalytics
    private void GameAnalyticsInitialize()
    {
        GameAnalytics.Initialize();
    }

    private void OnLevelStartEvent()
    {
        LevelNumber = _saveData.LoadInt(SaveKeyManager.LevelNumber);
        LevelDifficulty = _saveData.LoadInt(SaveKeyManager.Difficulty);
        LevelOverall = _saveData.LoadInt(SaveKeyManager.ComplitedLevelValue);
        GameAnalytics.NewDesignEvent($"Level:{LevelNumber}:{LevelDifficulty}:{LevelOverall}:Start");
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, LevelNumber.ToString());
        _startTime = Time.time;
    }

    private void OnLevelFailEvent()
    {        
        GameAnalytics.NewDesignEvent($"Level:{LevelNumber}:{LevelDifficulty}:{LevelOverall}:Failed");
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, LevelNumber.ToString());
    }

    private void OnLevelCompleteEvent()
    {
        GameAnalytics.NewDesignEvent($"Level:{LevelNumber}:{LevelDifficulty}:{LevelOverall}:Complete");
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, LevelNumber.ToString());
    }
    #endregion
    #region FacebookSDK
    private void FacebookInitialize()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.LogWarning("Failed to Initialize the Facebook SDK");
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    #endregion
    #region InitializeIronSourseSDK
    private void InitializeIronSource(bool rewardEnabled)//подписать этот метод чтобы узнать работает реклама или нет сейчас
    {
        if (!_isISInitialised)
        {
            if (rewardEnabled)
            {
                _ISIOSAppKey = ConfigManager.appConfig.GetString("IronSourseIOSAppKey");
                //Debug.LogWarning($"IOSKey {_ISIOSAppKey}");
                _ISAndroidAppKey = ConfigManager.appConfig.GetString("IronSourceAndroidAppKey");
                //Debug.LogWarning($"AndroidKeyKey {_ISAndroidAppKey}");
                
#if UNITY_ANDROID
                _currentAppKey = _ISAndroidAppKey;
#elif UNITY_IPHONE
                _currentAppKey = _ISIOSAppKey;
#else
                _currentAppKey = "unexpected_platform";
#endif                
                IronSource.Agent.validateIntegration();
                IronSource.Agent.init(_currentAppKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
                _isISInitialised = true;
                isRewardAdvertismentEnbaled = true;
            }
        }
        isRewardAdvertismentEnbaled = rewardEnabled;
    }
    
    

    #endregion

    #region Interstitial

    private void StartInterstitialOnLevelEnding()
    {
        if (Time.time - _lastInterstitialTime > 40f)
        {
            ShowInterstitial();
        }
    }

    private void LoadInterstitial()
    {
        if (IsRewardAdvertismentEnbaled)
        {
            IronSource.Agent.loadInterstitial();
        }
    }

    private void ShowInterstitial()
    {
        if (IsRewardAdvertismentEnbaled)
        {
            if (IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.showInterstitial();
                GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "IronSource", "EndLevel");
                _lastInterstitialTime = Time.time;
            }
            else
            {
                LoadInterstitial();
            }
        }
    }

    private void SubscribeInterstitialEvents()
    {
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
    }

    private void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
    }

    private void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    private void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
    }

    private void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    private void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    private void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    private void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
    }
    #endregion
    #region Rewarded
    private void SubscribeRewardedEvents()
    {
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        GameEvents.Current.OnAskingRewardedVideo += AskingShowReward;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
    }

    private void ShowRewardedVideo()
    {
        if (IsRewardAdvertismentEnbaled)
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
            }
        }
    }

    public void AskingShowReward(IGetReward instance)
    {
        SetRewardInstance(instance);
        ShowRewardedVideo();
    }

    private void SetRewardInstance(IGetReward instance)
    {
        RewardInstance = instance;
    }

    private void ClearRewardInstance()
    {
        RewardInstance = null;
    }

    private void RewardedVideoAdOpenedEvent()
    {

    }
    //Invoked when the RewardedVideo ad view is about to be closed.
    //Your activity will now regain its focus.
    private void RewardedVideoAdClosedEvent()
    {

    }
    //Invoked when there is a change in the ad availability status.
    //@param - available - value will change to true when rewarded videos are available.
    //You can then show the video by calling showRewardedVideo().
    //Value will change to false when no videos are available.
    private void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        //Change the in-app 'Traffic Driver' state according to availability.
    }

    //Invoked when the user completed the video and should be rewarded. 
    //If using server-to-server callbacks you may ignore this events and wait for 
    // the callback from the  ironSource server.
    //@param - placement - placement object which contains the reward data
    private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {        
        if (RewardInstance != null)
        {
            RewardInstance.RewardPlayer();
            GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, "IronSource", $"{RewardInstance}");
        }
        else
        {
            RewardInstance = null;
        }
        ClearRewardInstance();
    }
    //Invoked when the Rewarded Video failed to show
    //@param description - string - contains information about the failure.
    private void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        ClearRewardInstance();
    }

    // ----------------------------------------------------------------------------------------
    // Note: the events below are not available for all supported rewarded video ad networks. 
    // Check which events are available per ad network you choose to include in your build. 
    // We recommend only using events which register to ALL ad networks you include in your build. 
    // ----------------------------------------------------------------------------------------

    //Invoked when the video ad starts playing. 
    private void RewardedVideoAdStartedEvent()
    {

    }
    //Invoked when the video ad finishes playing. 
    private void RewardedVideoAdEndedEvent()
    {

    }

    private void RewardedVideoAdClickedEvent(IronSourcePlacement placement)
    {
        GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.RewardedVideo, "IronSource", $"{RewardInstance}");
    }
    #endregion
}