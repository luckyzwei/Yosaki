using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using static GameManager;

public class UiContentsLockMask : MonoBehaviour
{
    [SerializeField]
    private ContentsType contentsType;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        SetDescription();

        Subscribe();
    }

    private void SetDescription()
    {
        switch (contentsType)
        {
            case ContentsType.FireFly:
                {
                    description.SetText($"{GameBalance.bonusDungeonUnlockLevel}레벨에 오픈!");
                }
                break;
            case ContentsType.Boss:
                {
                    description.SetText($"{GameBalance.bossUnlockLevel}레벨에 오픈!");
                }
                break;
            case ContentsType.InfiniteTower:
                {
                    description.SetText($"{GameBalance.InfinityDungeonUnlockLevel}레벨에 오픈!");
                }
                break;
        }
    }
    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(currentLevel =>
        {
            switch (contentsType)
            {
                case ContentsType.FireFly:
                    {
                        rootObject.SetActive(currentLevel < GameBalance.bonusDungeonUnlockLevel);
                        description.SetText($"{GameBalance.bonusDungeonUnlockLevel}레벨에 오픈!");
                    }
                    break;
                case ContentsType.Boss:
                    {
                        rootObject.SetActive(currentLevel < GameBalance.bossUnlockLevel);
                        description.SetText($"{GameBalance.bossUnlockLevel}레벨에 오픈!");
                    }
                    break;
                case ContentsType.InfiniteTower:
                    {
                        rootObject.SetActive(currentLevel < GameBalance.InfinityDungeonUnlockLevel);
                        description.SetText($"{GameBalance.InfinityDungeonUnlockLevel}레벨에 오픈");
                    }
                    break;
            }
        }).AddTo(this);
    }
}