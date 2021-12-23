using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildInfoChangeBoard : MonoBehaviour
{
    [SerializeField]
    private UiGuildIconCell uiGuildIconCell;

    [SerializeField]
    private Transform cellParent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var guildIconList = CommonUiContainer.Instance.guildIcon;

        for (int i = 0; i < guildIconList.Count; i++)
        {
            var cell = Instantiate<UiGuildIconCell>(uiGuildIconCell, cellParent);
            cell.Initialize(i);
        }
    }
}
