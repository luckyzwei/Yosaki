﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;

public class UiBokPassCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon_free;

    [SerializeField]
    private TextMeshProUGUI itemName_free;

    [SerializeField]
    private Image itemIcon_ad;

    [SerializeField]
    private TextMeshProUGUI itemName_ad;

    [SerializeField]
    private TextMeshProUGUI itemAmount_free;

    [SerializeField]
    private TextMeshProUGUI itemAmount_ad;

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
    private GameObject gaugeImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void Subscribe()
    {
        disposables.Clear();

        //무료보상 데이터 변경시
        ServerData.bokPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_Free_Key, passInfo.id);
            rewardedObject_Free.SetActive(rewarded);

        }).AddTo(disposables);

        //광고보상 데이터 변경시
        ServerData.bokPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Subscribe(e =>
        {
            bool rewarded = HasReward(passInfo.rewardType_IAP_Key, passInfo.id);
            rewardedObject_Ad.SetActive(rewarded);

        }).AddTo(disposables);

        //킬카운트 변경될때
        ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).AsObservable().Subscribe(e =>
        {
            if (this.gameObject.activeInHierarchy)
            {
                lockIcon_Free.SetActive(!CanGetReward());
                lockIcon_Ad.SetActive(!CanGetReward());
                gaugeImage.SetActive(CanGetReward());
            }
        }).AddTo(disposables);
    }

    public void Initialize(PassInfo passInfo)
    {
        this.passInfo = passInfo;

        SetAmount();

        SetItemIcon();

        SetDescriptionText();

        Subscribe();

        RefreshParent();
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
    }

    private void SetDescriptionText()
    {
        descriptionText.SetText($"{Utils.ConvertBigNum(passInfo.require)}");
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.bokPassServerTable.TableDatas[key].Value.Split(',').ToList();
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
            PopupManager.Instance.ShowAlarmMessage("몹 처치가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_Free_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");

        GetFreeReward();

    }

    //광고아님
    public void OnClickAdRewardButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("몹 처치가 부족합니다.");
            return;
        }

        if (HasReward(passInfo.rewardType_IAP_Key, passInfo.id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        if (HasPassItem())
        {
            GetAdReward();
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"보상권이 필요합니다.");
        }
    }

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiChildPassBuyButton.childPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    private void GetFreeReward()
    {
        //로컬
        ServerData.bokPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value += $",{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_Free, passInfo.rewardTypeValue_Free);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_Free_Key, ServerData.bokPassServerTable.TableDatas[passInfo.rewardType_Free_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(BokPassServerTable.tableName, BokPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_Free);
        transactionList.Add(rewardTransactionValue);

        //킬카운트
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.attenCountBok, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            //  LogManager.Instance.SendLogType("월간", "무료", $"{passInfo.id}");
        });
    }
    private void GetAdReward()
    {
        //로컬
        ServerData.bokPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value += $",{passInfo.id}";
        ServerData.AddLocalValue((Item_Type)(int)passInfo.rewardType_IAP, passInfo.rewardTypeValue_IAP);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //패스 보상
        Param passParam = new Param();
        passParam.Add(passInfo.rewardType_IAP_Key, ServerData.bokPassServerTable.TableDatas[passInfo.rewardType_IAP_Key].Value);
        transactionList.Add(TransactionValue.SetUpdate(BokPassServerTable.tableName, BokPassServerTable.Indate, passParam));

        var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)passInfo.rewardType_IAP);
        transactionList.Add(rewardTransactionValue);

        //킬카운트
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.attenCountBok, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("월간", "유료", $"{passInfo.id}");
        });

        PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
    }

    private bool CanGetReward()
    {
        int killCountTotalBok = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value;
        return killCountTotalBok >= passInfo.require;
    }

    private void OnEnable()
    {
        RefreshParent();
    }

    public void RefreshParent()
    {
        if (passInfo == null) return;

        if (HasPassItem() == false)
        {
            if (CanGetReward() == true && HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false)
            {
                this.transform.SetAsFirstSibling();
            }
        }
        else
        {
            if (CanGetReward() == true &&
                (HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false || HasReward(passInfo.rewardType_IAP_Key, passInfo.id) == false))
            {
                this.transform.SetAsFirstSibling();
            }
        }
    }
}
