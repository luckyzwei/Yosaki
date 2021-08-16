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

    public void Initialize(TwelveBossTableData bossTableData)
    {
        this.bossTableData = bossTableData;
        title.SetText(bossTableData.Name);
        description.SetText(bossTableData.Description);

        lockObject.SetActive(bossTableData.Islock);

        bossIcon.sprite = CommonUiContainer.Instance.bossIcon[bossTableData.Id];
    }

    public void OnClickEnterButton()
    {
        int price = GameBalance.twelveDungeonEnterPrice;

        int currentJadeNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

        if (currentJadeNum < price)
        {
            PopupManager.Instance.ShowAlarmMessage($"옥이 부족합니다.");
            return;
        }

        enterButton.interactable = false;

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= price;

        ServerData.goodsTable.SyncToServerEach(GoodsTable.Jade, () =>
        {
            GameManager.Instance.SetBossId(bossTableData.Id);
            GameManager.Instance.LoadContents(GameManager.ContentsType.TwelveDungeon);
        },
        () =>
        {
            enterButton.interactable = true;

        });
    }
}
