using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiPetHomeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI petName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private PetTableData petData;

    private PetServerData petServerData;

    public void Initialize(PetTableData petData)
    {
        SetPetSpine(petData.Id);

        this.petData = petData;

        this.petServerData = ServerData.petTable.TableDatas[petData.Stringid];

        petName.SetText($"{petData.Name}");

        Subscribe();
    }

    private void Subscribe()
    {
        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);

    }

    private void SetPetSpine(int idx)
    {

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];

        if (idx != 15)
        {
            skeletonGraphic.startingAnimation = "walk";
        }
        else
        {
            skeletonGraphic.startingAnimation = "idel";
        }

        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        if (idx >= 15) 
        {
            skeletonGraphic.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }
    }
}
