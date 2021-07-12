using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGachaPopup : SingletonMono<UiGachaPopup>
{
    //누적
    private List<ObscuredInt> gachaLevelMinNum = new List<ObscuredInt>() { 0,500, 3000, 6000, 30000, 50000, 80000, 120000, 160000, 200000 };

    private ReactiveProperty<int> gachaLevel = new ReactiveProperty<int>();

    [SerializeField]
    private List<Image> gaugeImage;

    [SerializeField]
    private List<TextMeshProUGUI> gaugeDescription;

    [SerializeField]
    private List<TextMeshProUGUI> gachaLevelText;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Weapon).AsObservable().Subscribe(WhenGachaNumChanged_Weapon).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_MagicBook).AsObservable().Subscribe(WhenGachaNumChanged_MagicBook).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Skill).AsObservable().Subscribe(WhenGachaNumChanged_SKill).AddTo(this);
    }

    private void WhenGachaNumChanged_Weapon(float num)
    {
        int gachaLevel = GachaLevel(UserInfoTable.gachaNum_Weapon);

        gachaLevelText[0].text = $"LV : {gachaLevel + 1}";

        int current = (int)num;

        //만렙아닐때
        if (gachaLevel < gachaLevelMinNum.Count - 1)
        {
            int prefMaxCount = gachaLevelMinNum[gachaLevel];
            int nextMaxCount = gachaLevelMinNum[gachaLevel + 1];

            gaugeDescription[0].text = $"{current - prefMaxCount}/{nextMaxCount - prefMaxCount}";

            gaugeImage[0].fillAmount = (float)(current - prefMaxCount) / (float)(nextMaxCount - prefMaxCount);
        }
        //만렙일때
        else
        {
            gaugeDescription[0].text = $"LV : {gachaLevel + 1}(MAX)";

            gachaLevelText[0].text = $"MAX";

            gaugeImage[0].fillAmount = 1f;
        }
    }

    private void WhenGachaNumChanged_MagicBook(float num)
    {
        int gachaLevel = GachaLevel(UserInfoTable.gachaNum_MagicBook);

        gachaLevelText[1].text = $"LV : {gachaLevel + 1}";

        int current = (int)num;

        //만렙아닐때
        if (gachaLevel < gachaLevelMinNum.Count - 1)
        {
            int prefMaxCount = gachaLevelMinNum[gachaLevel];
            int nextMaxCount = gachaLevelMinNum[gachaLevel + 1];

            gaugeDescription[1].text = $"{current - prefMaxCount}/{nextMaxCount - prefMaxCount}";

            gaugeImage[1].fillAmount = (float)(current - prefMaxCount) / (float)(nextMaxCount - prefMaxCount);
        }
        //만렙일때
        else
        {
            gaugeDescription[1].text = $"LV : {gachaLevel + 1}(MAX)";

            gachaLevelText[1].text = $"MAX";

            gaugeImage[1].fillAmount = 1f;
        }
    }

    private void WhenGachaNumChanged_SKill(float num)
    {
        int gachaLevel = GachaLevel(UserInfoTable.gachaNum_Skill);

        gachaLevelText[2].text = $"LV : {gachaLevel + 1}";

        int current = (int)num;

        //만렙아닐때
        if (gachaLevel < gachaLevelMinNum.Count - 1)
        {
            int prefMaxCount = gachaLevelMinNum[gachaLevel];
            int nextMaxCount = gachaLevelMinNum[gachaLevel + 1];

            gaugeDescription[2].text = $"{current - prefMaxCount}/{nextMaxCount - prefMaxCount}";

            gaugeImage[2].fillAmount = (float)(current - prefMaxCount) / (float)(nextMaxCount - prefMaxCount);
        }
        //만렙일때
        else
        {
            gaugeDescription[2].text = $"LV : {gachaLevel + 1}(MAX)";

            gachaLevelText[2].text = $"MAX";

            gaugeImage[2].fillAmount = 1f;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        WaitForSeconds randomizeDelay = new WaitForSeconds(1.0f);

        while (true)
        {
            gachaLevelMinNum.ForEach(e => e.RandomizeCryptoKey());
            yield return randomizeDelay;
        }
    }

    /// <summary>
    /// 0부터 시작
    /// </summary>
    /// <returns></returns>
    public int GachaLevel(string key)
    {
        int gachaNum = (int)ServerData.userInfoTable.GetTableData(key).Value;

        int gachaLevel = 0;

        for (int i = 0; i < gachaLevelMinNum.Count; i++)
        {
            if (gachaNum >= gachaLevelMinNum[i])
            {
                gachaLevel = i;
            }
            else
            {
                break;
            }
        }

        this.gachaLevel.Value = gachaLevel;

        return gachaLevel;
    }
}
