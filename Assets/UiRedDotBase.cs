using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UiRedDotBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject rootObject;

    [HideInInspector]
    public Transform moreButton;

    [SerializeField]
    private Transform targetObject;

    public void GoMoreButton()
    {
        if (moreButton == null) return;

        this.transform.parent = moreButton;
        this.transform.position = moreButton.position;
    }

    public void GoTargetButton()
    {
        if (targetObject == null) return;

        this.transform.parent = targetObject;
        this.transform.position = targetObject.position;
    }


    private void Start()
    {
        Subscribe();
    }

    protected abstract void Subscribe();
}
