using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

using UnityEngine.Events;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;

public class AdManager : SingletonMono<AdManager>
{
    private static string AndroidGameId = "4235399";
    private static string IOSGameId = "4235398";

    private string rewardedPlacementId = "Rewarded_Android";
    private string videoPlacementId = "video";

    public static string bannerPlacement = "Banner_Weapons";

    private ShowOptions options;

    private System.Action rewardEndCallBack;

    private bool admobLoadSuccess = false;

    private new void Awake()
    {
        base.Awake();
#if UNITY_ANDROID
        Advertisement.Initialize(AndroidGameId);
#elif UNITY_IOS
 Advertisement.Initialize(IOSGameId);
#endif
        Initialize();
    }

    private void Initialize()
    {
        options = new ShowOptions();
        options.resultCallback = HandleShowResult;


        //ADMob
    }

    private void ShowNormalVideo()
    {
        Advertisement.Show(videoPlacementId, options);
    }
    private void ShowRewardedUnityVideo()
    {
        Advertisement.Show(rewardedPlacementId, options);
    }

    public void ShowRewardedReward(System.Action callBack)
    {
        //광고제거 구매유저
        if (HasRemoveAdProduct())
        {
            callBack.Invoke();
        }
        else
        {
            this.rewardEndCallBack = callBack;

            if (admobLoadSuccess)
            {
                if (this.rewardedAd.IsLoaded())
                {
                    this.rewardedAd.Show();
                }
            }
            else
            {
                ShowRewardedUnityVideo();
            }


        }
    }

    private bool HasRemoveAdProduct()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas["removead"].buyCount.Value > 0;

        return hasIapProduct;
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            GetReward();
        }
        else if (result == ShowResult.Failed)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 재생에 실패했습니다.\n잠시후 다시 시도해주세요", null);
        }
    }

    private void GetReward() 
    {
        rewardEndCallBack?.Invoke();
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 보상 획득!", null);
        SoundManager.Instance.PlaySound("GoldUse");
    }


    //admob
    private RewardedAd rewardedAd;

    public void Start()
    {
        string adUnitId;
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;

        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;

        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //초기화 광고 성공
        admobLoadSuccess = true;
        Debug.LogError("AdMob_admobLoadSuccess");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //초기화 광고 실패
        admobLoadSuccess = false;
        Debug.LogError("AdMob_admobLoadFailed");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //X
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //실패
        Debug.LogError("AdMob_AdLoadFailed");
        ShowRewardedUnityVideo();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //X
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //보상 획득
        Debug.LogError("AdMob_GetReward");
        GetReward();
    }


}
