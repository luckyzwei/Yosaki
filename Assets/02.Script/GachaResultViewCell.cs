using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiGachaResultView;

public class GachaResultViewCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private WeaponView weaponView;

    //[SerializeField]
    //private GameObject rareEffect;
    [SerializeField]
    private GameObject uniqueEffect;

    private static string GetUniqueKey = "GetUnique";

    [SerializeField]
    private Image openMask;

    public void Initialzie(WeaponData weaponData, MagicBookData magicBookData, SkillTableData skillData,NewGachaTableData newGachaData, int amount)
    {
        weaponView.Initialize(weaponData, magicBookData, skillData, newGachaData);
        amountText.SetText($"{amount}개");

        if (weaponData != null)
        {
            // rareEffect.gameObject.SetActive(weaponData.Grade == 2);
            //uniqueEffect.gameObject.SetActive(weaponData.Grade == 3);
            if (weaponData.Grade == 3)
            {
                SoundManager.Instance.PlaySound(GetUniqueKey);
                PopupManager.Instance.ShowWhiteEffect();
            }

            openMask.color = CommonUiContainer.Instance.itemGradeColor[weaponData.Grade];
        }
        else if (magicBookData != null)
        {
            // rareEffect.gameObject.SetActive(magicBookData.Grade == 2);
            //uniqueEffect.gameObject.SetActive(magicBookData.Grade == 3);

            if (magicBookData.Grade == 3)
            {
                SoundManager.Instance.PlaySound(GetUniqueKey);
                PopupManager.Instance.ShowWhiteEffect();
            }

            openMask.color = CommonUiContainer.Instance.itemGradeColor[magicBookData.Grade];
        }
        else if (skillData != null)
        {
            // rareEffect.gameObject.SetActive(magicBookData.Grade == 2);
            //uniqueEffect.gameObject.SetActive(magicBookData.Grade == 3);

            if (skillData.Skillgrade == 3)
            {
                SoundManager.Instance.PlaySound(GetUniqueKey);
                PopupManager.Instance.ShowWhiteEffect();
            }

            openMask.color = CommonUiContainer.Instance.itemGradeColor[skillData.Skillgrade];
        }
        else if (newGachaData != null)
        {
            // rareEffect.gameObject.SetActive(magicBookData.Grade == 2);
            //uniqueEffect.gameObject.SetActive(magicBookData.Grade == 3);

      
            SoundManager.Instance.PlaySound(GetUniqueKey);
            PopupManager.Instance.ShowWhiteEffect();
       

            openMask.color = CommonUiContainer.Instance.itemGradeColor[newGachaData.Id];
        }
    }
}
