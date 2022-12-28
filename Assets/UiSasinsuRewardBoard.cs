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
    private ReactiveProperty<int> sasinIndex;


    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        sasinIndex.AsObservable().Subscribe(e =>
        {
            ButtonDescriptionChange();
        }).AddTo(this);
    }

    public void OnClickGetButton()
    {
        if (sasinIndex.Value == 0)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b0"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 현무 별자리({Utils.ConvertBigNum(TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])})를 클리어 해야 합니다.");
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
        else if(sasinIndex.Value==1)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b1"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 백호 별자리({Utils.ConvertBigNum(TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])})를 클리어 해야 합니다.");
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
        else if(sasinIndex.Value==2)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b2"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 {TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Name} 별자리({Utils.ConvertBigNum(TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])})를 클리어 해야 합니다.");
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
        else if(sasinIndex.Value==3)
        {
            if (ServerData.sasinsuServerTable.TableDatas["b3"].score.Value < TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])
            {
                PopupManager.Instance.ShowAlarmMessage($"마지막 {TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Name} 별자리({Utils.ConvertBigNum(TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Score[6])})를 클리어 해야 합니다.");
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
            Debug.LogError($"{sasinIndex.Value} : 사신 인덱스 확인 필요");
        }


        ButtonDescriptionChange();
    }

    private void ButtonDescriptionChange()
    {
        if (sasinIndex.Value == 0)
        {
            buttonDescription.SetText(ServerData.weaponTable.TableDatas["weapon67"].hasItem.Value == 1 ? "획득 완료" : "획득");
        }
        else if (sasinIndex.Value == 1)
        {
            buttonDescription.SetText(ServerData.weaponTable.TableDatas["weapon68"].hasItem.Value == 1 ? "획득 완료" : "획득");
        }
        else if (sasinIndex.Value == 2)
        {
            buttonDescription.SetText(ServerData.weaponTable.TableDatas["weapon69"].hasItem.Value == 1 ? "획득 완료" : "획득");
        }
        else if (sasinIndex.Value == 3)
        {
            buttonDescription.SetText(ServerData.weaponTable.TableDatas["weapon70"].hasItem.Value == 1 ? "획득 완료" : "획득");
        }
    }

    public void SetSasinIdx(int idx)
    {
        sasinIndex.Value = idx;

        RequireDescirption.SetText($"마지막 {TableManager.Instance.sasinsuTable.dataArray[sasinIndex.Value].Name} 별자리 클리어시 획득 가능!");
        HasEffectDescription.SetText($"보유 효과 : {CommonString.GetStatusName((StatusType)TableManager.Instance.WeaponEffectDatas[idx+84].Haseffecttype1)} {Utils.ConvertBigNum(TableManager.Instance.WeaponEffectDatas[idx+84].Haseffectbase1*100f)} 증가");

    }
}
