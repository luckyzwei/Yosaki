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
    private List<Sprite> passFrame;

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
            if (this.gameObject.activeInHierarchy == true) 
            {
                lockIcon_Free.SetActive(!CanGetReward());
                lockIcon_Ad.SetActive(!CanGetReward());
            }
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

        paidPassBg.sprite = passFrame[passInfo.passGrade];
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
            else if (passInfo.passGrade == 2)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스3이 필요합니다!");
            }
            else if (passInfo.passGrade == 3)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스4가 필요합니다!");
            }
            else if (passInfo.passGrade == 4)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스5가 필요합니다!");
            }
            else if (passInfo.passGrade == 5)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스6이 필요합니다!");
            }
            else if (passInfo.passGrade == 6)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스7이 필요합니다!");
            }
            else if (passInfo.passGrade == 7)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스8이 필요합니다!");
            }
            else if (passInfo.passGrade == 8)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스9이 필요합니다!");
            }
            else if (passInfo.passGrade == 9)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스10이 필요합니다!");
            }
            else if (passInfo.passGrade == 10)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스11이 필요합니다!");
            }
            else if (passInfo.passGrade == 11)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스12가 필요합니다!");
            }
            else if (passInfo.passGrade == 12)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스13가 필요합니다!");
            }
            else if (passInfo.passGrade == 13)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스14가 필요합니다!");
            }
            else if (passInfo.passGrade == 14)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스15가 필요합니다!");
            }
            else if (passInfo.passGrade == 15)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스16가 필요합니다!");
            }   
            else if (passInfo.passGrade == 16)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스17가 필요합니다!");
            }
            else if (passInfo.passGrade == 17)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스18가 필요합니다!");
            }
            else if (passInfo.passGrade == 18)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스19가 필요합니다!");
            }
            else if (passInfo.passGrade == 19)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스20이 필요합니다!");
            }
            else if (passInfo.passGrade == 20)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스21이 필요합니다!");
            }
            else if (passInfo.passGrade == 21)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스22이 필요합니다!");
            }
            else if (passInfo.passGrade == 22)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스23이 필요합니다!");
            }
            else if (passInfo.passGrade == 23)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스24가 필요합니다!");
            }   
            else if (passInfo.passGrade == 24)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스25가 필요합니다!");
            }  
            else if (passInfo.passGrade == 25)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스26이 필요합니다!");
            }  
            else if (passInfo.passGrade == 26)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스27이 필요합니다!");
            }
            else if (passInfo.passGrade == 27)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스28이 필요합니다!");
            }
            else if (passInfo.passGrade == 28)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스29이 필요합니다!");
            }
            else if (passInfo.passGrade == 29)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스30이 필요합니다!");
            }
            else if (passInfo.passGrade == 30)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스31이 필요합니다!");
            }
            else if (passInfo.passGrade == 31)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스32이 필요합니다!");
            }
            else if (passInfo.passGrade == 32)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스33이 필요합니다!");
            }
            else if (passInfo.passGrade == 33)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스34이 필요합니다!");
            }
            else if (passInfo.passGrade == 34)
            {
                PopupManager.Instance.ShowAlarmMessage("여우패스35이 필요합니다!");
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
        else if (passInfo.passGrade == 2)
        {
            return ServerData.iapServerTable.TableDatas["levelpass3"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 3)
        {
            return ServerData.iapServerTable.TableDatas["levelpass4"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 4)
        {
            return ServerData.iapServerTable.TableDatas["levelpass5"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 5)
        {
            return ServerData.iapServerTable.TableDatas["levelpass6"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 6)
        {
            return ServerData.iapServerTable.TableDatas["levelpass7"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 7)
        {
            return ServerData.iapServerTable.TableDatas["levelpass8"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 8)
        {
            return ServerData.iapServerTable.TableDatas["levelpass9"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 9)
        {
            return ServerData.iapServerTable.TableDatas["levelpass10"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 10)
        {
            return ServerData.iapServerTable.TableDatas["levelpass11"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 11)
        {
            return ServerData.iapServerTable.TableDatas["levelpass12"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 12)
        {
            return ServerData.iapServerTable.TableDatas["levelpass13"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 13)
        {
            return ServerData.iapServerTable.TableDatas["levelpass14"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 14)
        {
            return ServerData.iapServerTable.TableDatas["levelpass15"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 15)
        {
            return ServerData.iapServerTable.TableDatas["levelpass16"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 16)
        {
            return ServerData.iapServerTable.TableDatas["levelpass17"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 17)
        {
            return ServerData.iapServerTable.TableDatas["levelpass18"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 18)
        {
            return ServerData.iapServerTable.TableDatas["levelpass19"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 19)
        {
            return ServerData.iapServerTable.TableDatas["levelpass20"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 20)
        {
            return ServerData.iapServerTable.TableDatas["levelpass21"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 21)
        {
            return ServerData.iapServerTable.TableDatas["levelpass22"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 22)
        {
            return ServerData.iapServerTable.TableDatas["levelpass23"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 23)
        {
            return ServerData.iapServerTable.TableDatas["levelpass24"].buyCount.Value > 0;
        }   
        else if (passInfo.passGrade == 24)
        {
            return ServerData.iapServerTable.TableDatas["levelpass25"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 25)
        {
            return ServerData.iapServerTable.TableDatas["levelpass26"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 26)
        {
            return ServerData.iapServerTable.TableDatas["levelpass27"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 27)
        {
            return ServerData.iapServerTable.TableDatas["levelpass28"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 28)
        {
            return ServerData.iapServerTable.TableDatas["levelpass29"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 29)
        {
            return ServerData.iapServerTable.TableDatas["levelpass30"].buyCount.Value > 0;
        }
        
        else if (passInfo.passGrade == 30)
        {
            return ServerData.iapServerTable.TableDatas["levelpass31"].buyCount.Value > 0;
        }        
        else if (passInfo.passGrade == 31)
        {
            return ServerData.iapServerTable.TableDatas["levelpass32"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 32)
        {
            return ServerData.iapServerTable.TableDatas["levelpass33"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 33)
        {
            return ServerData.iapServerTable.TableDatas["levelpass34"].buyCount.Value > 0;
        }
        else if (passInfo.passGrade == 34)
        {
            return ServerData.iapServerTable.TableDatas["levelpass35"].buyCount.Value > 0;
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
          //    LogManager.Instance.SendLogType("Fox", "Normal", $"보상 {passInfo.id}");
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
          //  LogManager.Instance.SendLogType("Fox", "Premium", $"보상 {passInfo.id}");
            PopupManager.Instance.ShowAlarmMessage("보상을 수령했습니다!");
        });
    }

    private bool CanGetReward()
    {
        int currentLevel = (int)ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        return currentLevel >= passInfo.require;
    }

    private void OnEnable()
    {
        RefreshParent();
    }

    private void RefreshParent()
    {
        if (passInfo == null) return;

        if (HasLevelPassProduct() == false)
        {
            if (CanGetReward() == true && HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false)
            {
                this.transform.SetAsFirstSibling();
            }
        }
        else
        {
            if (CanGetReward() == true &&
                (HasReward(passInfo.rewardType_Free_Key, passInfo.id) == false || HasReward(passInfo.rewardType_IAP_Key, passInfo.id)==false))
            {
                this.transform.SetAsFirstSibling();
            }
        }
    }
}
