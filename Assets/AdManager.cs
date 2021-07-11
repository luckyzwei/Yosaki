using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : SingletonMono<AdManager>
{
    private static string AndroidGameId = "4085691";
    private static string IOSGameId = "4085690";

    private string rewardedPlacementId = "rewardedVideo";
    private string videoPlacementId = "video";

    public static string bannerPlacement = "Banner_Weapons";

    private ShowOptions options;

    private System.Action rewardEndCallBack;

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
    }

    private void ShowNormalVideo()
    {
        Advertisement.Show(videoPlacementId, options);
    }
    private void ShowRewardedVideo()
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
            ShowRewardedVideo();
        }
    }

    private bool HasRemoveAdProduct()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas["removead"].buyCount.Value > 0;

        bool hasReceipt = IAPManager.Instance.HasProduct("removead");

        return hasReceipt || hasIapProduct;
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            rewardEndCallBack?.Invoke();
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 보상 획득!", null);
            SoundManager.Instance.PlaySound("GoldUse");
        }
        else if (result == ShowResult.Failed)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 재생에 실패했습니다.\n잠시후 다시 시도해주세요", null);
        }
    }

    ////배너
    //IEnumerator ShowBannerWhenReady()
    //{
    //    while (!Advertisement.IsReady(bannerPlacement))
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    //    Advertisement.Banner.Show(bannerPlacement);
    //    // Advertisement.Banner.Hide();
    //}

}
