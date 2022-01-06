using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiYomulSidongEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private GameObject buffAwakeObject;

    private List<ReactiveProperty<int>> buffRemainTimes = new List<ReactiveProperty<int>>();

    void Start()
    {
        Subscribe();
    }

    private bool HasActivatedYomulBuff()
    {
        for (int i = 0; i < buffRemainTimes.Count; i++)
        {
            if (buffRemainTimes[i].Value > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void Subscribe()
    {
        buffRemainTimes.Clear();

        var tableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].BUFFTYPEENUM==BuffTypeEnum.Yomul)
            {
                buffRemainTimes.Add(ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec);
            }
        }

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].BUFFTYPEENUM==BuffTypeEnum.Yomul)
            {
                ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.AsObservable().Subscribe(e =>
                {
                    rootObject.SetActive(HasActivatedYomulBuff() && ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 0);
                }).AddTo(this);
            }
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].AsObservable().Subscribe(e =>
        {
            buffAwakeObject.SetActive(e == 1&& SettingData.YachaEffect.Value == 1);
        }).AddTo(this);

        SettingData.YachaEffect.AsObservable().Subscribe(e =>
        {
            buffAwakeObject.SetActive(ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 1 && e == 1);
        }).AddTo(this);
    }
}
