
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;

public class WeaponChangeTest : MonoBehaviour
{


    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    Bone bone;

    [SerializeField]
    private GameObject weapon;

    [SerializeField]
    private string boneName;

    private void Start()
    {
        var skeleton = skeletonAnimation.skeleton;
        bone = skeleton.FindBone(boneName);

    }

    private void Update()
    {

        weapon.transform.position = transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0));
    }
}
