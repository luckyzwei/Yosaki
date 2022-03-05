using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
public enum guildLevelType
{
    guildMemberPlus,
    guildBuff,
    guildIcon,
    spawnPlus
}
public class UiGuildLevelBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI guildExpText;

    [SerializeField]
    private TextMeshProUGUI guildLevelText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private GameObject rootObject;

    private void Start()
    {

        StartCoroutine(Initialize());
    }

    private void Awake()
    {
        Subscribe();
    }
    private void OnEnable()
    {
        GuildManager.Instance.LoadGuildLevelGoods();
    }

    private IEnumerator Initialize()
    {
        var tableData = TableManager.Instance.GuildLevel.dataArray;

        string description = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            description += $"명성 : {tableData[i].Needamount}\n<color=yellow>{tableData[i].Description}</color>\n\n";
        }

        descriptionText.SetText(description);

        rootObject.SetActive(false);
        yield return null;
        rootObject.SetActive(true);
        yield return null;
        rootObject.SetActive(false);
        yield return null;
        rootObject.SetActive(true);

    }

    private void Subscribe()
    {
        GuildManager.Instance.guildLevelExp.AsObservable().Subscribe(e =>
        {
            RefreshUi();
        }).AddTo(this);
    }

    private void RefreshUi()
    {
        guildExpText.SetText($"명성 : {GuildManager.Instance.guildLevelExp.Value}");
        guildLevelText.SetText($"레벨 : {GuildManager.Instance.GetGuildLevel(GuildManager.Instance.guildLevelExp.Value)}");
    }

}
