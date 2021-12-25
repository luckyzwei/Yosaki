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
    private SkeletonGraphic petGraphic;
    [SerializeField]
    private TextMeshProUGUI nickName;
    [SerializeField]
    private TextMeshProUGUI rankText;

    [SerializeField]
    private Image weapon;
    [SerializeField]
    private Image magicBook;

    [SerializeField]
    private GameObject yomulObject;

    [SerializeField]
    private List<GameObject> norigaeEffects;

    [SerializeField]
    private TextMeshProUGUI guildName;

    public void Initialize(string nickName, string rankText, int costumeId, int petId, int weaponId, int magicBookId, int fightPoint, string guildName)
    {
        this.nickName.SetText(nickName);
        this.rankText.SetText(rankText);
        SetCostumeSpine(costumeId);
        SetPetSpine(petId);

        this.guildName.gameObject.SetActive(string.IsNullOrEmpty(guildName) == false);
        this.guildName.SetText($"({guildName})");

        weapon.gameObject.SetActive(weaponId != -1);

        yomulObject.SetActive(weaponId == 20);

        if (weaponId != -1)
        {
            weapon.sprite = CommonResourceContainer.GetWeaponSprite(weaponId);
        }

        magicBook.gameObject.SetActive(magicBookId != -1);

        if (magicBookId != -1)
        {
            magicBook.sprite = CommonResourceContainer.GetMagicBookSprite(magicBookId);

            if (magicBookId < 16)
            {
                norigaeEffects.ForEach(e => e.SetActive(false));
            }
            else
            {
                int effectIdx = magicBookId % 4;

                if (magicBookId == 20)
                {
                    effectIdx = 4;
                }

                for (int i = 0; i < norigaeEffects.Count; i++)
                {
                    norigaeEffects[i].SetActive(i == effectIdx);
                }
            }
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
        petGraphic.startingAnimation = "walk";
        petGraphic.Initialize(true);
        petGraphic.SetMaterialDirty();
    }

    private void SetCostumeSpine(int idx)
    {
        costumeGraphic.Clear();
        costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        costumeGraphic.Initialize(true);
        costumeGraphic.SetMaterialDirty();

        //boneFollowerGraphic.SetBone("Weapon1");
    }
}
