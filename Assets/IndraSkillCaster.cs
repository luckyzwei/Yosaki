using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;



public class IndraSkillCaster : SingletonMono<IndraSkillCaster>
{
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
    private static string weaponName = "weapon26";
    private IEnumerator UserSonSkillRoutine()
    {
        yield break;

        var skillTableDatas = TableManager.Instance.SkillTable.dataArray;

        while (true)
        {
            if (ServerData.weaponTable.TableDatas[weaponName].hasItem.Value != 0)
            {
                PlayerSkillCaster.Instance.UseSkill(skillTableDatas[18].Id);
            }

            yield return null;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            StopCoroutine(skillRoutine);
        }

    }
#endif
}
