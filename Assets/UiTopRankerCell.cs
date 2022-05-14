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
    private BoneFollowerGraphic boneFollowerGraphic_Mask;

    [SerializeField]
    private Image weapon;
    [SerializeField]
    private Image magicBook;

    [SerializeField]
    private Image mask;


    [SerializeField]
    private GameObject yomulObject;

    [SerializeField]
    private GameObject newWeaponEffect;

    [SerializeField]
    private List<GameObject> norigaeEffects;

    [SerializeField]
    private TextMeshProUGUI guildName;

    public void Initialize(string nickName, string rankText, int costumeId, int petId, int weaponId, int magicBookId, int fightPoint, string guildName, int maskIdx)
    {
        this.nickName.SetText(nickName);
        this.rankText.SetText(rankText);
        SetCostumeSpine(costumeId);
        SetPetSpine(petId);

        this.guildName.gameObject.SetActive(string.IsNullOrEmpty(guildName) == false);
        this.guildName.SetText($"({guildName})");

        weapon.gameObject.SetActive(weaponId != -1);

        yomulObject.SetActive(weaponId == 20);

        newWeaponEffect.SetActive(weaponId == 22);

        if (maskIdx != -1)
        {
            mask.gameObject.SetActive(true);
            mask.sprite = CommonResourceContainer.GetMaskSprite(maskIdx);
        }
        else
        {
            mask.gameObject.SetActive(false);
        }

        if (weaponId != -1)
        {
            weapon.sprite = CommonResourceContainer.GetWeaponSprite(weaponId);

            if (fightPoint < CommonUiContainer.Instance.weaponEnhnaceMats.Count)
            {
                weapon.material = CommonUiContainer.Instance.weaponEnhnaceMats[fightPoint];
            }
            else
            {
                weapon.material = CommonUiContainer.Instance.weaponEnhnaceMats[0];
            }
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


        if (idx <= 14)
        {
            petGraphic.startingAnimation = "walk";
            petGraphic.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            petGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector3(44.9f, 74.7f, 0.7f);
        }
        else
        {
            petGraphic.startingAnimation = "idel";
            petGraphic.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            petGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector3(44.9f, 138.2f, 0.7f);
        }

        petGraphic.gameObject.SetActive(true);
        petGraphic.Clear();
        petGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];
        petGraphic.Initialize(true);
        petGraphic.SetMaterialDirty();

    }

    private void SetCostumeSpine(int idx)
    {
        costumeGraphic.Clear();
        costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        costumeGraphic.Initialize(true);
        costumeGraphic.SetMaterialDirty();

        boneFollowerGraphic_Mask.SetBone("bone21");
        //boneFollowerGraphic.SetBone("Weapon1");
    }
}
