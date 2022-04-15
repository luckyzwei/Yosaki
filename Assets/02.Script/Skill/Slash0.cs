using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slash0 : SkillBase
{
    private WaitForSeconds damageApplyInterval_0 = new WaitForSeconds(0.07f);

    public Slash0()
    {
    }

    //매직클로
    public override void UseSkill()
    {
        base.UseSkill();

        Vector3 overlapCircleOrigin = playerTr.position + playerSkillCaster.GetSkillCastingPosOffset(skillInfo);

        // var hitEnemies = playerSkillCaster.GetEnemiesInCircle(overlapCircleOrigin, skillInfo.Targetrange);

        Vector3 rayDirection = playerSkillCaster.PlayerMoveController.MoveDirection == MoveDirection.Right ? Vector3.right : Vector3.left;

        Vector3 skillCastPos = playerTr.position + rayDirection;

        var hitEnemies = playerSkillCaster.GetEnemiesInBoxcast(skillCastPos, rayDirection, skillInfo.Targetrange, 4f).Select(e => e.collider).ToArray();

        //발동 이펙트
        //+이펙트

        //이동제한있을경우
        playerSkillCaster.SetMoveRestriction(skillInfo.Movedelay);

        //데미지
        double damage = GetSkillDamage(skillInfo);

        //데미지적용
        for (int i = 0; i < hitEnemies.Length && i < skillInfo.Targetcount; i++)
        {
            PlayerSkillCaster.Instance.StartCoroutine(playerSkillCaster.ApplyDamage(hitEnemies[i], skillInfo, damage, i == 0));
        }
    }

}
