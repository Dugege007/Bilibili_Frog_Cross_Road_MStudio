using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;   //加载广告

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener,IUnityAdsLoadListener
{
    public static AdsManager instance;

#if UNITY_IOS
    private string gameID = "4971175";
    private string rewardPlacementID = "Rewarded_iOS";
    private string interstitialPlacementID = "Interstitial_iOS";
#elif UNITY_ANDROID
    private string gameID = "4971174";
    private string rewardPlacementID = "Rewarded_Android";
    private string interstitialPlacementID = "Interstitial_Android";
#endif

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);

        //初始化
        Advertisement.Initialize(gameID, false, false, this);
    }

    public void ShowRewardAds()
    {
        Advertisement.Show(rewardPlacementID, this);
    }

    public void ShowInterstitialAds()
    {
        Advertisement.Show(interstitialPlacementID, this);
    }

    #region 广告初始化
    public void OnInitializationComplete()
    {
        Debug.Log("广告初始化成功！");
        //将广告预先加载好，到播放时就不容易卡顿
        Advertisement.Load(rewardPlacementID, this);
        Advertisement.Load(interstitialPlacementID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("广告初始化失败！" + message);
    }
    #endregion

    #region 广告加载

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("广告：" + placementId + "加载完成！");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("广告：" + placementId + "加载失败！");
    }
    #endregion

    #region 广告显示
    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //重新开始游戏
        TransitionManager.instance.Transition("GamePlay");
        //播放音乐
        AudioManager.instance.bgmMusic.Play();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //停止音乐
        AudioManager.instance.bgmMusic.Stop();
    }
    #endregion
}
