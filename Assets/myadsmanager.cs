using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Gley.MobileAds;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    #region Variables

    [HideInInspector] public static AdsManager Instance;

    #endregion

    #region Unity Behaviour

    private void Awake()
    {
        // Singleton guard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log("Scene changed: " + newScene.name);

        StopAllCoroutines();
        StartCoroutine(ShowBannerDelayed());
    }

    private IEnumerator ShowBannerDelayed()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Re-showing banner after delay...");
        API.HideBanner();
        API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }


    private void Start()
    {
        InitAd();
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("Application focus " + focus);
        if (focus)
        {
            // API.ShowAppOpen(); // optional if you use app-open ads
        }
    }

    #endregion

    #region Initialize Ad

    private void InitAd()
    {
        Gley.MobileAds.API.Initialize(() =>
        {
            RequestBanner();
        });
    }

    public void RequestBanner()
    {
        Debug.Log("Show Banner");
        // Optional: hide first to be extra safe when changing scenes
        API.HideBanner();
        API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }

    public void HideBanner()
    {
        API.HideBanner();
    }

    #endregion

    #region Show Ad

    public void ShowInterstitialAd(Action action)
    {
        UnityAction converted = new UnityAction(action);
        API.ShowInterstitial(converted);
    }

    public void ShowInterstitialAd()
    {
        API.ShowInterstitial();
    }

    public bool IsInterstitialAdLoaded()
    {
        return API.IsInterstitialAvailable();
    }

    public void ShowRewardedAd(Action<bool> action)
    {
        UnityAction<bool> converted = new UnityAction<bool>(action);
        API.ShowRewardedVideo(converted);
    }

    #endregion
}
