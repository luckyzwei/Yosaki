using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGradeTestFrameView : MonoBehaviour
{
    [SerializeField]
    private Image icon;


    void Start()
    {
        SetIcon();
    }

    private void SetIcon()
    {
        int currentIdx = PlayerStats.GetGradeTestGrade();

        if (currentIdx == -1)
        {
            icon.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = Resources.Load<Sprite>($"GradeTest/{currentIdx}");
    }

}
