using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiReviewPopup : MonoBehaviour
{
    private bool alreadyComplete = false;
    public void OnClickButton()
    {
        if (alreadyComplete == true) return;

        alreadyComplete = true;

        SoundManager.Instance.PlayButtonSound();

        GameEscapeManager.Instance.ShowReviewPage();
    }
}
