using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using static GameManager;

public class Portal : MonoBehaviour
{
    public bool isNextPortal;

    private void Start()
    {
        if (isNextPortal == true)
        {
            this.gameObject.SetActive(!GameManager.Instance.IsLastScene());
        }
        else if (isNextPortal == false)
        {
            this.gameObject.SetActive(!GameManager.Instance.IsFirstScene());
        }

        SetPlayerPosToPortal();
    }

    private void SetPlayerPosToPortal()
    {
        if (isNextPortal && GameManager.Instance.initPlayerPortalPosit == InitPlayerPortalPosit.Right)
        {
            PlayerMoveController.Instance.transform.position = this.transform.position;
            PlayerMoveController.Instance.transform.position += Vector3.left * 2f;
        }


        if (isNextPortal == false && GameManager.Instance.initPlayerPortalPosit == InitPlayerPortalPosit.Left)
        {
            PlayerMoveController.Instance.transform.position = this.transform.position;
            PlayerMoveController.Instance.transform.position += Vector3.right * 2f;
        }
    }

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
            if (isNextPortal)
            {
                GameManager.Instance.LoadNextScene();
                return;
            }
            else
            {
                GameManager.Instance.LoadBackScene();
                return;
            }
        }

        //버튼
        if (AutoManager.Instance.IsAutoMode == false)
        {
            UiReactionButton.Instance.Show(true);

            if (AutoManager.Instance.IsAutoMode == false)
            {
                if (isNextPortal)
                {
                    UiReactionButton.Instance.Initialize("다음맵", GameManager.Instance.LoadNextScene);
                    return;
                }
                else
                {
                    UiReactionButton.Instance.Initialize("이전맵", GameManager.Instance.LoadBackScene);
                    return;
                }
            }
        }
        else
        {
            UiReactionButton.Instance.Show(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UiReactionButton.Instance.Show(false);
    }
}
