using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineTowerBoss : MonoBehaviour
{
    [SerializeField]
    private BoneFollower boneFollower;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        boneFollower.SetBone("bone3");
        spriteRenderer.sprite = CommonResourceContainer.GetBeltSprite(PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor);

        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.gameObject.SetActive(false);
        }
    }
}
