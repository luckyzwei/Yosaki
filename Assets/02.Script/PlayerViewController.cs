using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerViewController : SingletonMono<PlayerViewController>
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    private AnimState animState;

    private string CurrentAnimation;

    private const string Anim_Idle = "Idle";
    private const string Anim_Run = "walk";
    private const string Anim_Attack = "attack";

    private Coroutine attackAnimEndRoutine;

    private WaitForSeconds attackAnimDelay = new WaitForSeconds(0.3f);
    private IEnumerator AttackAnimEndRoutine()
    {
        yield return attackAnimDelay;
        CurrentAnimation = string.Empty;
        attackAnimEndRoutine = null;
    }

    public enum AnimState
    {
        idle, run, attack
    }

    private void SetAnimation(string animName)
    {
        if (attackAnimEndRoutine != null || (CurrentAnimation == animName && animName != Anim_Attack)) return;

        if (animName == Anim_Attack)
        {
            if (attackAnimEndRoutine != null)
            {
                StopCoroutine(attackAnimEndRoutine);
            }

            attackAnimEndRoutine = StartCoroutine(AttackAnimEndRoutine());
        }

        bool loop = animName.Equals(Anim_Idle) || animName.Equals(Anim_Run);


        skeletonGraphic.AnimationState.SetAnimation(0, animName, loop);

        CurrentAnimation = animName;
    }

    public void SetCurrentAnimation(AnimState state)
    {
        //attack idle run
        switch (state)
        {
            case AnimState.attack:
                {
                    SetAnimation(Anim_Attack);
                }
                break;
            case AnimState.idle:
                {
                    SetAnimation(Anim_Idle);
                }
                break;
            case AnimState.run:
                {
                    SetAnimation(Anim_Run);
                }
                break;
        }
    }
}
