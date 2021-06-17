using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBase : MonoBehaviour
{
    [SerializeField]
    protected AgentHpController agentHpController;

    [SerializeField]
    protected SkeletonAnimation skeletonAnimation;

    [SerializeField]
    protected EnemyHitObject hitObject;

}
