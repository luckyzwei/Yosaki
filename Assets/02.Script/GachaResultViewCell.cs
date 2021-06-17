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
    public void Initialzie(WeaponData weaponData, MagicBookData magicBookData, int amount)
    {
        weaponView.Initialize(weaponData, magicBookData);
        amountText.SetText($"{amount}개");

        if (weaponData != null)
        {
            // rareEffect.gameObject.SetActive(weaponData.Grade == 2);
            uniqueEffect.gameObject.SetActive(weaponData.Grade == 3);
            if (weaponData.Grade == 3)
            {
                SoundManager.Instance.PlaySound(GetUniqueKey);
                PopupManager.Instance.ShowWhiteEffect();
            }
        }
        else if (magicBookData != null)
        {
            // rareEffect.gameObject.SetActive(magicBookData.Grade == 2);
            uniqueEffect.gameObject.SetActive(magicBookData.Grade == 3);

            if (magicBookData.Grade == 3)
            {
                SoundManager.Instance.PlaySound(GetUniqueKey);
                PopupManager.Instance.ShowWhiteEffect();
            }
        }
    }
}
