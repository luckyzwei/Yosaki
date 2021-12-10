using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniGameMoveArea : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
{
    [SerializeField]
    private GameObject player;

    private Vector2 prefTouchPos = Vector2.zero;

    [SerializeField]
    private Transform min;

    [SerializeField]
    private Transform max;

    private void SetPrefTouchPos()
    {
        prefTouchPos = GetCurrentTouchWorldPos();
    }
    private Vector2 GetCurrentTouchWorldPos()
    {
        Vector3 touchPos;
#if UNITY_EDITOR
        touchPos = Input.mousePosition;
#endif
#if !UNITY_EDITOR
        touchPos = Input.GetTouch(0).position;
#endif

        touchPos = InGameCanvas.Instance.MainCam.ScreenToWorldPoint(touchPos);

        return touchPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UiMinigameBoard.Instance.GameState_Cur != UiMinigameBoard.MiniGameState.Playing) return;

        if (prefTouchPos == Vector2.zero)
        {
            SetPrefTouchPos();
        }

        Vector2 currentTouchPos = GetCurrentTouchWorldPos();
        Vector3 moveDir = (GetCurrentTouchWorldPos() - prefTouchPos).normalized;
        float distance = Vector2.Distance(prefTouchPos, currentTouchPos);

        player.transform.position += moveDir * distance;

        float clampedX = Mathf.Clamp(player.transform.position.x, min.position.x, max.position.x);
        float clampedY = Mathf.Clamp(player.transform.position.y, min.position.y, max.position.y);

        player.transform.position = new Vector3(clampedX, clampedY, 0f);

        SetPrefTouchPos();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {

        prefTouchPos = Vector2.zero;

    }
}
