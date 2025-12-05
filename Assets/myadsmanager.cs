using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Gley.MobileAds;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    [HideInInspector] public static AdsManager Instance;

    private const string GamesPlayedKey = "GamesPlayed";
    private const int MinGamesBeforeAds = 5;

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

    private void Start()
    {
        InitAd();
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("Application focus " + focus);
    }

    // ---------- Play counter ----------

    public void IncrementGamesPlayed()
    {
        int played = PlayerPrefs.GetInt(GamesPlayedKey, 0);
        played++;
        PlayerPrefs.SetInt(GamesPlayedKey, played);
        PlayerPrefs.Save();
        Debug.Log("Games played = " + played);
    }

    public bool CanShowAds()
    {
        int played = PlayerPrefs.GetInt(GamesPlayedKey, 0);
        return played >= MinGamesBeforeAds;
    }

    // ---------- Scene change → banner ----------

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log("Scene changed: " + newScene.name);

        StopAllCoroutines();
        StartCoroutine(ShowBannerDelayed());
    }

    private IEnumerator ShowBannerDelayed()
    {
        yield return new WaitForSeconds(0.5f);

        if (!API.IsInitialized())
            yield break;

        if (!CanShowAds())
        {
            API.HideBanner();
            yield break;
        }

        Debug.Log("Re-showing banner after delay...");
        API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }

    // ---------- Initialize ads ----------

    private void InitAd()
    {
        API.Initialize(() =>
        {
            RequestBanner();
        });
    }

    public void RequestBanner()
    {
        if (!API.IsInitialized())
            return;

        if (!CanShowAds())
        {
            API.HideBanner();
            return;
        }

        Debug.Log("Show Banner");
        API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }

    public void HideBanner()
    {
        if (!API.IsInitialized())
            return;

        API.HideBanner();
    }

    // ---------- Interstitial / Rewarded ----------

    public void ShowInterstitialAd(Action onClosed)
    {
        if (!API.IsInitialized())
        {
            onClosed?.Invoke();
            return;
        }

        if (!CanShowAds())
        {
            onClosed?.Invoke(); // continue flow without ad
            return;
        }

        UnityAction converted = new UnityAction(onClosed);
        API.ShowInterstitial(converted);
    }

    public void ShowInterstitialAd()
    {
        if (!API.IsInitialized() || !CanShowAds())
            return;

        API.ShowInterstitial();
    }

    public bool IsInterstitialAdLoaded()
    {
        if (!API.IsInitialized())
            return false;

        if (!CanShowAds())
            return false;

        return API.IsInterstitialAvailable();
    }

    public void ShowRewardedAd(Action<bool> onComplete)
    {
        if (!API.IsInitialized())
        {
            onComplete?.Invoke(false);
            return;
        }

        if (!CanShowAds())
        {
            onComplete?.Invoke(false);
            return;
        }

        UnityAction<bool> converted = new UnityAction<bool>(onComplete);
        API.ShowRewardedVideo(converted);
    }
}
