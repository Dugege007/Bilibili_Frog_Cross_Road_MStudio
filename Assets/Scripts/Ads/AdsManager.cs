using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;   //���ع��

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

        //��ʼ��
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

    #region ����ʼ��
    public void OnInitializationComplete()
    {
        Debug.Log("����ʼ���ɹ���");
        //�����Ԥ�ȼ��غã�������ʱ�Ͳ����׿���
        Advertisement.Load(rewardPlacementID, this);
        Advertisement.Load(interstitialPlacementID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("����ʼ��ʧ�ܣ�" + message);
    }
    #endregion

    #region ������

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("��棺" + placementId + "������ɣ�");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("��棺" + placementId + "����ʧ�ܣ�");
    }
    #endregion

    #region �����ʾ
    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //���¿�ʼ��Ϸ
        TransitionManager.instance.Transition("GamePlay");
        //��������
        AudioManager.instance.bgmMusic.Play();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //ֹͣ����
        AudioManager.instance.bgmMusic.Stop();
    }
    #endregion
}
