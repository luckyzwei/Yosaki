using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;
public class UiLevelPassCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon_free;

    [SerializeField]
    private Image itemIcon_ad;

    [SerializeField]
    private TextMeshProUGUI itemAmount_free;

    [SerializeField]
    private TextMeshProUGUI itemName_free;

    [SerializeField]
    private TextMeshProUGUI itemAmount_ad;

    [SerializeField]
    private TextMeshProUGUI itemName_ad;

    [SerializeField]
    private GameObject lockIcon_Free;

    [SerializeField]
    private GameObject lockIcon_Ad;

    private PassInfo passInfo;

    [SerializeField]
    private GameObject rewardedObject_Free;

    [SerializeField]
    private GameObject rewardedObject_Ad;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private CompositeDisposable disposables = new CompositeDisposable();

    [SerializeField]
    private Image paidPassBg;

    [SerializeField]
    private Sprite pass1Frame;

    [SerializeField]
    private Sprite pass2Frame;

    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void Subscribe()
    {
        disposables.Clear();

        //무료보상 데이터 변경시
        ServerData.newLevelPass.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key, passInfo.id);
            rewardedObject_Free.SetActive(rewarded);

        }).AddTo(disposables);

        //유료보상 데이터 변경시
        ServerData.newLevelPass.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key, passInfo.id);
            rewardedObject_Ad.SetActive(rewarded);

        }).AddTo(disposables);

        //레벨 변경될때
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
        {
            lockIcon_Free.SetActive(!CanGetReward());
            lockIcon_Ad.SetActive(!CanGetReward());

            //itemAmount_free.color = lockIcon_Free.activeInHierarchy == true ? Color.grey : Color.white;
            //itemName_free.color = lockIcon_Free.activeInHierarchy == true ? Color.grey : Color.white;
            //itemAmount_ad.color = lockIcon_Ad.activeInHierarchy == true ? Color.grey : Color.white;
            //itemName_ad.color = lockIcon_Ad.activeInHierarchy == true ? Color.grey : Color.white;
        }).AddTo(disposables);
    }

    public void Initialize(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();
    }

    private void SetAmount()
    {
        itemAmount_free.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_Free));
        itemAmount_ad.SetText(Utils.ConvertBigNum(passInfo.rewardTypeValue_IAP));
    }

    private void SetItemIcon()
    {
        itemIcon_free.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_Free);
        itemIcon_ad.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)(int)passInfo.rewardType_IAP);

        itemName_free.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_Free));
        itemName_ad.SetText(CommonString.GetItemName((Item_Type)(int)passInfo.rewardType_IAP));

        paidPassBg.sprite = passInfo.passGrade == 0 ? pass1Frame : pass2Frame;
    }

    private void SetDescriptionText()
    {
        descriptionText.SetText($"{passInfo.require}");
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.newLevelPass.TableDatas[key].Value.Split(',').ToList();
    }

    public bool HasReward(string key, int data)
    {
        var splitData = GetSplitData(key);
        return splitData.Contains(data.ToString());
    }

    public void OnClickFreeRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨이 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        GetFreeReward();
    }

    //광고아님
    public void OnClickAdRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨이 부족합니다.");
            return;
        }

        if (HasLevelPassProduct() == false)
        {
            if (passInfo.passGrade == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스1이 필요합니다!");
            }
            else if (passInfo.passGrade == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스2가 필요합니다!");
            }

            return;
        }

        if (HasReward(passInfo.rewardType_IAP_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        GetPassReward();
    }

    private bool HasLevelPassProduct()
    {
        bool hasIapProduct = false;

        if (passInfo.passGrade == 0)
        {
            return ServerData.iapServerTable.TableDatas["levelpass"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 1)
        {
            return ServerData.iapServerTable.TableDatas["levelpass2"].buyCount.Value > 0;
        }

        return hasIapProduct;
    }



    private void GetFreeReward()
    {
        //로컬
        ServerData.newLevelPass.TableDatas[passInfo.rewardType_Free_Key].Value += $",{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.newLevelPass.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(NewLevelPass.tableName, NewLevelPass.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransaction(transactionList, successCallBack: () =>
          {
              SoundManager.Instance.PlaySound("Reward");
              LogManager.Instance.SendLogType("Fox", "Normal", $"보상 {passInfo.id}");
              PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
          });
    }
    private void GetPassReward()
    {
        //로컬
        ServerData.newLevelPass.TableDatas[passInfo.rewardType_IAP_Key].Value += $",{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_IAP, passInfo.rewardTypeValue_IAP);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_IAP_Key, ServerData.newLevelPass.TableDatas[passInfo.rewardType_IAP_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(NewLevelPass.tableName, NewLevelPass.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
        transactionList.Add(rewardTransactionValue);

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
            LogManager.Instance.SendLogType("Fox", "Premium", $"보상 {passInfo.id}");
            PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
        });
    }

    private bool CanGetReward()
    {
        int currentLevel = (int)ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        return currentLevel >= passInfo.require;
    }
}
