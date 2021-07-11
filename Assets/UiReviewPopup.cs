using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiReviewPopup : MonoBehaviour
{
    private bool alreadyComplete = false;
    public void OnClickButton()
    {
        if (alreadyComplete == true) return;

        alreadyComplete = true;

        SoundManager.Instance.PlayButtonSound();

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += 1000;

        ServerData.goodsTable.SyncToServerEach(GoodsTable.Jade, whenSyncSuccess: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상 수령 성공! 리뷰 남겨주실꺼죠...?", () =>
            {
                GameEscapeManager.Instance.ShowReviewPage();
            });
        },
        whenRequestFailed: () =>
         {
             PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크가 불안정 합니다.", () =>
             {
                 this.gameObject.SetActive(false);
             });
         });
    }
}
