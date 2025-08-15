using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using YandexMobileAds;
using YandexMobileAds.Base;

public class AppOpenAdController : MonoBehaviour
{
    private AppOpenAdLoader _appOpenAdLoader;
    private AppOpenAd _appOpenAd;
    private static bool _isAdShowColdStart;
    private static bool _isFirstLaunched = true;

    private void Awake()
    {
#if UNITY_WEBGL
    Debug.Log("--> AppOpenAdController.Awake: UNITY_WEBGL");
    return;
#endif

        if (!_isFirstLaunched)
            return;

        _isFirstLaunched = false;
        SetupLoader();
        RequesAppOpenAd();

        DontDestroyOnLoad(gameObject);

        AppStateObserver.OnAppStateChanged += HandleAppStateChanged;
    }

    private void OnATTReceiveAndMobileAdsSetUserConsentComplete(bool obj)
    {
        RequesAppOpenAd();
    }

    private void OnDestroy()
    {
        AppStateObserver.OnAppStateChanged -= HandleAppStateChanged;
    }

    private void SetupLoader()
    {
        _appOpenAdLoader = new AppOpenAdLoader();

        _appOpenAdLoader.OnAdLoaded += HandleAdLoaded;
        _appOpenAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    private void RequesAppOpenAd()
    {
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(AdDictionary.APP_OPEN_AD_UNIT_ID).Build();
        _appOpenAdLoader.LoadAd(adRequestConfiguration);
    }

    private void ShowAppOpenAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Show();
        }
    }

    private void DestroyAppOpenAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }
    }

    private void HandleAppStateChanged(object sender, AppStateChangedEventArgs args)
    {
        if (!args.IsInBackground)
        {
            ShowAppOpenAd();
        }
    }

    private void HandleAdLoaded(object sender, AppOpenAdLoadedEventArgs args)
    {
        _appOpenAd = args.AppOpenAd;
        Debug.Log("--> AppOpenAdController.HandleAdLoaded");

        _appOpenAd.OnAdClicked += HandleAdClicked;
        _appOpenAd.OnAdShown += HandleAdShown;
        _appOpenAd.OnAdFailedToShow += HandleAdFailedToShow;
        _appOpenAd.OnAdDismissed += HandleAdDismissed;
        _appOpenAd.OnAdImpression += HandleAdImpression;

        if (!_isAdShowColdStart)
        {
            ShowAppOpenAd();
            _isAdShowColdStart = true;
        }
    }

    private void HandleAdImpression(object sender, ImpressionData e)
    {
        Debug.Log($"--> AppOpenAdController.HandleAdImpression: {e.rawData}");
    }

    private void HandleAdDismissed(object sender, EventArgs e)
    {
        Debug.Log("--> AppOpenAdController.HandleAdDismissed");
        DestroyAppOpenAd();
        RequesAppOpenAd();
    }

    private void HandleAdFailedToShow(object sender, AdFailureEventArgs e)
    {
        Debug.Log("--> AppOpenAdController.HandleAdFailedToShow");
        DestroyAppOpenAd();
        RequesAppOpenAd();
    }

    private void HandleAdShown(object sender, EventArgs e)
    {
        Debug.Log("--> AppOpenAdController.HandleAdShown");
    }

    private void HandleAdClicked(object sender, EventArgs e)
    {
        Debug.Log("--> AppOpenAdController.HandleAdClicked");
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("--> AppOpenAdController.HandleAdFailedToLoad");
    }
}
