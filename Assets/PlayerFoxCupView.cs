using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFoxCupView : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(e =>
        {

            RefreshUi();

        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.getFoxCup].AsObservable().Subscribe(e =>
        {

            if (e == 1)
            {
                RefreshUi();
            }

        }).AddTo(this);
    }


    private void RefreshUi()
    {
        int idx = PlayerStats.GetCurrentFoxCupIdx();

        if (idx == -1 || ServerData.userInfoTable.TableDatas[UserInfoTable.getFoxCup].Value == 0)
        {
            this.icon.gameObject.SetActive(false);
        }
        else
        {
            this.icon.gameObject.SetActive(true);
            icon.sprite = CommonResourceContainer.GetFoxCupSprite(idx);
        }
    }
}
