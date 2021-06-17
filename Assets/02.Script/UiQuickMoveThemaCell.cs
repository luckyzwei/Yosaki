using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiQuickMoveThemaCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stageName;

    private int mapIdx;

    public void Initialize(int mapIdx)
    {
        if (mapIdx == 0)
        {
            stageName.SetText($"{CommonString.ThemaName[0]}");
        }
        else
        {
            int pref = (mapIdx - 1) / 6 + 1;
            int def = (mapIdx - 1) % 6 + 1;

            stageName.SetText($"{pref}-{def}");
        }

        this.mapIdx = mapIdx;
    }

    public void OnClickButton()
    {
        GameManager.Instance.MoveMapByIdx(this.mapIdx);
    }
}
