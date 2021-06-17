using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyMoveController : EnemyMoveBase
{
    public enum MoveState
    {
        Normal, FollowPlayer
    }

    private ReactiveProperty<MoveDirection> moveDirectionType = new ReactiveProperty<MoveDirection>();

    private Vector3 moveDirection;

    [SerializeField]
    private Transform characterView;

    public static string EnemyWall_str = "EnemyWall";
    public static string DefenseWall_str = "DefenseWall";

    private bool nowKnockBack = false;
    //이 시간값을 테이블에 넣으면 좋을듯 (슈퍼아머도 만들고)
    private WaitForSeconds knockBackTime = new WaitForSeconds(0.2f);

    private ReactiveProperty<MoveState> moveState = new ReactiveProperty<MoveState>();
    private Coroutine returnState;
    private WaitForSeconds returnStateDelay = new WaitForSeconds(8f);
    private ObscuredFloat followMoveSpeedAddValue = 1.5f;

    private new void Awake()
    {
        base.Awake();
        Subscribe();
    }
    private new void Start()
    {
        base.Start();
        SetRandomZPos();
    }

    public void SetMoveState(MoveState moveState)
    {
        this.moveState.Value = moveState;

        if (moveState == MoveState.FollowPlayer)
        {
            if (returnState != null)
            {
                StopCoroutine(returnState);
            }

            returnState = StartCoroutine(ReturnNormalState());

            //이동속도 증가
            WhenDirectionChanged(moveDirectionType.Value);
        }
    }

    private IEnumerator ReturnNormalState()
    {
        yield return returnStateDelay;
        SetMoveState(MoveState.Normal);
    }

    private void SetRandomZPos()
    {
        characterView.transform.localPosition = new Vector3(characterView.transform.localPosition.x, characterView.transform.localPosition.y, Random.Range(-1f, -0.01f));
    }

    private void ResetState()
    {
        nowKnockBack = false;
        moveState.Value = MoveState.Normal;
    }

    private IEnumerator MoveRoutine()
    {
        WaitForSeconds ws = new WaitForSeconds(1.0f);

        SetRunAnimation();

        while (true)
        {
            switch (moveState.Value)
            {
                case MoveState.Normal:
                    {

                    }
                    break;
                case MoveState.FollowPlayer:
                    {
                        FollowPlayer();
                    }
                    break;
            }

            MoveCharcterByDirection();

            yield return ws;
        }
    }

    protected virtual void FollowPlayer()
    {
        var playerPositionX = PlayerMoveController.Instance.transform.position.x;

        moveDirectionType.Value = this.transform.position.x > playerPositionX ? MoveDirection.Left : MoveDirection.Right;
    }

    private void MoveCharcterByDirection()
    {
        bool canMove = nowKnockBack == false;

        if (canMove)
        {
            rb.velocity = moveDirection;
        }
    }

    private void Subscribe()
    {
        enemyInfo.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {
                moveDirectionType.AsObservable().Subscribe(WhenDirectionChanged).AddTo(this);
                WhenDirectionChanged(moveDirectionType.Value);
                MoveCharcterByDirection();
            }
        }).AddTo(this);

    }



    protected virtual void WhenDirectionChanged(MoveDirection moveDirectionType)
    {
        float moveSpeed = enemyInfo.Value.Movespeed;

        if (moveState.Value == MoveState.FollowPlayer)
        {
            moveSpeed += followMoveSpeedAddValue;
        }

        moveDirection = moveDirectionType == MoveDirection.Left ? Vector3.left : Vector3.right;

        moveDirection *= moveSpeed;

        FlipCharacter();

        SetRunAnimation();
    }

    private void SetRunAnimation()
    {
        //  animator.SetBool(CommonString.RunAnimKey, true);
    }

    private void FlipCharacter()
    {
        if (moveDirectionType.Value == MoveDirection.Left)
        {
            characterView.transform.localScale = Vector3.one;
            characterView.transform.rotation = Quaternion.identity;
        }
        else
        {
            characterView.transform.localScale = new Vector3(1f, 1f, -1f);
            characterView.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void OnEnable()
    {
        ResetState();
        SetRandomDirection();
        StartCoroutine(MoveRoutine());
    }



    private void SetRandomDirection()
    {
        moveDirectionType.Value = (MoveDirection)Random.Range((int)MoveDirection.Left, (int)MoveDirection.Max);
    }

    private void SetReverseDirection()
    {
        moveDirectionType.Value = moveDirectionType.Value == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (moveState.Value)
        {
            case MoveState.Normal:
                {
                    if (collision.gameObject.layer == LayerMask.NameToLayer(EnemyWall_str))
                    {
                        SetReverseDirection();
                        MoveCharcterByDirection();
                    }
                }
                break;
            case MoveState.FollowPlayer:
                {


                }
                break;
        }
    }

    public void SetKnockBack()
    {
        if (nowKnockBack == true) return;
        //슈퍼아머
        if (enemyInfo.Value==null || enemyInfo.Value.Knockbackpower == 0f) return;

        nowKnockBack = true;

        Vector2 knockBackDir = this.transform.position.x < PlayerMoveController.Instance.transform.position.x ? Vector2.left : Vector2.right;

        knockBackDir += Vector2.up * 0.5f;

        rb.velocity = Vector2.zero;
        rb.AddForce(knockBackDir * enemyInfo.Value.Knockbackpower, ForceMode2D.Impulse);

        StartCoroutine(KnockBackRoutine());
    }

    private IEnumerator KnockBackRoutine()
    {
        yield return knockBackTime;
        nowKnockBack = false;
        rb.velocity = Vector2.zero;
    }
}
