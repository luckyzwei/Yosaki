using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonSkillCaster : SingletonMono<SonSkillCaster>
{
    private WaitForSeconds ws = new WaitForSeconds(0.1f);
    IEnumerator Start()
    {
        //세팅시간 필요 (1프레임)
        yield return null;
        
        StartCoroutine(UserSonSkillRoutine());
    }

    public void SonSkillAnim()
    {
        
    }

    private IEnumerator UserSonSkillRoutine()
    {
        var skillTableDatas = TableManager.Instance.SkillData;

        while (true)
        {
            int sonLevel = ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value;

            for (int i = 0; i < skillTableDatas.Count; i++)
            {
                if (skillTableDatas[i].Issonskill == false) continue;
                if (sonLevel < skillTableDatas[i].Sonunlocklevel) continue;

                PlayerSkillCaster.Instance.UseSkill(skillTableDatas[i].Id);
            }

            yield return ws;
        }
    }

}
