using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveSkill0 : SkillBase
{
    private float jumpPower = 50f;

    public MoveSkill0()
    {

    }

    //매직클로
    public override void UseSkill()
    {
        base.UseSkill();

        //좌우
        if (UiMoveStick.Instance.Horizontal != 0)
        {
            Vector3 rayDirection = Vector3.zero;

            if (UiMoveStick.Instance.Horizontal == 1)
            {
                rayDirection = Vector3.right;
            }
            else if (UiMoveStick.Instance.Horizontal == -1)
            {
                rayDirection = Vector3.left;
            }
            else
            {
                return;
            }

            var wallHitPoint = playerSkillCaster.GetRayHitWallPoint(playerTr.position, rayDirection, skillInfo.Targetrange);

            //캐릭터 이동
            if (wallHitPoint == Vector2.zero)
            {
                playerTr.position += (playerSkillCaster.PlayerMoveController.MoveDirection == MoveDirection.Right ? Vector3.right : Vector3.left) * skillInfo.Targetrange;
            }
            else
            {
                playerTr.position = wallHitPoint;
            }
        }
        //상하
        else
        {
            //위
            if (UiMoveStick.Instance.Vertical == 1)
            {
                var wallHitPoint = playerSkillCaster.GetRayHitPlatformPoint(playerTr.position, Vector3.up, skillInfo.Targetrange);

                if (wallHitPoint != Vector2.zero)
                {
                    playerTr.position = wallHitPoint + Vector2.up * 3f;
                }
                else
                {
                    playerTr.position += Vector3.up * skillInfo.Targetrange;
                }
            }
            //아래
            else if (UiMoveStick.Instance.Vertical == -1)
            {
                var wallHitPoint = playerSkillCaster.GetRayHitPlatformPoint(playerTr.position, Vector3.down, skillInfo.Targetrange);

                if (wallHitPoint != Vector2.zero)
                {
                    playerTr.position = wallHitPoint + Vector2.up * 2f;
                }
                else
                {
                    playerTr.position += Vector3.down * skillInfo.Targetrange;
                }
            }
        }
    }

}
