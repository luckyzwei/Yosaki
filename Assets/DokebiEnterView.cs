using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class DokebiEnterView : MonoBehaviour
{
    [SerializeField]
    private GameObject enterButton;

    [SerializeField]
    private TextMeshProUGUI enterCountText;

    [SerializeField]
    private RectTransform popupBg;

    [SerializeField]
    private float popupOriginWidth;

    [SerializeField]
    private float popupDokebiWidth;

    [SerializeField]
    private List<Button> dokebiEnterButtons;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).AsObservable().Subscribe(e =>
        {
            enterCountText.SetText($"오늘 입장({(int)e}/{GameBalance.dokebiEnterCount})");
        }).AddTo(this);
    }

    public void OnClickEnterButton(int idx)
    {
        int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value;

        if (currentEnterCount >= GameBalance.dokebiEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 입장할수 없습니다.");
            return;
        }

        dokebiEnterButtons.ForEach(e => e.interactable = false);

        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value++;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList,
          successCallBack: () =>
          {
              GameManager.Instance.SetDokebiId(idx);
              GameManager.Instance.LoadContents(GameManager.ContentsType.Dokebi);
          },
          completeCallBack: () =>
          {
              dokebiEnterButtons.ForEach(e => e.interactable = true);
          });
    }


    private void OnEnable()
    {
        enterButton.SetActive(false);

        popupBg.sizeDelta = new Vector2(popupDokebiWidth, popupBg.sizeDelta.y);
    }

    private void OnDisable()
    {
        popupBg.sizeDelta = new Vector2(popupOriginWidth, popupBg.sizeDelta.y);
    }


}
