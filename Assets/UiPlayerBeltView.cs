using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPlayerBeltView : MonoBehaviour
{
    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;

        boneFollowerGraphic.SetBone("bone");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
