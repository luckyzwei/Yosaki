using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;
using UniRx;
public class UiSasinsuRewardBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;
    [SerializeField]
    private TextMeshProUGUI RequireDescirption;
    [SerializeField]
    private TextMeshProUGUI HasEffectDescription;

    [SerializeField]
    private WeaponView weaponView;

    private BossServerData bossServerData;

    [SerializeField]
    private int sasinIndex;


    private void Start()
    {
        Subscribe();
        SetSasinIdx(0);
    }

    private void Subscribe()
    {
        var weaponServerData0 = ServerData.weaponTable.TableDatas["weapon67"];
        var weaponServerData1 = ServerData.weaponTable.TableDatas["weapon68"];
        var weaponServerData2 = ServerData.weaponTable.TableDatas["weapon69"];
        var weaponServerData3 = ServerData.weaponTable.TableDatas["weapon70"];

        weaponServerData0.hasItem.AsObservable().Subscribe(e =>
        {
            if (sasinIndex == 0)
            {
                buttonDescription.SetText(e == 1 ? "획득 완료" : "획득");
            }

        }).AddTo(this);
        weaponServerData1.hasItem.AsObservable().Subscribe(e =>
        {
            if (sasinIndex == 1)
            {
                buttonDescription.SetText(e == 1 ? "획득 완료" : "획득");
            }

        }).AddTo(this);
        weaponServerData2.hasItem.AsObservable().Subscribe(e =>
        {
            if (sasinIndex == 2)
            {
                buttonDescription.SetText(e == 1 ? "획득 완료" : "획득");
            }

        }).AddTo(this);
        weaponServerData3.hasItem.AsObservable().Subscribe(e =>
        {
            if (sasinIndex == 3)
            {
                buttonDescription.SetText(e == 1 ? "획득 완료" : "획득");
            }

        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        if (sasinIndex == 0)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b0"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 현무 별자리({TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6]}점)를 클리어 해야 합니다.");
                return;
            }

            if (ServerData.weaponTable.TableDatas["weapon67"].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                return;
            }

            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon67"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon67"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon67", ServerData.weaponTable.TableDatas["weapon67"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "현무검(진) 획득!", null);
            });
        }
        else if(sasinIndex==1)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b1"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 백호 별자리({TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6]}점)를 클리어 해야 합니다.");
                return;
            }

            if (ServerData.weaponTable.TableDatas["weapon68"].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                return;
            }

            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon68"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon68"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon68", ServerData.weaponTable.TableDatas["weapon68"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "백호검(진) 획득!", null);
            });
        }
        else if(sasinIndex==2)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b2"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 {TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Name} 별자리({TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6]}점)를 클리어 해야 합니다.");
                return;
            }

            if (ServerData.weaponTable.TableDatas["weapon69"].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                return;
            }

            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon69"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon69"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon69", ServerData.weaponTable.TableDatas["weapon69"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "주작검(진) 획득!", null);
            });
        }
        else if(sasinIndex==3)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b3"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 {TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Name} 별자리({TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Score[6]}점)를 클리어 해야 합니다.");
                return;
            }

            if (ServerData.weaponTable.TableDatas["weapon70"].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                return;
            }

            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon70"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon70"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon70", ServerData.weaponTable.TableDatas["weapon70"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "청룡검(진) 획득!", null);
            });
        }
        else
        {
            Debug.LogError($"{sasinIndex} : 사신 인덱스 확인 필요");
        }
        

 

    }

    public void SetSasinIdx(int idx)
    {
        sasinIndex = idx;

        RequireDescirption.SetText($"마지막 {TableManager.Instance.sasinsuTable.dataArray[sasinIndex].Name} 별자리 클리어시 획득 가능!");
        HasEffectDescription.SetText($"보유 효과 : {CommonString.GetStatusName((StatusType)TableManager.Instance.WeaponEffectDatas[idx+84].Haseffecttype1)} {Utils.ConvertBigNum(TableManager.Instance.WeaponEffectDatas[idx+84].Haseffectvalue1)} 증가");

    }
}
