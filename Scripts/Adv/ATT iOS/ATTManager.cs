using System;
using System.Runtime.InteropServices;
using UnityEngine;
using YandexMobileAds;

public class ATTManager : MonoBehaviour
{
    // Метод для вызова нативного кода iOS
    [DllImport("__Internal")] private static extern void RequestTrackingAuthorization();

    public Action<bool> ATTReceiveAndMobileAdsSetUserConsentComplete;

    private string _currentStatus = "default";

    /// <summary>
    /// authorized, denied, not_determined, iOS_less_14, restricted
    /// </summary>
    public string CurrentStatus => _currentStatus;


    private void Awake()
    {
        Debug.Log("--> ATTManager.Awake: RequestTrackingAuthorization called!");
        RequestTrackingAuthorization();
    }

    // Вызывается из Objective-C кода
    public void OnTrackingAuthorizationComplete(string status)
    {
        Debug.Log("--> ATT Status: " + status);
        _currentStatus = status;

        bool consentGiven = false;

        switch (status)
        {
            case "authorized":
                consentGiven = true;
                break;
            case "denied":
                consentGiven = false;
                break;
            case "not_determined":
                consentGiven = false;
                break;
            case "iOS_less_14":
                consentGiven = true;
                break;
            default:
                Debug.Log("--> ATTManager.OnTrackingAuthorizationComplete: Неизвестный статус: " + status);
                consentGiven = false;
                break;
        }

        // Передаём согласие в Yandex Mobile Ads
        MobileAds.SetUserConsent(consentGiven);
        Debug.Log($"--> ATTManager.OnTrackingAuthorizationComplete: Yandex Mobile Ads: согласие = {consentGiven}");

        ATTReceiveAndMobileAdsSetUserConsentComplete?.Invoke(consentGiven);
    }
}