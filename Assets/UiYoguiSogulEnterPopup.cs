using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYoguiSogulEnterPopup : MonoBehaviour
{
    public void OnClickEnterButton()
    {
        GameManager.Instance.LoadContents(GameManager.ContentsType.YoguiSoGul);
    }
}
