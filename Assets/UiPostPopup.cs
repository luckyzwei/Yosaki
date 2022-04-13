using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TMPro;

public class UiPostPopup : MonoBehaviour
{
    [SerializeField]
    private UiPostView uipostViewPrefab;

    [SerializeField]
    private Transform viewParent;

    private List<UiPostView> postViewContainer = new List<UiPostView>();

    [SerializeField]
    private GameObject emptyText;

    [SerializeField]
    private TextMeshProUGUI loadText;

    private Coroutine textingRoutine;

    void Start()
    {
        Subscribe();
    }

    private void OnEnable()
    {
        emptyText.SetActive(false);
        loadText.gameObject.SetActive(true);
        PostManager.Instance.RefreshPost(true);

        if (textingRoutine != null)
        {
            StopCoroutine(textingRoutine);
        }

        textingRoutine = StartCoroutine(LoadTextingRoutine());
    }

    WaitForSeconds textingDelay = new WaitForSeconds(0.2f);
    private IEnumerator LoadTextingRoutine()
    {
        while (true)
        {
            loadText.SetText("우편 확인중...");
            yield return textingDelay;
            loadText.SetText("우편 확인중..");
            yield return textingDelay;
            loadText.SetText("우편 확인중.");
            yield return textingDelay;
            loadText.SetText("우편 확인중..");
            yield return textingDelay;
        }
    }

    private void Subscribe()
    {
        PostManager.Instance.WhenPostRefreshed.Subscribe(e =>
        {
            if (textingRoutine != null)
            {
                StopCoroutine(textingRoutine);
            }

            loadText.gameObject.SetActive(false);

            WhenPostRefreshed();
        }).AddTo(this);
    }

    private void WhenPostRefreshed()
    {
        emptyText.SetActive(PostManager.Instance.PostList.Count == 0);

        int interval = PostManager.Instance.PostList.Count - postViewContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var posView = Instantiate<UiPostView>(uipostViewPrefab, viewParent);
            postViewContainer.Add(posView);
        }

        for (int i = 0; i < postViewContainer.Count; i++)
        {
            if (i < PostManager.Instance.PostList.Count)
            {
                postViewContainer[i].gameObject.SetActive(true);
                postViewContainer[i].Initilaize(PostManager.Instance.PostList[i]);
            }
            else
            {
                postViewContainer[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickAllReceiveButton()
    {
        int receiveCount = 0;

        for (int i = 0; i < postViewContainer.Count; i++)
        {
            if (postViewContainer[i].gameObject.activeInHierarchy)
            {
                postViewContainer[i].OnClickReceiveButton();
                receiveCount++;
            }
        }

        if (receiveCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("우편이 없습니다.");
        }
    }
}
