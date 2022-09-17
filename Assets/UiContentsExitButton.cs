using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiContentsExitButton : MonoBehaviour
{
    [SerializeField]
    private bool ShowWarningMessage = true;
    public void OnClickExitButton()
    {
        if (ShowWarningMessage == true)
        {
            PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
            {
                GameManager.Instance.LoadNormalField();
            }, null);
        }
        else
        {
            GameManager.Instance.LoadNormalField();
        }

    }

    public void OnClickExitButton_ForPartyRaid()
    {
            PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
            {
                PartyRaidManager.Instance.OnClickCloseButton();
                GameManager.Instance.LoadNormalField();
            }, null);
    }
}
