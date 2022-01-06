using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class UiYomulBuffIndicator : MonoBehaviour
{
    [SerializeField]
    private Image buffIconPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<Image> buffIconList;

    [SerializeField]
    private GameObject awakeBuffObject;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        buffIconList = new List<Image>();

        var tableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var buffImage = Instantiate<Image>(buffIconPrefab, cellParent);

            buffImage.sprite = CommonUiContainer.Instance.buffIconList[i];

            buffIconList.Add(buffImage);

            buffImage.gameObject.SetActive(tableData[i].BUFFTYPEENUM==BuffTypeEnum.Yomul);
        }

        for (int i = 0; i < tableData.Length; i++)
        {
            Image buffImage = buffIconList[i];

            var buffTableData = tableData[i];

            ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.AsObservable().Subscribe(e =>
            {
                buffImage.gameObject.SetActive(((e == -1f || e > 0) && buffTableData.BUFFTYPEENUM==BuffTypeEnum.Yomul) && ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 0);
            }).AddTo(this);
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].AsObservable().Subscribe(e =>
        {
            awakeBuffObject.SetActive(e == 1 && SettingData.YachaEffect.Value == 1);

            if (e == 1)
            {
                for (int i = 0; i < tableData.Length; i++)
                {
                    buffIconList[i].gameObject.SetActive(false);
                }
            }
        }).AddTo(this);

        SettingData.YachaEffect.AsObservable().Subscribe(e =>
        {
            awakeBuffObject.SetActive(ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 1 && e == 1);
        }).AddTo(this);
    }
}
