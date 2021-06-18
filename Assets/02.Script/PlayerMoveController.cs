using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static PlayerViewController;

public enum MoveDirection { Left, Right, Max };
public class PlayerMoveController : SingletonMono<PlayerMoveController>
{
    [SerializeField]
    private Rigidbody2D rb;
    public Rigidbody2D Rb => rb;

    [SerializeField]
    private Transform characterView;

    [SerializeField]
    private Collider2D collider2D;

    [SerializeField]
    private Transform jumpRayPos;

    private UiMoveStick uiMoveStick;

    private string horizontal = "Horizontal";
    private string vertical = "Vertical";

    private bool canJump = true;

    private bool hasDoubleJump = true;
    private bool isDoubleJump = false;
    private bool isUpJump = false;
    private int jumpCount = 0;
    private string jumpSoundName = "Jump";

    public MoveDirection MoveDirection { get; private set; }

    private void Start()
    {
        Initialize();
     
    }



    private void Initialize()
    {
        uiMoveStick = UiMoveStick.Instance;
    }

    private void ResetDownJump()
    {
        collider2D.isTrigger = false;

        canJump = true;
    }
    void Update()
    {
        MovePlayer();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && AutoManager.Instance.IsAutoMode == false)
        {
            JumpPlayer();
        }
#endif
    }
    private void MovePlayer()
    {
        if (isDoubleJump == true && isUpJump == false)
        {
            return;
        }

        Vector3 moveDirection;

        if (PlayerSkillCaster.Instance.isSkillMoveRestriction == false)
        {
            moveDirection = new Vector3(GetHorizontalAxis(), rb.velocity.y / PlayerBalance.moveSpeed);
        }
        else
        {
            moveDirection = new Vector3(0f, rb.velocity.y / PlayerBalance.moveSpeed);
        }

        rb.velocity = moveDirection * PlayerBalance.moveSpeed;

        if (rb.velocity.magnitude != 0)
        {
            PlayerViewController.Instance.SetCurrentAnimation(AnimState.run);
        }
        else
        {
            PlayerViewController.Instance.SetCurrentAnimation(AnimState.idle);
        }
    }

    public void OnClickJumpButton()
    {
        if (AutoManager.Instance.IsAutoMode == true)
        {
            //if (SkillCoolTimeManager.jumpAutoValue.Value == 1)
            //{
            //    SkillCoolTimeManager.SetJumpAuto(false);
            //    PopupManager.Instance.ShowAlarmMessage("점프 해제");
            //}
            //else
            //{
            //    SkillCoolTimeManager.SetJumpAuto(true);
            //    PopupManager.Instance.ShowAlarmMessage("점프 사용");
            //}

            return;
        }

        JumpPlayer();
    }

    public void JumpPlayer()
    {
        if (PlayerSkillCaster.Instance.isSkillMoveRestriction == true) return;

        //2단점프
        if (jumpCount == 1 && isDoubleJump == false)
        {
            UiTutorialManager.Instance.SetClear(TutorialStep._2_Jump);

            //이펙트
            EffectManager.SpawnEffect("FeatherExplosion", PlayerMoveController.Instance.transform.position);

            rb.velocity = Vector3.zero;

            float directionX = GetHorizontalAxis();
            float directionY = GetVerticalAxis();
            Vector3 jumpDirection;
            if (directionY > 0 || directionX == 0f)
            {
                jumpDirection = (Vector3.up * PlayerBalance.doubleJumpPower) + Vector3.up * 7f;
                isUpJump = true;
            }
            else
            {
                jumpDirection = (Vector3.right * directionX * PlayerBalance.doubleJumpPower) + Vector3.up * 7f;
            }

            SoundManager.Instance.PlaySound(jumpSoundName);
            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
            isDoubleJump = true;

            //  Invoke("ResetJump", 0.3f);

            DatabaseManager.goodsTable.GetTableData(GoodsTable.FeatherKey).Value++;

            return;
        }

        if (canJump == false) return;

        canJump = false;

        //하강점프
        if (GetVerticalAxis() < 0)
        {
            var rayHit = Physics2D.Raycast(jumpRayPos.position, Vector3.down, 10f, 1 << LayerMask.NameToLayer(CommonString.Platform));
            if (rayHit.collider != null && rayHit.collider.gameObject.name.Equals(CommonString.BottomBlock) == false)
            {
                collider2D.isTrigger = true;
                UiTutorialManager.Instance.SetClear(TutorialStep._3_Down);
            }
            else
            {
                canJump = true;
            }

            SoundManager.Instance.PlaySound(jumpSoundName);

            return;
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * PlayerBalance.jumpPower, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(jumpSoundName);
        jumpCount++;
    }

    private int GetVerticalAxis()
    {
#if UNITY_EDITOR
        if (AutoManager.Instance.IsAutoMode == false)
        {
            return (int)Input.GetAxisRaw(vertical);
        }
#endif
        return uiMoveStick.Vertical;
    }

    private int GetHorizontalAxis()
    {
        int value = 0;

        if (AutoManager.Instance.IsAutoMode == false)
        {
#if UNITY_EDITOR
            value = (int)Input.GetAxisRaw(horizontal);
#else
        value = uiMoveStick.Horizontal;
#endif
        }
        else
        {
            value = uiMoveStick.Horizontal;
        }

        if (value == 1)
        {
            SetMoveDirection(MoveDirection.Right);
        }
        else if (value == -1)
        {
            SetMoveDirection(MoveDirection.Left);
        }

        return value;
    }

    public void SetMoveDirection(MoveDirection direction)
    {
        MoveDirection = direction;
        FlipCharacter();
    }

    private void FlipCharacter()
    {
        if (MoveDirection == MoveDirection.Left)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < this.transform.position.y)
        {
            ResetJump();
        }
    }

    private void ResetJump()
    {
        isDoubleJump = false;
        canJump = true;
        isUpJump = false;
        jumpCount = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMasks.PlatformLayerMask)
        {
            ResetDownJump();
        }
    }

    public void AddForce(Vector3 direction, float power)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * power, ForceMode2D.Impulse);
    }


}

