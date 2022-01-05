using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
public enum guildLevelType
{
    guildMemberPlus,

    guildBuff0 = 100,
    guildBuff1,
    guildBuff2,
    guildBuff3,
    guildBuff4,

    guildIcon0 = 200,
    guildIcon1,
    guildIcon2,
    guildIcon3,
    guildIcon4,
}
public class UiGuildLevelBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI guildExpText;

    [SerializeField]
    private TextMeshProUGUI guildLevelText;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private UiGuildLevelAbilCell levelAbilCellPrefab;

    [SerializeField]
    private Transform levelAbillCellParent;



    private void Awake()
    {
        Subscribe();
    }
    private void OnEnable()
    {
        GuildManager.Instance.LoadGuildLevelGoods();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiGuildLevelAbilCell>(levelAbilCellPrefab, levelAbillCellParent);

            cell.Initialize(tableData[i]);
        }
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildLevelGoods.AsObservable().Subscribe(e =>
        {
            RefreshUi();
        }).AddTo(this);
    }

    private void RefreshUi()
    {
        guildExpText.SetText($"경험치 : {GuildManager.Instance.guildLevelGoods.Value}");
        guildLevelText.SetText($"레벨 : {GuildManager.Instance.GetGuildLevel(GuildManager.Instance.guildLevelGoods.Value)}");
    }

}
