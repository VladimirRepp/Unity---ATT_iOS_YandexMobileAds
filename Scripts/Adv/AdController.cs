#define DEBUG

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YandexMobileAds;
using YandexMobileAds.Base;

public class AdController : Singleton<AdController>, IInitialized
{
    private static int COUNT_OF_LEVEL_LOADING;

    private ATTManager _attManager;
    private InterstitialAdController _interstitialAdController;
    private RewardedAdController _rewardedAdController;

    private int _frequencyOfInterstitialAdvertising;
    private int _indexLevel;

    public UnityAction<ERewardedFor> Rewarding;
    public UnityAction ErrorShowReward;

    private ERewardedFor _rewardedFor;

    public int CountOfLevelLoading
    {
        get => COUNT_OF_LEVEL_LOADING;
        set => COUNT_OF_LEVEL_LOADING = value;
    }

    public override void Awake()
    {
        Debug.Log("--> AdController.Awake: MobileAds.SetUserConsent(false)");
        MobileAds.SetUserConsent(false);

        Startup();
        DontDestroyOnLoad(gameObject);
    }

    public void Startup()
    {
        _frequencyOfInterstitialAdvertising = 4;

#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("--> AdController.Awake: UNITY_WEBGL");

        YandexSDK.Instance.Rewarding += OnRewarding;
        YandexSDK.Instance.ErrorShowRewardedAdv += OnFailToShowRewarded;
#else
        Debug.Log("--> AdController.Awake: MOBILE");

#if UNITY_IOS
        _attManager = GameObject.Find("ATTManager").GetComponent<ATTManager>();
        if (_attManager != null)
        {
            _attManager.ATTReceiveAndMobileAdsSetUserConsentComplete += OnATTReceiveAndMobileAdsSetUserConsentComplete;
        }
        else
        {
            Debug.LogError("--> AdController.Startup: _attManager is Null!");
        }

        Debug.Log($"_attManager equals null - {_attManager == null}");
#endif

        InitAds();
#endif

        _indexLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnATTReceiveAndMobileAdsSetUserConsentComplete(bool status)
    {
        InitAds();
    }

    private void InitAds()
    {
        //_interstitialAdController = Instantiate(transform, transform) as InterstitialAdController();
        //_rewardedAdController = new RewardedAdController();

        _interstitialAdController = GetComponent<InterstitialAdController>();
        _rewardedAdController = GetComponent<RewardedAdController>();

        //_appOpenAdController.transform = transform;
        //_interstitialAdController.transform.SetParent(transform);
        //_rewardedAdController.transform.SetParent(transform);
        //_stickyBannerAdController.transform.SetParent(transform);

        _rewardedAdController.Rewarded += OnRewarding;
        _rewardedAdController.FailedToShow += OnFailToShowRewarded;
    }

    private void OnDisable()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexSDK.Instance.Rewarding -= OnRewarding;
        YandexSDK.Instance.ErrorShowRewardedAdv -= OnFailToShowRewarded;
#else
        _rewardedAdController.Rewarded -= OnRewarding;
        _rewardedAdController.FailedToShow -= OnFailToShowRewarded;
#endif

        if (_attManager != null)
        {
            _attManager.ATTReceiveAndMobileAdsSetUserConsentComplete += OnATTReceiveAndMobileAdsSetUserConsentComplete;
        }
    }

    private void OnRewarding()
    {
        Rewarding?.Invoke(_rewardedFor);
    }

    private void OnRewarding(object sender, Reward args)
    {
        Rewarding?.Invoke(_rewardedFor);
    }

    private void OnFailToShowRewarded()
    {
        ErrorShowReward?.Invoke();
    }

    public void ShowInterstitial()
    {
        if (COUNT_OF_LEVEL_LOADING >= _frequencyOfInterstitialAdvertising)
        {
            COUNT_OF_LEVEL_LOADING = 1;
            Debug.LogWarning("--> ShowInterstitial()");

#if UNITY_WEBGL && !UNITY_EDITOR
            YandexSDK.Instance.ShowInterstitialAdv();
#elif UNITY_EDITOR
            Debug.Log("---> UNITY_EDITOR ShowInterstitial");
#else
            _interstitialAdController.ShowInterstitial();
#endif
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        else if (COUNT_OF_LEVEL_LOADING == 1)
        {
            // ������������� �� mobile ��� ������ ������� �� �����
            // ��� ��� ������������ App Open Ad
            Debug.LogWarning("--> ShowInterstitial()");
            YandexSDK.Instance.ShowInterstitialAdv();
        }
#endif
    }

    public void ShowRewarded(ERewardedFor rewardedFor)
    {
        _rewardedFor = rewardedFor;

#if UNITY_WEBGL
        YandexSDK.Instance.ShowRewardedlAdv();
#else
        _rewardedAdController.ShowRewarded();
#endif
    }
}

public enum ERewardedFor
{
    SkipLevel,
    StepBack,
    Test
}
