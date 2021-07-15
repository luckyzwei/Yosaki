using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCurrentStageInfoButton : MonoBehaviour
{
    public void OnClickButton()
    {
        CurrentStageInfoPopup.Instance.ShowInfoPopup(true);
    }
}
