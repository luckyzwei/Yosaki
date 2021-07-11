using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiAttendanceCell : MonoBehaviour
{
    [SerializeField]
    private WeaponView weaponView;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private TextMeshProUGUI dateText;

    private AttendanceRewardData attendanceRewardData;

    private CompositeDisposable compositeDisposable = new CompositeDisposable();

    [SerializeField]
    private GameObject receivedIcon;

    [SerializeField]
    private GameObject lockedIcon;

    public void Initialize(AttendanceRewardData attendanceRewardData)
    {
        this.attendanceRewardData = attendanceRewardData;

        dateText.SetText($"{attendanceRewardData.Id + 1}일차");
        weaponView.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);

        //무기 마도서
        if ((1000 <= attendanceRewardData.Reward_Type && 1100 > attendanceRewardData.Reward_Type) ||
            (attendanceRewardData.Reward_Type >= 2000 && attendanceRewardData.Reward_Type < 2100))
        {
            weaponView.gameObject.SetActive(true);

            //무기
            if (attendanceRewardData.Reward_Type < 2000)
            {
                weaponView.Initialize(TableManager.Instance.GetWeaponDataByStringId(((Item_Type)attendanceRewardData.Reward_Type).ToString()), null);
            }
            //마도서
            else
            {
                weaponView.Initialize(null, TableManager.Instance.GetMagicBookDataByStringId(((Item_Type)attendanceRewardData.Reward_Type).ToString()));
            }
        }
        else
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)attendanceRewardData.Reward_Type);
        }

        amountText.SetText($"{Utils.ConvertBigNum(attendanceRewardData.Reward_Value)}개");

        Subscribe();
    }

    private void Subscribe()
    {
        compositeDisposable.Clear();
        ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.rewardKey].AsObservable().Subscribe(WhenRewardInfoChanged).AddTo(compositeDisposable);
        ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).AsObservable().Subscribe(WhenDayChanged).AddTo(compositeDisposable);
    }

    private void WhenDayChanged(float attendanceCount)
    {
        bool canReceiveReward = attendanceRewardData.Id < attendanceCount;
        lockedIcon.SetActive(!canReceiveReward);
    }

    private void WhenRewardInfoChanged(string rewards)
    {
        var rewardList = rewards.Split(',');

        bool hasReward = false;
        for (int i = 0; i < rewardList.Length; i++)
        {
            if (string.IsNullOrEmpty(rewardList[i]) == false && int.Parse(rewardList[i]) == attendanceRewardData.Id)
            {
                hasReward = true;
            }
        }

        receivedIcon.SetActive(hasReward);
    }

    private void OnDestroy()
    {
        compositeDisposable.Dispose();
    }

    public void OnClickRewardButton()
    {
        var receivedRewardList = ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.rewardKey].Value;
        var rewards = receivedRewardList.Split(',');

        bool hasReward = false;
        for (int i = 0; i < rewards.Length; i++)
        {
            if (string.IsNullOrEmpty(rewards[i]) == false && int.Parse(rewards[i]) == attendanceRewardData.Id)
            {
                hasReward = true;
            }
        }

        if (hasReward)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        int attendanceCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value;

        bool canReceiveReward = attendanceRewardData.Id < attendanceCount;

        if (!canReceiveReward)
        {
            PopupManager.Instance.ShowAlarmMessage("출석일수가 부족합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance((Item_Type)attendanceRewardData.Reward_Type, attendanceRewardData.Reward_Value));

        Param rewardParam = new Param();

        ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.rewardKey].Value += $",{attendanceRewardData.Id}";
        rewardParam.Add(AttendanceServerTable.rewardKey, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.rewardKey].Value);

        transactions.Add(TransactionValue.SetUpdate(AttendanceServerTable.tableName, AttendanceServerTable.Indate, rewardParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
              SoundManager.Instance.PlaySound("RoulletSpin");
              LogManager.Instance.SendLog("출석", "보상획득");
          });
    }
}
