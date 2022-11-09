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

        rewardDescription.SetText("");

        petHasCount.SetText($"환수 보유 {PlayerStats.GetPetHomeHasCount()}");
    }
}
