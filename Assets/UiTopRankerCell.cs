using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTopRankerCell : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic costumeGraphic;
    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic;
    [SerializeField]
    private SkeletonGraphic petGraphic;
    [SerializeField]
    private TextMeshProUGUI nickName;
    [SerializeField]
    private TextMeshProUGUI rankText;

    [SerializeField]
    private Image weapon;
    [SerializeField]
    private Image magicBook;

    public void Initialize(string nickName, string rankText, int costumeId, int petId, int weaponId, int magicBookId, int fightPoint)
    {
        this.nickName.SetText(nickName);
        this.rankText.SetText(rankText);
        SetCostumeSpine(costumeId);
        SetPetSpine(petId);

        weapon.gameObject.SetActive(weaponId != -1);
        if (weaponId != -1)
        {
            weapon.sprite = CommonResourceContainer.GetWeaponSprite(weaponId);
        }

        magicBook.gameObject.SetActive(magicBookId != -1);
        if (magicBookId != -1)
        {
            magicBook.sprite = CommonResourceContainer.GetMagicBookSprite(magicBookId);
        }
    }

    private void SetPetSpine(int idx)
    {
        if (idx == -1)
        {
            petGraphic.gameObject.SetActive(false);
            return;
        }
        petGraphic.gameObject.SetActive(true);
        petGraphic.Clear();
        petGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];
        petGraphic.startingAnimation = "Walk";
        petGraphic.Initialize(true);
        petGraphic.SetMaterialDirty();
    }

    private void SetCostumeSpine(int idx)
    {
        costumeGraphic.Clear();
        costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        costumeGraphic.Initialize(true);
        costumeGraphic.SetMaterialDirty();

        boneFollowerGraphic.SetBone("Weapon1");
    }
}
