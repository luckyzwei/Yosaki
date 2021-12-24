using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiGuildRankBoard;

public class UiGuildTopRankView : SingletonMono<UiGuildTopRankView>
{
    [SerializeField]
    private List<UiGuildTopRankerCell> topRankerCells;

    public int currentIndex;

    private new void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        topRankerCells.ForEach(e => e.gameObject.SetActive(false));
    }

    public void SetTopRankInfo(GuildRankInfo rankInfo)
    {
        if (currentIndex < topRankerCells.Count)
        {
            topRankerCells[currentIndex].gameObject.SetActive(true);
            topRankerCells[currentIndex].Initialize(rankInfo);
        }

        currentIndex++;
    }

}
