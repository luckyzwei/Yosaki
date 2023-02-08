
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

        GameEscapeManager.Instance.ShowReviewPage();
    }

    public void OnClickCloseButton()
    {
        PopupManager.Instance.ShowConfirmPopup("알림", "우측 메뉴 -> 보상 -> 요린이 임무를 확인 해 주세요!", null);
        this.gameObject.SetActive(false);
    }
}
