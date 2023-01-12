using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using static GameManager;
using UnityEngine.UI;

public class UiSonCloneBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI clearAmount;

    [SerializeField]
    private Button enterButton;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.sonCloneClear].AsObservable().Subscribe(e =>
        {
            int addAmount = (int)(e * GameBalance.sonCloneAddValue);

            description.SetText($"{CommonString.GetItemName(Item_Type.PeachReal)} + {addAmount}개 추가 적용됨");

            clearAmount.SetText($"{Utils.ConvertBigNum(e)}");

        }).AddTo(this);

    }

    public void OnClickEnterCloneButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {
            GameManager.Instance.LoadContents(ContentsType.SonClone);
            enterButton.interactable = false;
        }, () => { });
    }
}
