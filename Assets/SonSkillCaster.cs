using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SonSkillCaster : SingletonMono<SonSkillCaster>
{
    private WaitForSeconds ws = new WaitForSeconds(0.1f);

    private Coroutine skillRoutine;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        AutoManager.Instance.AutoMode.AsObservable().Subscribe(e =>
        {
            if (e)
            {
                if (skillRoutine != null)
                {
                    StopCoroutine(skillRoutine);
                }

                skillRoutine = StartCoroutine(UserSonSkillRoutine());
            }
            else
            {
                if (skillRoutine != null)
                {
                    StopCoroutine(skillRoutine);
                }
            }
        }).AddTo(this);
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
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StopCoroutine(skillRoutine);
        }

    }
#endif

}
