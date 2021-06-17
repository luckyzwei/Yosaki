using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLoading : MonoBehaviour
{
    [SerializeField]
    private Image tipIcon;
    [SerializeField]
    private TextMeshProUGUI description;

    private IEnumerator Start()
    {
        SetLoadingDesc();

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(2);
        asyncOper.allowSceneActivation = false;

        while (!asyncOper.isDone)
        {
            yield return null;

            if (asyncOper.progress >= 0.9f)
            {
                yield return new WaitForSeconds(2.0f);
                asyncOper.allowSceneActivation = true;
                break;
            }
        }



        PopupManager.Instance.PlayFade();
    }

    private void SetLoadingDesc()
    {
        int randIdx = Random.Range(0, TableManager.Instance.LoadingTip.dataArray.Length);

        var tableData = TableManager.Instance.LoadingTip.dataArray[randIdx];

        description.SetText(tableData.Description);

        tipIcon.sprite = CommonUiContainer.Instance.loadingTipIcon[randIdx];
    }

}
