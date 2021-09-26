#if UNITY_ANDROID
using Google.Play.Review;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEscapeManager : SingletonMono<GameEscapeManager>
{
    [SerializeField]
    private GameObject popupObject;

    public void WhenEscapeInputReceived()
    {
        popupObject.SetActive(!popupObject.activeInHierarchy);

        if (popupObject.activeInHierarchy)
        {
            this.transform.SetAsFirstSibling();
        }
    }

    public void OnClickApplicationQuitButton()
    {
        Application.Quit();
    }

    public void ShowReviewPage()
    {
#if UNITY_ANDROID
        var reviewManager = new ReviewManager();

        // start preloading the review prompt in the background
        var playReviewInfoAsyncOperation = reviewManager.RequestReviewFlow();

        // define a callback after the preloading is done
        playReviewInfoAsyncOperation.Completed += playReviewInfoAsync =>
        {
            if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
            {
                // display the review prompt
                var playReviewInfo = playReviewInfoAsync.GetResult();
                reviewManager.LaunchReviewFlow(playReviewInfo);

                //여기서 보상 주던지

                //
            }
            else
            {
                Debug.LogError($"Review fail error {playReviewInfoAsync.Error}");
                // handle error when loading review prompt
            }

        };
#endif
    }

    public void OnClickCafeButton() 
    {
        Application.OpenURL(CommonString.CafeURL);
    }
}
