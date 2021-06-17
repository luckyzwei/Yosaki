using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : SkillBase
{
    public Skill2()
    {
        damageApplyInterval = new WaitForSeconds(0.07f);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        //데미지
        float damage = GetSkillDamage(skillInfo);

        var hitEnemies = playerSkillCaster.GetEnemiesInCircle(playerTr.position,skillInfo.Targetrange);

        //데미지적용
        for (int i = 0; i < hitEnemies.Length && i < skillInfo.Targetcount; i++)
        {
            PlayerSkillCaster.Instance.StartCoroutine(playerSkillCaster.ApplyDamage(hitEnemies[i], skillInfo, damage, damageApplyInterval));
        }

    }

}
