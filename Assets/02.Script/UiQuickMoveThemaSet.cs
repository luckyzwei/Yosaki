using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiQuickMoveThemaSet : MonoBehaviour
{
    [SerializeField]
    private UiQuickMoveThemaCell cellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI stageDescription;

    [SerializeField]
    private List<Sprite> themas;

    [SerializeField]
    private Image bg;

    public void Initialize(List<StageMapData> mapDatas)
    {
        bg.sprite = themas[mapDatas[0].Mapthema];

        stageDescription.SetText($"{CommonString.ThemaName[mapDatas[0].Mapthema]}");

        for (int i = 0; i < mapDatas.Count; i++)
        {
            var cell = Instantiate<UiQuickMoveThemaCell>(cellPrefab, cellParent);
            cell.Initialize(mapDatas[i].Id);
        }
    }
}
