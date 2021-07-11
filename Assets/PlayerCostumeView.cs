using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerCostumeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(WhenCostumeChanged).AddTo(this);
    }

    private void WhenCostumeChanged(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        boneFollowerGraphic.SetBone("Weapon1");
    }
}
