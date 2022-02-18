using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using Spine.Unity;

public class UiGuildPetView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    public List<SkeletonDataAsset> petCostumeList;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        GuildManager.Instance.guildPetExp.AsObservable().Subscribe(e =>
        {

            SetPetSpine(GetPetIdx());

        }).AddTo(this);
    }

    private int GetPetIdx()
    {
        int idx = (int)(GuildManager.Instance.guildPetExp.Value / 3000);

        return Mathf.Clamp(idx, 0, petCostumeList.Count - 1);
    }

    private void SetPetSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = petCostumeList[idx];
        skeletonGraphic.startingAnimation = "idle";
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GuildManager.Instance.guildPetExp.Value += 2500;
        }
    }
#endif

}
