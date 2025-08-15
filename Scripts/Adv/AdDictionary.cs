#define AppStore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdDictionary
{
    #region === DEMO ===
    public static string DEMO_APP_OPEN_AD_UNIT_ID = "demo-appopenad-yandex";
    public static string DEMO_INTERSTITIAL_AD_UNIT_ID = "demo-interstitial-yandex";
    public static string DEMO_REWARDED_AD_UNIT_ID = "demo-rewarded-yandex";
    public static string DEMO_BANNER_AD_UNIT_ID = "demo-banner-yandex";
    #endregion

    #region === Release ===
#if RuStore
    public static string APP_OPEN_AD_UNIT_ID => RuStore_app_open_ad_unit_id;
    public static string INTERSTITIAL_AD_UNIT_ID => RuStore_interstitial_ad_unit_id;
    public static string REWARDED_AD_UNIT_ID => RuStore_rewarded_ad_unit_id;
    public static string BANNER_AD_UNIT_ID => RuStore_banner_ad_unit_id;
#endif

#if GooglePlay
    public static string APP_OPEN_AD_UNIT_ID => GooglePlay_app_open_ad_unit_id;
    public static string INTERSTITIAL_AD_UNIT_ID => GooglePlay_interstitial_ad_unit_id;
    public static string REWARDED_AD_UNIT_ID => GooglePlay_rewarded_ad_unit_id;
    public static string BANNER_AD_UNIT_ID => GooglePlay_banner_ad_unit_id;
#endif

#if AppStore
    public static string APP_OPEN_AD_UNIT_ID => AppStore_app_open_ad_unit_id;
    public static string INTERSTITIAL_AD_UNIT_ID => AppStore_interstitial_ad_unit_id;
    public static string REWARDED_AD_UNIT_ID => AppStore_rewarded_ad_unit_id;
    public static string BANNER_AD_UNIT_ID => AppStore_banner_ad_unit_id;
#endif
    #endregion

    #region === GooglePlay ===
    private static string GooglePlay_app_open_ad_unit_id = "A-A-1111111-0";
    private static string GooglePlay_interstitial_ad_unit_id = "A-A-1111111-0";
    private static string GooglePlay_rewarded_ad_unit_id = "A-A-1111111-0";
    private static string GooglePlay_banner_ad_unit_id = "A-A-1111111-0";
    #endregion

    #region === RuStore ===
    private static string RuStore_app_open_ad_unit_id = "A-A-1111111-0";
    private static string RuStore_interstitial_ad_unit_id = "A-A-1111111-0";
    private static string RuStore_rewarded_ad_unit_id = "A-A-1111111-0";
    private static string RuStore_banner_ad_unit_id = "A-A-1111111-0";
    #endregion

    #region === AppStore ===
    private static string AppStore_app_open_ad_unit_id = "A-A-1111111-0";
    private static string AppStore_interstitial_ad_unit_id = "A-A-1111111-0";
    private static string AppStore_rewarded_ad_unit_id = "A-A-1111111-0";
    private static string AppStore_banner_ad_unit_id = "A-A-1111111-0";
    #endregion
}
