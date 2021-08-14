using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UiMoveStick : SingletonMono<UiMoveStick>
{
    public enum InputType
    {
        Top, Down, Left, Right, Common
    }
    [SerializeField]
    private List<GameObject> arrowSprites;

    public int Horizontal { get; private set; }
    public int Vertical { get; private set; }

    private Transform playerTr;

    private ObscuredFloat quickMoveRange_Common = 8f;
    private ObscuredFloat quickMoveRange_Down = 5f;

    private readonly WaitForSeconds doubleInputDelay = new WaitForSeconds(0.5f);
    private const float quickMoveDelaySec = 0.5f;
    private readonly WaitForSeconds quickMoveDelay = new WaitForSeconds(quickMoveDelaySec);

    [SerializeField]
    private Image quickMoveDelayGauge;

    public bool nowTouching { get; private set; } = false;

    //[SerializeField]
    //private GameObject autoObject;

    //[SerializeField]
    //private GameObject autoToggleObject;

    private void Start()
    {
        playerTr = PlayerMoveController.Instance.transform;

        Subscribe();

        StartCoroutine(TouchCountCheckRoutine());
    }

    private IEnumerator TouchCountCheckRoutine()
    {
        var ws = new WaitForSeconds(1.0f);

        while (true)
        {
#if !UNITY_EDITOR
            if (Input.touchCount == 0)
            {
                EndTouch();
            }
#endif
            yield return ws;
        }
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

    private Dictionary<InputType, bool> downInputContainter = new Dictionary<InputType, bool>()
    {
        { InputType.Top,false},
        { InputType.Down,false},
        { InputType.Left,false},
        { InputType.Right,false}
    };

    private Dictionary<InputType, bool> QuickMoveDelayContainter = new Dictionary<InputType, bool>()
    {
        { InputType.Common,false},
    };

    private IEnumerator InputDelay(InputType type)
    {
        downInputContainter[type] = true;
        yield return doubleInputDelay;
        downInputContainter[type] = false;
    }

    private IEnumerator QuickMoveDelay(InputType type)
    {
        float tick = 0f;

        QuickMoveDelayContainter[type] = true;

        while (tick < quickMoveDelaySec)
        {
            tick += Time.deltaTime;
            quickMoveDelayGauge.fillAmount = tick / quickMoveDelaySec;
            yield return null;
        }

        quickMoveDelayGauge.fillAmount = 1f;
        QuickMoveDelayContainter[type] = false;
    }

    private void ReceiveDownEvent(InputType type)
    {
        if (QuickMoveDelayContainter[InputType.Common] == true) return;

        if (downInputContainter[type] == true)
        {
            StartCoroutine(QuickMoveDelay(InputType.Common));

            QuickMove(type);
        }
        else
        {
            StartCoroutine(InputDelay(type));
        }
    }

    private const string TeleportEfxName = "Teleport";
    private void QuickMove(InputType type)
    {
        EffectManager.SpawnEffectAllTime(TeleportEfxName, playerTr.position + Vector3.down * 1f);
        SoundManager.Instance.PlaySound(TeleportEfxName);

        switch (type)
        {
            case InputType.Top:
                {
                    var wallHitPoint = PlayerSkillCaster.Instance.GetRayHitPlatformPoint(playerTr.position, Vector3.up, quickMoveRange_Common);

                    if (wallHitPoint != Vector2.zero)
                    {
                        playerTr.position = wallHitPoint + Vector2.up * 3f;
                    }
                    else
                    {
                        playerTr.position += Vector3.up * quickMoveRange_Common;
                    }
                }
                break;
            case InputType.Down:
                {
                    if (BottomTeleportDetector.Instance.triggered == false)
                    {
                        playerTr.position += Vector3.down * quickMoveRange_Down;
                    }
                    else
                    {
                        var wallHitPoint = PlayerSkillCaster.Instance.GetRayHitPlatformPoint(playerTr.position, Vector3.down, quickMoveRange_Down);

                        if (wallHitPoint != Vector2.zero)
                        {
                            playerTr.position = wallHitPoint + Vector2.up * 1f;
                        }
                        else
                        {
                            playerTr.position += Vector3.down * quickMoveRange_Down;
                        }
                    }
                }
                break;
            case InputType.Left:
                {
                    var wallHitPoint = PlayerSkillCaster.Instance.GetRayHitWallPoint(playerTr.position, Vector2.left, quickMoveRange_Common);

                    //캐릭터 이동
                    if (wallHitPoint == Vector2.zero)
                    {
                        playerTr.position += Vector3.left * quickMoveRange_Common;
                    }
                    else
                    {
                        playerTr.position = wallHitPoint;
                    }
                }
                break;
            case InputType.Right:
                {
                    var wallHitPoint = PlayerSkillCaster.Instance.GetRayHitWallPoint(playerTr.position, Vector2.right, quickMoveRange_Common);

                    //캐릭터 이동
                    if (wallHitPoint == Vector2.zero)
                    {
                        playerTr.position += Vector3.right * quickMoveRange_Common;
                    }
                    else
                    {
                        playerTr.position = wallHitPoint;
                    }
                }
                break;
        }


        UiTutorialManager.Instance.SetClear(TutorialStep.UseTelePort);
        EffectManager.SpawnEffectAllTime(TeleportEfxName, playerTr.position + Vector3.down * 1f);
    }

    public void TeleportToTop()
    {
        Top_downEvent();
        Top_downEvent();
    }
    public void TeleportToBottom()
    {
        Down_downEvent();
        Down_downEvent();
    }

    public void Top_downEvent()
    {
        ReceiveDownEvent(InputType.Top);
    }
    public void Down_downEvent()
    {
        ReceiveDownEvent(InputType.Down);
    }

    public void Left_downEvent()
    {
        ReceiveDownEvent(InputType.Left);
    }

    public void Right_downEvent()
    {
        ReceiveDownEvent(InputType.Right);
    }

    public void Top()
    {
        nowTouching = true;
        Vertical = 1;
        Horizontal = 0;

        SetArrowSprites(0);
    }
    public void Down()
    {
        nowTouching = true;
        Vertical = -1;
        Horizontal = 0;

        SetArrowSprites(1);
    }
    public void Left()
    {
        nowTouching = true;
        Horizontal = -1;
        Vertical = 0;

        SetArrowSprites(2);
    }
    public void Right()
    {
        nowTouching = true;
        Horizontal = 1;
        Vertical = 0;

        SetArrowSprites(3);
    }

    public void EndTouch()
    {
        Vertical = 0;
        Horizontal = 0;
        nowTouching = false;
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
    }
    private void OffArrowSprites()
    {
        for (int i = 0; i < arrowSprites.Count; i++)
        {
            arrowSprites[i].SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Top_downEvent();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down_downEvent();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left_downEvent();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right_downEvent();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Top();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Down();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Left();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Right();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            EndTouch();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            EndTouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            EndTouch();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            EndTouch();
        }
    }
#endif


}
