using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Coffee.UIEffects;

public class WeaponView : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    [SerializeField]
    private Image weaponIcon;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private TextMeshProUGUI lvText;

    [SerializeField]
    private TextMeshProUGUI gradeNumText;

    private WeaponData weaponData;
    private MagicBookData magicBookData;

    private bool initialized = false;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private UIShiny uishiny;

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData)
    {
        this.weaponData = weaponData;
        this.magicBookData = magicBookData;

        int grade = weaponData != null ? weaponData.Grade : magicBookData.Grade;
        int id = weaponData != null ? weaponData.Id : magicBookData.Id;

        this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName[grade]);

        this.gradeText.color = (CommonUiContainer.Instance.itemGradeColor[grade]);

        int gradeText = 4 - (id % 4);

        gradeNumText.SetText($"{gradeText}등급");

        bg.sprite = CommonUiContainer.Instance.itemGradeFrame[grade];

        if (weaponData != null)
        {
            weaponIcon.sprite = CommonResourceContainer.GetWeaponSprite(id);
        }
        else
        {
            weaponIcon.sprite = CommonResourceContainer.GetMagicBookSprite(id);
        }

        if (weaponData != null)
        {
            SubscribeWeapon();
        }
        else
        {
            SubscribeMagicBook();
        }

        uishiny.width = ((float)grade / 3f) * 0.8f;
        uishiny.brightness = ((float)grade / 3f) * 0.8f;
    }


    private void SubscribeWeapon()
    {
        disposable.Clear();

        DatabaseManager.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        DatabaseManager.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);

    }

    private void SubscribeMagicBook()
    {
        disposable.Clear();

        DatabaseManager.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        DatabaseManager.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);

    }


    private void WhenLevelChanged(int level)
    {
        lvText.SetText($"Lv.{level}");
    }

    private void WhenCountChanged(int amount)
    {
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        if (weaponData != null)
        {
            amountText.SetText($"({DatabaseManager.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{weaponData.Requireupgrade})");
        }
        else
        {
            amountText.SetText($"({DatabaseManager.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{magicBookData.Requireupgrade})");
        }
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
