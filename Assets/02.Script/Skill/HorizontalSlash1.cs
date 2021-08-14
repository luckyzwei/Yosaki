using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HorizontalSlash1 : HorizontalSlash0
{
    private float moveDistance;

    protected float boxSize = 4;

    protected bool attractEnemy = false;

    public HorizontalSlash1()
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

        Vector3 skillDir = PlayerMoveController.Instance.MoveDirection == MoveDirection.Right ? Vector3.right : Vector3.left;

        var hitEnemies_1 = playerSkillCaster.GetEnemiesInBoxcast(playerTr.position, skillDir, skillInfo.Targetrange, boxSize);
        //var hitEnemies_2 = playerSkillCaster.GetEnemiesInBoxcast(playerTr.position, Vector3.left, skillInfo.Targetrange, boxSize);


        Dictionary<int, RaycastHit2D> hitEnemiesDic = new Dictionary<int, RaycastHit2D>();

        for (int i = 0; i < hitEnemies_1.Length; i++)
        {
            hitEnemiesDic.Add(hitEnemies_1[i].collider.gameObject.GetInstanceID(), hitEnemies_1[i]);
        }

        //for (int i = 0; i < hitEnemies_2.Length; i++)
        //{
        //    int instanceId = hitEnemies_2[i].collider.gameObject.GetInstanceID();

        //    if (hitEnemiesDic.ContainsKey(instanceId) == true) continue;

        //    hitEnemiesDic.Add(hitEnemies_2[i].collider.gameObject.GetInstanceID(), hitEnemies_2[i]);
        //}

        List<RaycastHit2D> hitEnemies = hitEnemiesDic.Select(e => e.Value).ToList();

        //파티클
        CoroutineExecuter.Instance.StartCoroutine(SpawnLineEffect());

        //데미지적용
        for (int i = 0; i < hitEnemies.Count && i < skillInfo.Targetcount; i++)
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
