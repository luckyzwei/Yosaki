using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiPostRedDot : UiRedDotBase
{
    protected override void Subscribe()
    {
        rootObject.SetActive(PostManager.Instance.PostList.Count > 0);

        PostManager.Instance.WhenPostRefreshed.AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(PostManager.Instance.PostList.Count > 0);
        }).AddTo(this);
    }

}
