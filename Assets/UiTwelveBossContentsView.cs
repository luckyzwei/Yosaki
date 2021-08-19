using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;
public class UiTwelveBossContentsView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private Image bossIcon;

    private TwelveBossTableData bossTableData;

    [SerializeField]
    private Button enterButton;

    [SerializeField]
    private GameObject buttons;


    public void Initialize(TwelveBossTableData bossTableData)
    {
        this.bossTableData = bossTableData;
        title.SetText(bossTableData.Name);

        var score = ServerData.bossServerTable.TableDatas[bossTableData.Stringid].score.Value;
        if (string.IsNullOrEmpty(score) == false)
        {
            description.SetText($"최고 피해량 : {Utils.ConvertBigNum(float.Parse(score))}");
        }
        else
        {
            description.SetText("기록 없음");
        }

        lockObject.SetActive(bossTableData.Islock);
        buttons.SetActive(bossTableData.Islock == false);

        bossIcon.sprite = CommonUiContainer.Instance.bossIcon[bossTableData.Id];
    }

    public void OnClickRewardButton()
    {
        UiTwelveRewardPopup.Instance.Initialize(bossTableData.Id);
    }

    public void OnClickEnterButton()
    {
        enterButton.interactable = false;
        GameManager.Instance.SetBossId(bossTableData.Id);
        GameManager.Instance.LoadContents(GameManager.ContentsType.TwelveDungeon);
    }
}
