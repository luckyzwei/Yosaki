using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class UiPostPopup : MonoBehaviour
{
    [SerializeField]
    private UiPostView uipostViewPrefab;

    [SerializeField]
    private Transform viewParent;

    private List<UiPostView> postViewContainer = new List<UiPostView>();

    [SerializeField]
    private GameObject emptyText;

    void Start()
    {
        Subscribe();
    }

    private void OnEnable()
    {
        PostManager.Instance.RefreshPost(true);
    }

    private void Subscribe()
    {
        PostManager.Instance.WhenPostRefreshed.Subscribe(e =>
        {
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
}
