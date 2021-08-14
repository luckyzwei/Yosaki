using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSlash0 : SkillBase
{
    private float moveDistance;

    protected float boxSize = 4;

    protected bool attractEnemy = false;

    public HorizontalSlash0()
    {
        damageApplyInterval = new WaitForSeconds(0.1f);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        //발동 이펙트

        //+이펙트

        //이동제한있을경우
        playerSkillCaster.SetMoveRestriction(skillInfo.Movedelay);

        //데미지
        float damage = GetSkillDamage(skillInfo);

        Vector3 rayDirection = playerSkillCaster.PlayerMoveController.MoveDirection == MoveDirection.Right ? Vector3.right : Vector3.left;

        var hitEnemies = playerSkillCaster.GetEnemiesInBoxcast(playerTr.position, rayDirection, skillInfo.Targetrange, boxSize);
        var wallHitPoint = playerSkillCaster.GetRayHitWallPoint(playerTr.position, rayDirection, skillInfo.Targetrange);

        //파티클
        CoroutineExecuter.Instance.StartCoroutine(SpawnLineEffect());

        ////캐릭터 이동
        //if (wallHitPoint == Vector2.zero)
        //{
        //    playerTr.position += (playerSkillCaster.PlayerMoveController.MoveDirection == MoveDirection.Right ? Vector3.right : Vector3.left) * skillInfo.Targetrange;
        //}
        //else
        //{
        //    playerTr.position = wallHitPoint;
        //}

        //이동후 파티클
        //EffectManager.SpawnEffect("FeatherExplosion", PlayerMoveController.Instance.transform.position);

        //데미지적용
        for (int i = 0; i < hitEnemies.Length && i < skillInfo.Targetcount; i++)
        {
            PlayerSkillCaster.Instance.StartCoroutine(playerSkillCaster.ApplyDamage(hitEnemies[i].collider, skillInfo, damage, damageApplyInterval, i == 0));

            //끌어모음
            if (attractEnemy && hitEnemies[i].transform.tag.Equals(Tags.Boss) == false)
            {
                if (playerSkillCaster.PlayerMoveController.MoveDirection == MoveDirection.Right)
                {
                    hitEnemies[i].transform.position = playerTr.position - Vector3.right * 4f;
                }
                else
                {
                    hitEnemies[i].transform.position = playerTr.position + Vector3.right * 4f;
                }
            }
        }
    }

    private IEnumerator SpawnLineEffect()
    {
        var effect = EffectManager.SpawnEffectAllTime("LightningFloorYellowTrail", PlayerMoveController.Instance.transform.position);

        yield return null;

        if (effect != null)
        {
            effect.transform.position = PlayerMoveController.Instance.transform.position;
        }
    }
}
