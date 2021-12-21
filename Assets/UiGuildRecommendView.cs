using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildRecommendView : MonoBehaviour
{
    [SerializeField]
    private List<UiGuildListCell> guildListCell;

    [SerializeField]
    private GameObject loadFailObject;

    [SerializeField]
    private Button refreshButton;

    void Start()
    {
        RefreshGuildList();
    }

    private void RefreshGuildList()
    {
        var bro = Backend.Social.Guild.GetRandomGuildInfoV3(5);

        loadFailObject.SetActive(!bro.IsSuccess());

        if (bro.IsSuccess())
        {
            int rowCount = bro.Rows().Count;

            for (int i = 0; i < guildListCell.Count; i++)
            {
                if (i < rowCount)
                {
                    guildListCell[i].gameObject.SetActive(true);
                    guildListCell[i].Initialize(bro.Rows()[i]);
                }
                else
                {
                    guildListCell[i].gameObject.SetActive(false);
                }
            }

            ////example
            //for (int i = 0; i < bro.Rows().Count; i++)
            //{

            //}
        }
        else
        {

        }


    }
    private Coroutine refreshDelay;
    private WaitForSeconds refreshDelayWs = new WaitForSeconds(1.0f);
    private IEnumerator RefreshRoutine()
    {
        if (refreshButton != null)
        {
            refreshButton.interactable = false;
        }

        yield return refreshDelayWs;

        if (refreshButton != null)
        {
            refreshButton.interactable = true;
        }

        refreshDelay = null;
    }
    public void OnClickRefreshButton()
    {
        if (refreshDelay == null)
        {
            refreshDelay = CoroutineExecuter.Instance.StartCoroutine(RefreshRoutine());
            RefreshGuildList();
        }
    }
}
