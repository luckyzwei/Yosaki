using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AutoManager : Singleton<AutoManager>
{
    private Transform playerTr;

    private float moveDistMax = 2f;
    private float jumpHeight = 2f;
    private float jumpWidth = 0.1f;
    private float newFindDistance = 3f;
    private float horizontalJumpDistance = 5f;
    public ReactiveProperty<bool> AutoMode { get; private set; } = new ReactiveProperty<bool>();

    private List<int> skillQueue = new List<int>();

    private WaitForSeconds skillDelay = new WaitForSeconds(0.1f);

    public void SetPlayerTr() 
    {
        playerTr = PlayerSkillCaster.Instance.PlayerMoveController.transform;
    }

    public void Subscribe()
    {

        GameManager.Instance.whenSceneChanged.Subscribe(e =>
        {
            if (IsAutoMode && UiAutoRevive.autoRevive == false)
            {
                SetAuto(false);
            }
        });
    }

    public void ResetSkillQueue()
    {
        skillQueue.Clear();
    }

    public void SetSkillQueue()
    {
        if (skillQueue.Count != 0) return;

        var selectedSkill = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillSlotIdx];

        for (int i = 0; i < selectedSkill.Count; i++)
        {
            int skillIdx = selectedSkill[i].Value;

            if (skillIdx != -1 &&
                SkillCoolTimeManager.HasSkillCooltime(skillIdx) == false &&
                SkillCoolTimeManager.registeredSkillIdx[i].Value == 1
                )
            {
                skillQueue.Add(skillIdx);
            }
        }
    }

    public void SetAuto(bool auto)
    {
        AutoMode.Value = auto;

        if (auto)
        {
            StopAutoRoutine();

            ResetAuto();

            autoPlayRoutine = CoroutineExecuter.Instance.StartCoroutine(AutoPlayRoutine());
        }
        else
        {
            StopAutoRoutine();
        }
    }

    private void ResetAuto()
    {
        currentTarget = null;
    }

    private void StopAutoRoutine()
    {
        UiMoveStick.Instance.SetHorizontalAxsis(0);
        UiMoveStick.Instance.SetVerticalAxsis(0);

        if (autoPlayRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoPlayRoutine);
            autoPlayRoutine = null;
            skillQueue.Clear();
        }
    }

    public bool IsAutoMode => AutoMode.Value;


    private Coroutine autoPlayRoutine;

    private Transform currentTarget;

    WaitForEndOfFrame updateTick = new WaitForEndOfFrame();
    private IEnumerator AutoPlayRoutine()
    {
        while (true)
        {
            yield return updateTick;

            FindTarget();

            //타겟 없을때 대기
            if (currentTarget == null)
            {
                UiMoveStick.Instance.SetHorizontalAxsis(0);
            }
            else
            {
                //타겟이랑 거리가 멀때 이동
                if (Vector3.Distance(playerTr.transform.position, currentTarget.transform.position) > moveDistMax && (SkillCoolTimeManager.moveAutoValue.Value == 1))
                {
                    //예전모드
                    if (SkillCoolTimeManager.moveAutoValue.Value == 1 && SkillCoolTimeManager.jumpAutoValue.Value == 1)
                    {
                        int Horizontal = currentTarget.transform.position.x > playerTr.transform.position.x ? 1 : -1;
                        UiMoveStick.Instance.SetHorizontalAxsis(Horizontal);

                        float xDist = Mathf.Abs(playerTr.transform.position.x - currentTarget.transform.position.x);
                        float yDist = Mathf.Abs(playerTr.transform.position.y - currentTarget.transform.position.y);

                        //위아래 체크
                        bool needToUpDown = xDist < jumpWidth && yDist > jumpHeight;

                        //박치기 꺼둠
                        //bool needToHorizontalJump = xDist > horizontalJumpDistance;
                        bool needToHorizontalJump = false;

                        if (needToHorizontalJump)
                        {
                            UiMoveStick.Instance.SetVerticalAxsis(0);
                            PlayerMoveController.Instance.JumpPlayer();
                            yield return new WaitForSeconds(0.3f);
                            PlayerMoveController.Instance.JumpPlayer();
                            yield return new WaitForSeconds(0.3f);
                        }
                        else if (needToUpDown)
                        {
                            //이단점프

                            bool doubleJump = playerTr.transform.position.y < currentTarget.transform.position.y;

                            if (doubleJump)
                            {
                                UiMoveStick.Instance.SetHorizontalAxsis(0);
                                UiMoveStick.Instance.SetVerticalAxsis(1);
                                PlayerMoveController.Instance.JumpPlayer();

                                //최대높이로 점프
                                while (PlayerMoveController.Instance.Rb.velocity.y > 2f)
                                {
                                    // UiMoveStick.Instance.SetHorizontalAxsis(UiMoveStick.Instance.Horizontal);
                                    yield return null;
                                }

                                PlayerMoveController.Instance.JumpPlayer();

                                //이단점프후딜레이
                                yield return new WaitForSeconds(0.6f);
                            }
                            //내려가기
                            else
                            {
                                UiMoveStick.Instance.SetVerticalAxsis(-1);
                                PlayerMoveController.Instance.JumpPlayer();
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                    }
                    //이동 on 점프 off =>이동만
                    else if (SkillCoolTimeManager.moveAutoValue.Value == 1 && SkillCoolTimeManager.jumpAutoValue.Value == 0)
                    {
                        int Horizontal = currentTarget.transform.position.x > playerTr.transform.position.x ? 1 : -1;
                        UiMoveStick.Instance.SetHorizontalAxsis(Horizontal);
                    }


                }
                else
                {
                    if (SkillCoolTimeManager.moveAutoValue.Value == 0 && SkillCoolTimeManager.jumpAutoValue.Value == 1)
                    {
                        //점프만
                        if (SkillCoolTimeManager.jumpAutoValue.Value == 1)
                        {
                            UiMoveStick.Instance.SetHorizontalAxsis(0);
                            UiMoveStick.Instance.SetVerticalAxsis(1);

                            PlayerMoveController.Instance.JumpPlayer();
                            PlayerMoveController.Instance.JumpPlayer();
                        }
                    }


                    if (skillQueue.Count == 0)
                    {
                        SetSkillQueue();
                    }

                    if (skillQueue.Count > 0)
                    {
                        //스킬 발동 방향 
                        bool isEnemyOnRight = currentTarget.transform.position.x > this.playerTr.position.x;
                        UiMoveStick.Instance.SetHorizontalAxsis(0);
                        PlayerMoveController.Instance.SetMoveDirection(isEnemyOnRight ? MoveDirection.Right : MoveDirection.Left);

                        //스킬 큐 세팅

                        //스킬 사용

                        int useSkillIdx = skillQueue[0];

                        bool skillCast = PlayerSkillCaster.Instance.UseSkill(useSkillIdx);

                        skillQueue.RemoveAt(0);

                        if (skillCast)
                        {
                            yield return skillDelay;
                            //후딜대기
                            //Debug.Log($"Wait {PlayerSkillCaster.Instance.UserSkills[useSkillIdx].skillInfo.Movedelay}");
                            //yield return new WaitForSeconds(PlayerSkillCaster.Instance.UserSkills[useSkillIdx].skillInfo.Movedelay);
                        }
                    }

                }
            }
        }
    }

    private void SortEnemy(List<Enemy> spawnedEnemyList)
    {
        if (spawnedEnemyList.Count >= 2)
        {
            spawnedEnemyList.Sort((a, b) =>
            {
                if (Vector3.Distance(a.transform.position, playerTr.transform.position) <
                Vector3.Distance(b.transform.position, playerTr.transform.position))
                {
                    return -1;
                }
                return 1;
            });
        }
    }

    private void FindTarget()
    {
        //이미 타겟을 찾은상태
        if (currentTarget != null && currentTarget.gameObject.activeInHierarchy == true)
        {
            //너무 멀어지면 다시찾도록
            if (Vector3.Distance(currentTarget.transform.position, playerTr.transform.position) <= newFindDistance)
            {
                return;
            }
        }

        if (GameManager.Instance.contentsType == GameManager.ContentsType.NormalField)
        {
            var spawnedEnemy = MapInfo.Instance.SpawnedEnemyList;

            SortEnemy(spawnedEnemy);

            if (spawnedEnemy.Count == 0)
            {
                currentTarget = null;
            }
            else
            {
                currentTarget = spawnedEnemy[0].transform;
            }
        }
        else if (GameManager.Instance.contentsType == GameManager.ContentsType.FireFly)
        {
            currentTarget = GetBonusDefenseTarget();
        }
    }

    public Transform GetNeariestEnemy()
    {
        Transform neariestEnemy = null;

        var spawnedEnemy = MapInfo.Instance.SpawnedEnemyList;

        SortEnemy(spawnedEnemy);

        if (spawnedEnemy.Count == 0)
        {
            neariestEnemy = null;
        }
        else
        {
            neariestEnemy = spawnedEnemy[0].transform;
        }

        return neariestEnemy;
    }

    private Transform GetBonusDefenseTarget()
    {
        var enemies = BattleObjectManager.Instance.PoolContainer[BonusDefenseManager.poolName].OutPool;

        if (enemies.Count == 0) return null;

        Transform neariestTarget = null;
        float minPos = float.MaxValue;
        var e = enemies.GetEnumerator();
        while (e.MoveNext())
        {
            float dist = Vector3.Distance(e.Current.Value.transform.position, playerTr.transform.position);
            if (dist < minPos)
            {
                minPos = dist;
                neariestTarget = e.Current.Value.transform;
            }
        }

        return neariestTarget;
    }
}
