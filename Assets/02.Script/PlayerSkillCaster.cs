using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerSkillCaster : SingletonMono<PlayerSkillCaster>
{
    [SerializeField]
    private PlayerMoveController playerMoveController;
    public PlayerMoveController PlayerMoveController => playerMoveController;

    public Dictionary<int, SkillBase> UserSkills { get; private set; } = new Dictionary<int, SkillBase>();

    public bool isSkillMoveRestriction = false;

    private ObscuredBool ignoreDamDecrease = false;

    public bool UseSkill(int skillIdx)
    {
        bool canUserSkill = UserSkills[skillIdx].CanUseSkill();

        if (canUserSkill)
        {
            UserSkills[skillIdx].UseSkill();
        }

        return canUserSkill;
    }

    private void Start()
    {
        InitSkill();

        ignoreDamDecrease = ServerData.userInfoTable.TableDatas[UserInfoTable.IgnoreDamDec].Value == 1;
    }

    public void SetMoveRestriction(float time)
    {
        if (time == 0f) return;

        StartCoroutine(MoveRestrictionRoutine(time));
    }

    private IEnumerator MoveRestrictionRoutine(float time)
    {
        isSkillMoveRestriction = true;
        yield return new WaitForSeconds(time);
        isSkillMoveRestriction = false;
    }

    private void InitSkill()
    {
        for (int i = 0; i < TableManager.Instance.SkillTable.dataArray.Length; i++)
        {
            var SkillTableData = TableManager.Instance.SkillTable.dataArray[i];

            if (ServerData.skillServerTable.HasSkill(SkillTableData.Id))
            {
                Type elementType = Type.GetType(SkillTableData.Skillclassname);

                object classType = Activator.CreateInstance(elementType);

                var skillBase = classType as SkillBase;

                skillBase.Initialize(this.transform, SkillTableData, this);

                UserSkills.Add(SkillTableData.Id, skillBase);
            }
        }
    }

    public Collider2D[] GetEnemiesInCircle(Vector2 origin, float radius)
    {
        return Physics2D.OverlapCircleAll(origin, radius, LayerMasks.EnemyLayerMask);
    }

    public RaycastHit2D[] GetEnemiesInRaycast(Vector2 origin, Vector2 rayDirection, float length)
    {
        return Physics2D.RaycastAll(origin, rayDirection, length, LayerMasks.EnemyLayerMask);
    }

    public RaycastHit2D[] GetEnemiesInBoxcast(Vector2 origin, Vector2 rayDirection, float length, float size)
    {
        return Physics2D.BoxCastAll(origin, Vector2.one * size, 0f, rayDirection, length, LayerMasks.EnemyLayerMask);
    }

    private string wallString = "Wall";
    public Vector2 GetRayHitWallPoint(Vector2 origin, Vector2 rayDirection, float length)
    {
        int hitLayer = LayerMasks.PlatformLayerMask_Ray + LayerMasks.EnemyWallLayerMask_Ray;

        var rayHits = Physics2D.RaycastAll(origin, rayDirection, length, hitLayer);

        for (int i = 0; i < rayHits.Length; i++)
        {
            if (rayHits[i].collider.gameObject.tag.Equals(wallString))
            {
                return rayHits[i].point - rayDirection.normalized * 0.5f;
            }
        }
        return Vector2.zero;
    }

    public Vector2 GetRayHitPlatformPoint(Vector2 origin, Vector2 rayDirection, float length, bool ignoreEnemyWall = false)
    {
        int hitLayer = 0;

        if (ignoreEnemyWall == false)
        {
            hitLayer = LayerMasks.PlatformLayerMask_Ray + LayerMasks.EnemyWallLayerMask_Ray;
        }
        else
        {
            hitLayer = LayerMasks.PlatformLayerMask_Ray;
        }

        var rayHits = Physics2D.RaycastAll(origin, rayDirection, length, hitLayer);

        for (int i = 0; i < rayHits.Length; i++)
        {
            return rayHits[i].point;
        }

        return Vector2.zero;
    }


    public void PlayAttackAnim()
    {
        PlayerViewController.Instance.SetCurrentAnimation(PlayerViewController.AnimState.attack);
    }

    public Vector3 GetSkillCastingPosOffset(SkillTableData tableData)
    {
        return tableData.Activeoffset * Vector2.right * (playerMoveController.MoveDirection == MoveDirection.Right ? 1 : -1);
    }
    private Dictionary<int, AgentHpController> agentHpControllers = new Dictionary<int, AgentHpController>();
    private Dictionary<double, double> calculatedDamage = new Dictionary<double, double>();
    private Dictionary<double, bool> calculatedDamage_critical = new Dictionary<double, bool>();
    private Dictionary<double, bool> calculatedDamage_superCritical = new Dictionary<double, bool>();

    public IEnumerator ApplyDamage(Collider2D hitEnemie, SkillTableData skillInfo, double damage, bool playSound)
    {
        AgentHpController agentHpController;

        int instanceId = hitEnemie.GetInstanceID();

        if (agentHpControllers.ContainsKey(instanceId) == false)
        {
            agentHpControllers.Add(instanceId, hitEnemie.gameObject.GetComponent<AgentHpController>());

            agentHpController = agentHpControllers[instanceId];

        }
        else
        {
            agentHpController = agentHpControllers[instanceId];
        }

        int hitCount = skillInfo.Hitcount + PlayerStats.GetSkillHitAddValue();

        double originDam = damage;
        double defense = agentHpController.Defense;
        double key = originDam + defense;

        if (calculatedDamage.ContainsKey(key) == false)
        {
            agentHpController.ApplyDefense(ref damage);

            bool isCritical = false;
            bool isSuperCritical = false;

            agentHpController.ApplyPlusDamage(ref damage, ref isCritical, ref isSuperCritical);

            calculatedDamage.Add(key, damage);
            calculatedDamage_critical.Add(key, isCritical);
            calculatedDamage_superCritical.Add(key, isSuperCritical);
        }
        

        for (int hit = 0; hit < hitCount; hit++)
        {
            if (agentHpController.gameObject == null || agentHpController.gameObject.activeInHierarchy == false) yield break;

            agentHpController.SpawnDamText(calculatedDamage_critical[key], calculatedDamage_superCritical[key], damage);
            agentHpController.UpdateHp(-calculatedDamage[key]);

            //이펙트
            if (string.IsNullOrEmpty(skillInfo.Hiteffectname) == false &&
                Vector3.Distance(this.transform.position, hitEnemie.gameObject.transform.position) < GameBalance.effectActiveDistance)
            {
                Vector3 spawnPos = hitEnemie.gameObject.transform.position + Vector3.forward * -1f + Vector3.up * 0.3f;
                spawnPos += (Vector3)UnityEngine.Random.insideUnitCircle * 0.5f;
                spawnPos += (Vector3)Vector3.back;
                EffectManager.SpawnEffectAllTime(skillInfo.Hiteffectname, spawnPos, limitSpawnSize: true);
            }

            //사운드
            //시전할때 사운드 있어서 따로재생X
            if (hit != 0 && playSound)
            {
                SoundManager.Instance.PlaySound(skillInfo.Soundname);
            }

            float tick = 0f;

            while (tick < 0.05f)
            {
                tick += Time.deltaTime;
                yield return null;
            }
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }


}
