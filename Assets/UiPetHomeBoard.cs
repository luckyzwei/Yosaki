using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPetHomeBoard : MonoBehaviour
{
    [SerializeField]
    private UiPetHomeView petHomeViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;

    [SerializeField]
    private TextMeshProUGUI petHasCount;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        UpdateDescription();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.PetTable.dataArray;

        for (int i = 8; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiPetHomeView>(petHomeViewPrefab, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

    private void UpdateDescription()
    {
        abilDescription.SetText("");

        SetRewardText();

        petHasCount.SetText($"환수 보유 {PlayerStats.GetPetHomeHasCount()}");
    }

    private void SetRewardText()
    {
        int petHomeHasCount = PlayerStats.GetPetHomeHasCount();

        var tableData = TableManager.Instance.petHome.dataArray;

        Dictionary<Item_Type, float> rewards = new Dictionary<Item_Type, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (petHomeHasCount <= i) break;

            Item_Type rewardType = (Item_Type)tableData[i].Rewardtype;
            float rewardValue = tableData[i].Rewardvalue;

            if (rewards.ContainsKey(rewardType) == false)
            {
                rewards.Add(rewardType, 0f);
            }

            rewards[rewardType] += rewardValue;
        }

        var e = rewards.GetEnumerator();

        string description = "";

        while (e.MoveNext())
        {
            description += $"{CommonString.GetItemName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)}개\n";
        }

        rewardDescription.SetText(description);
    }

    public void OnClickRewardButton()
    {

    }
}
