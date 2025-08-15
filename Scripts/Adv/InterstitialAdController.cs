using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class InterstitialAdController : MonoBehaviour
{
    private InterstitialAdLoader _interstitialAdLoader;
    private Interstitial _interstitial;

    private void Awake()
    {
#if UNITY_WEBGL
    Debug.Log("--> InterstitialAdController.Awake: UNITY_WEBGL");
    return;
#endif

        SetupLoader();
        RequestInterstitial();

        //DontDestroyOnLoad(gameObject);
    }

    private void SetupLoader()
    {
        _interstitialAdLoader = new InterstitialAdLoader();

        _interstitialAdLoader.OnAdLoaded += HandleAdLoaded;
        _interstitialAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    private void RequestInterstitial()
    {
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(AdDictionary.INTERSTITIAL_AD_UNIT_ID).Build();
        _interstitialAdLoader.LoadAd(adRequestConfiguration);
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("--> InterstitialAdController.HandleAdFailedToLoad");
    }

    private void HandleAdLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        _interstitial = args.Interstitial;
        Debug.Log("--> InterstitialAdController.HandleAdLoaded");

        _interstitial.OnAdClicked += HandleAdClicked;
        _interstitial.OnAdShown += HandleAdShown;
        _interstitial.OnAdFailedToShow += HandleAdFailedToShow;
        _interstitial.OnAdImpression += HandleAdImpression;
        _interstitial.OnAdDismissed += HandleAdDismissed;
    }

    private void DestroyInterstitial()
    {
        if (_interstitial != null)
        {
            _interstitial.Destroy();
            _interstitial = null;
        }
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        Debug.Log("--> InterstitialAdController.HandleAdDismissed");
        DestroyInterstitial();
        RequestInterstitial();
    }

    public void HandleAdImpression(object sender, ImpressionData args)
    {
        Debug.Log($"--> InterstitialAdController.HandleAdImpression: {args.rawData}");
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        Debug.Log("--> InterstitialAdController.HandleAdFailedToShow");
        DestroyInterstitial();
        RequestInterstitial();
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        Debug.Log("--> InterstitialAdController.HandleAdShown");
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        Debug.Log("--> InterstitialAdController.HandleAdClicked");
    }

    public void ShowInterstitial()
    {
        if (_interstitial != null)
        {
            _interstitial.Show();
        }
    }
}
