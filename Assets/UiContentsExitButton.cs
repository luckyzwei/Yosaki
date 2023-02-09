using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiContentsExitButton : MonoBehaviour
{
    [SerializeField]
    private bool ShowWarningMessage = true;
    [SerializeField]
    private GameObject buttonRootObject;

    private void OnEnable()
    {
        if (buttonRootObject != null)
        {
            buttonRootObject.SetActive(NextStageCheck());
        }
    }
    private bool NextStageCheck()
    {
        if (GameManager.contentsType == GameManager.ContentsType.InfiniteTower2 || GameManager.contentsType == GameManager.ContentsType.DokebiTower ||
            GameManager.contentsType == GameManager.ContentsType.FoxMask || GameManager.contentsType == GameManager.ContentsType.Yum ||
            GameManager.contentsType == GameManager.ContentsType.Ok || GameManager.contentsType == GameManager.ContentsType.Do ||
            GameManager.contentsType == GameManager.ContentsType.GradeTest || GameManager.contentsType == GameManager.ContentsType.Sasinsu ||
            GameManager.contentsType == GameManager.ContentsType.SumisanTower
            )
        {
            return true;
        }
        //산신령 & 서재 & 지키미 & 보도
        if ((GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 57) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 72) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 82) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 83)
            )
        {
            return true;
        }
        //도깨비 보스 & 수미산 사천왕
        if ((GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 85) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 86) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 87) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 88) ||
            (GameManager.contentsType == GameManager.ContentsType.TwelveDungeon && GameManager.Instance.bossId == 92))



        {
            return true;
        }
        return false;
    }
    public void OnClickExitButton()
    {
        if (ShowWarningMessage == true)
        {
            PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
            {
                BuffOff();
                GameManager.Instance.LoadNormalField();
            }, null);
        }
        else
        {
            BuffOff();
            GameManager.Instance.LoadNormalField();
        }
    }
    public void OnClickNextStageButton()
    {


        //if (GameManager.contentsType == GameManager.ContentsType.InfiniteTower)
        //{
        //    if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value < (TableManager.Instance.TowerTable.dataArray.Length))
        //    {
        //        GameManager.Instance.LoadContents(GameManager.ContentsType.InfiniteTower);
        //    }
        //}
        if (GameManager.contentsType == GameManager.ContentsType.InfiniteTower2)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value < (TableManager.Instance.TowerTable2.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.InfiniteTower2);
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.DokebiTower)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx3).Value < (TableManager.Instance.towerTable3.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.DokebiTower);
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.FoxMask)
        {
            if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxMask).Value < (TableManager.Instance.FoxMask.dataArray.Length))
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.FoxMask);
            }
        }
        else if (GameManager.contentsType == GameManager.ContentsType.Yum)
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Yum);
        }
        else if (GameManager.contentsType == GameManager.ContentsType.Ok)
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Ok);
        }
        else if (GameManager.contentsType == GameManager.ContentsType.Do)
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Do);
        }
        else if (GameManager.contentsType == GameManager.ContentsType.GradeTest)
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.GradeTest);
        }

        else if ((GameManager.contentsType == GameManager.ContentsType.TwelveDungeon) && (
            (GameManager.Instance.bossId == 57) ||
            (GameManager.Instance.bossId == 72) ||
            (GameManager.Instance.bossId == 82) ||
            (GameManager.Instance.bossId == 83) ||
            (GameManager.Instance.bossId == 85) ||
            (GameManager.Instance.bossId == 86) ||
            (GameManager.Instance.bossId == 87) ||
            (GameManager.Instance.bossId == 88) ||
               (GameManager.Instance.bossId == 92) 
            
            )
            )
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TwelveDungeon);
        }

        //사신수
        else if (GameManager.contentsType == GameManager.ContentsType.Sasinsu)
        {
            GameManager.Instance.SetBossId(GameManager.Instance.bossId);
            GameManager.Instance.LoadContents(GameManager.ContentsType.Sasinsu);
        }
        else if (GameManager.contentsType == GameManager.ContentsType.SumisanTower)
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.SumisanTower);
        }
        else
        {
            if (buttonRootObject != null)
            {
                buttonRootObject.SetActive(false);
            }
            return;
        }



    }

    public void OnClickExitButton_ForPartyRaid()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "포기하고 나가시겠습니까?", () =>
        {
            BuffOff();

            PartyRaidManager.Instance.OnClickCloseButton();
            GameManager.Instance.LoadNormalField();
        }, null);
    }


    private void BuffOff()
    {
        UiSusanoBuff.isImmune.Value = false;
        UiDokebiBuff.isImmune.Value = false;
    }
}
