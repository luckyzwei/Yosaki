using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiHellWarModeBoard : MonoBehaviour
{
    [SerializeField]
    private Button enterButton;

    [SerializeField]
    private TextMeshProUGUI scoreDescription;

    [SerializeField]
    private TextMeshProUGUI markApplyDescription;

    private static bool registRank = false;

    private void Start()
    {
        scoreDescription.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        if (registRank == false)
        {
            if (ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value != 0)
            {
                RankManager.Instance.UpdateBoss_Score(ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value);
            }
            registRank = true;
        }

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.hellMark].AsObservable().Subscribe(e =>
        {
            if (e != 0)
            {
                int idx = (int)e;

                if (idx < GameBalance.warMarkAbils.Count)
                {
                    markApplyDescription.SetText($"{CommonString.GetHellMarkAbilName(idx)} 적용 : 경험치 획득(%) +{GameBalance.warMarkAbils[idx] * 100f}");
                }
                else
                {
                    markApplyDescription.SetText($"증표 없음");
                }
            }
            else
            {
                markApplyDescription.SetText($"증표 없음");
            }

        }).AddTo(this);
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(ContentsType.HellWarMode);

            enterButton.interactable = false;

        }, () => { });
    }
}
