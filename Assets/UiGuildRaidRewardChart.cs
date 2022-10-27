using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildRaidRewardChart : MonoBehaviour
{
    [SerializeField]
    private UiText uiTextPrefab;

    [SerializeField]
    private Transform parent;


    private void Start()
    {
        Iniaitlize();
    }

    private void Iniaitlize()
    {
        var tableData = TableManager.Instance.TwelveBossTable.dataArray[73];

        for (int i = 0; i < tableData.Cutstring.Length; i++)
        {
            var uiText = Instantiate<UiText>(uiTextPrefab, parent);
            uiText.Initialize($"{tableData.Cutstring[i]}({i + 1}점)");
        }
    }


}
