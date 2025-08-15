using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class StickyBannerAdController : MonoBehaviour
{
    private Banner _banner;

    private void Awake()
    {
#if UNITY_WEBGL
    Debug.Log("--> StickyBannerAdController.Awake: UNITY_WEBGL");
    return;
#endif

        RequestStickyBanner();
        DontDestroyOnLoad(gameObject);
    }

    private int GetScreenWindthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    private void RequestStickyBanner()
    {
        BannerAdSize bannerMaxSize = BannerAdSize.StickySize(GetScreenWindthDp());
        _banner = new Banner(AdDictionary.BANNER_AD_UNIT_ID, bannerMaxSize, AdPosition.TopCenter);

        AdRequest request = new AdRequest.Builder().Build();
        _banner.LoadAd(request);

        _banner.OnAdClicked += HandleAdClicked;
        _banner.OnAdLoaded += HandleAdLoaded; ;
        _banner.OnAdFailedToLoad += HandleAdFailedToLoad; ;
        _banner.OnLeftApplication += HandleLeftApplication; ;
        _banner.OnReturnedToApplication += HandleReturnedToApplication; ;
        _banner.OnImpression += HandleImpression; ;
    }

    private void HandleImpression(object sender, ImpressionData args)
    {
        var data = args == null ? null : args.rawData;
        Debug.Log($"--> StickyBannerAdController.HandleImpression: {data}");
    }

    private void HandleReturnedToApplication(object sender, EventArgs args)
    {
        Debug.Log("--> StickyBannerAdController.HandleReturnedToApplication");
    }

    private void HandleLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("--> StickyBannerAdController.HandleLeftApplication");
    }

    private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        Debug.Log("--> StickyBannerAdController.HandleAdFailedToLoad");
    }

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("--> StickyBannerAdController.HandleAdLoaded");
        _banner.Show();
    }

    private void HandleAdClicked(object sender, System.EventArgs args)
    {
        Debug.Log("--> StickyBannerAdController.HandleAdClicked");
    }
}
