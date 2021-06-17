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
    private List<ObscuredInt> gachaLevelMinNum = new List<ObscuredInt>() { 0, 500, 3000, 6000, 25000 };

    private ReactiveProperty<int> gachaLevel = new ReactiveProperty<int>();

    [SerializeField]
    private Image gaugeImage;

    [SerializeField]
    private TextMeshProUGUI gaugeDescription;

    [SerializeField]
    private TextMeshProUGUI gachaLevelText;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.gachaNum).AsObservable().Subscribe(WhenGachaNumChanged).AddTo(this);
    }

    private void WhenGachaNumChanged(float num)
    {
        int gachaLevel = GachaLevel();

        gachaLevelText.text = $"LV : {gachaLevel + 1}";

        int current = (int)num;

        //만렙아닐때
        if (gachaLevel < gachaLevelMinNum.Count - 1)
        {
            int prefMaxCount = gachaLevelMinNum[gachaLevel];
            int nextMaxCount = gachaLevelMinNum[gachaLevel + 1];

            gaugeDescription.text = $"{current - prefMaxCount}/{nextMaxCount - prefMaxCount}";

            gaugeImage.fillAmount = (float)(current - prefMaxCount) / (float)(nextMaxCount - prefMaxCount);
        }
        //만렙일때
        else
        {
            gaugeDescription.text = $"LV : {gachaLevel + 1}(MAX)";

            gachaLevelText.text = $"MAX";

            gaugeImage.fillAmount = 1f;
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
    public int GachaLevel()
    {
        int gachaNum = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.gachaNum).Value;

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
