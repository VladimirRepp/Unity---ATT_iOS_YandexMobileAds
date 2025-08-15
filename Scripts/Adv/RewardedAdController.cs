using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YandexMobileAds;
using YandexMobileAds.Base;

public class RewardedAdController : MonoBehaviour
{
    private RewardedAdLoader _rewardedAdLoader;
    private RewardedAd _rewardedAd;

    public UnityAction Rewarded;
    public UnityAction FailedToShow;

    private void Awake()
    {
#if UNITY_WEBGL
        Debug.Log("--> RewardedAdController.Awake: UNITY_WEBGL");
        return;
#endif
        SetupLoader();
        RequestRewardedAd();

        //DontDestroyOnLoad(gameObject);
    }

    private void SetupLoader()
    {
        _rewardedAdLoader = new RewardedAdLoader();

        _rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
        _rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    private void RequestRewardedAd()
    {
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(AdDictionary.REWARDED_AD_UNIT_ID).Build();
        _rewardedAdLoader.LoadAd(adRequestConfiguration);
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("--> RewardedAdController.HandleAdFailedToLoad");
    }

    private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        _rewardedAd = args.RewardedAd;
        Debug.Log("--> RewardedAdController.HandleAdLoaded");

        _rewardedAd.OnAdClicked += HandleAdClicked;
        _rewardedAd.OnAdShown += HandleAdShown; ;
        _rewardedAd.OnAdDismissed += HandleAdDismissed; ;
        _rewardedAd.OnAdFailedToShow += HandleAdFailedToShow; ;
        _rewardedAd.OnAdImpression += HandleAdImpression; ;
        _rewardedAd.OnRewarded += HandleRewarded; ;
    }

    private void DestriyRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }

    public void HandleRewarded(object sender, Reward args)
    {
        Debug.Log("--> RewardedAdController.HandleRewarded");
        Rewarded?.Invoke();
    }

    public void HandleAdImpression(object sender, ImpressionData args)
    {
        Debug.Log($"--> RewardedAdController.HandleAdImpression: {args.rawData}");
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        Debug.Log("--> RewardedAdController.HandleAdFailedToShow");
        FailedToShow?.Invoke();

        DestriyRewardedAd();
        RequestRewardedAd();
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        Debug.Log("--> RewardedAdController.HandleAdDismissed");

        DestriyRewardedAd();
        RequestRewardedAd();
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        Debug.Log("--> RewardedAdController.HandleAdShown");
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        Debug.Log("--> RewardedAdController.HandleAdClicked");
    }

    public void ShowRewarded()
    {
        if(_rewardedAd != null)
        {
            _rewardedAd.Show(); 
        }
    }
}
