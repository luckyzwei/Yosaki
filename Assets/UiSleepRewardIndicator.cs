using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class UiSleepRewardIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform rootObject;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Button button;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].AsObservable().Subscribe(e =>
        {
            rootObject.gameObject.SetActive(e > GameBalance.sleepRewardMinValue);

            TimeSpan ts = TimeSpan.FromSeconds(Mathf.Min((float)e, GameBalance.sleepRewardMaxValue));

            description.SetText($"{ts.Hours}시간 {ts.Minutes}분");
        }).AddTo(this);
    }

    public void OnClickGetRewardButton()
    {
        int elapsedTime = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value;

        if (elapsedTime <= GameBalance.sleepRewardMinValue)
        {
            PopupManager.Instance.ShowAlarmMessage($"보상을 받을 수 없습니다.");
            return;
        }

        button.interactable = false;

        SleepRewardReceiver.Instance.SetElapsedSecond(elapsedTime);

    }


}
