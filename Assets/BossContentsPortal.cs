using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContentsPortal : MonoBehaviour
{
    [SerializeField]
    private GameObject contentsPopupPrefab;

    private GameObject contentsPopup;

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool isInputEnter = UiMoveStick.Instance.Vertical > 0;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isInputEnter = true;
        }
#endif

        if (isInputEnter && AutoManager.Instance.IsAutoMode == false)
        {
            ShowBossPopup();
        }

        //버튼
        if (AutoManager.Instance.IsAutoMode == false)
        {
            UiReactionButton.Instance.Show(true);

            UiReactionButton.Instance.Initialize("보스전", ShowBossPopup);
        }
        else
        {
            UiReactionButton.Instance.Show(false);
        }

        if (contentsPopup != null && contentsPopup.activeInHierarchy)
        {
            UiReactionButton.Instance.Show(false);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        UiReactionButton.Instance.Show(false);
    }

    public void ShowBossPopup()
    {
        if (contentsPopup == null)
        {
            contentsPopup = Instantiate<GameObject>(contentsPopupPrefab, InGameCanvas.Instance.transform);
        }

        contentsPopup.gameObject.SetActive(true);
    }
}
