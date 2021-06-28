using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;

public class UiMoveStick : SingletonMono<UiMoveStick>
{
    [SerializeField]
    private List<GameObject> arrowSprites;

    [SerializeField]
    private GameObject handleObject;

    public int Horizontal { get; private set; }
    public int Vertical { get; private set; }

    //[SerializeField]
    //private GameObject autoObject;

    //[SerializeField]
    //private GameObject autoToggleObject;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        //AutoManager.Instance.AutoMode.AsObservable().Subscribe(WhenAutoModeChanged).AddTo(this);

        //SkillCoolTimeManager.moveAutoValue.AsObservable().Subscribe(e =>
        //{
        //    autoToggleObject.SetActive(e == 1);
        //}).AddTo(this);
    }

    private void WhenAutoModeChanged(bool auto)
    {
        return;
        //autoObject.SetActive(auto);

        if (auto)
        {
            OffArrowSprites();
            EndTouch();
        }
    }

    public void OnPointerDown()
    {
        SetMoveAuto();
    }

    //기능 꺼둠
    private void SetMoveAuto()
    {
        return;

        if (SkillCoolTimeManager.moveAutoValue.Value == 0)
        {
            SkillCoolTimeManager.SetMoveAuto(true);
        }
        else
        {
            SkillCoolTimeManager.SetMoveAuto(false);
        }
    }

    public void Top()
    {
        if (AutoManager.Instance.IsAutoMode == true)
        {
            return;
        }

        Vertical = 1;
        Horizontal = 0;

        SetArrowSprites(0);
    }
    public void Down()
    {
        if (AutoManager.Instance.IsAutoMode == true)
        {
            return;
        }

        Vertical = -1;
        Horizontal = 0;

        SetArrowSprites(1);
    }
    public void Left()
    {
        if (AutoManager.Instance.IsAutoMode == true)
        {
            return;
        }

        Horizontal = -1;
        Vertical = 0;

        SetArrowSprites(2);
    }
    public void Right()
    {
        if (AutoManager.Instance.IsAutoMode == true)
        {
            return;
        }

        Horizontal = 1;
        Vertical = 0;

        SetArrowSprites(3);
    }

    public void EndTouch()
    {
        Vertical = 0;
        Horizontal = 0;
        OffArrowSprites();
    }

    public void SetHorizontalAxsis(int horizontal)
    {
        Horizontal = horizontal;
    }
    public void SetVerticalAxsis(int vertical)
    {
        Vertical = vertical;
    }

    private void SetArrowSprites(int idx)
    {
        for (int i = 0; i < arrowSprites.Count; i++)
        {
            arrowSprites[i].SetActive(i == idx);
        }

        handleObject.transform.position = Input.mousePosition;
    }
    private void OffArrowSprites()
    {
        for (int i = 0; i < arrowSprites.Count; i++)
        {
            arrowSprites[i].SetActive(false);
        }
    }

 
}
