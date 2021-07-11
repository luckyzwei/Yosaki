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
    private Image skillIcon;

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
    private SkillTableData skillData;

    private bool initialized = false;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private GameObject weaponMagicBookObject;

    [SerializeField]
    private GameObject skillObject;

    [SerializeField]
    private UIShiny uishiny;

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData, SkillTableData skillData = null)
    {
        weaponMagicBookObject.SetActive(skillData == null);

        skillObject.SetActive(skillData != null);

        this.weaponData = weaponData;
        this.magicBookData = magicBookData;
        this.skillData = skillData;

        int grade = 0;
        int id = 0;

        if (weaponData != null)
        {
            grade = weaponData.Grade;
            id = weaponData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetWeaponSprite(id);

        }
        else if (magicBookData != null)
        {
            grade = magicBookData.Grade;
            id = magicBookData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetMagicBookSprite(id);
        }
        else if (skillData != null)
        {
            grade = skillData.Skillgrade;
            id = skillData.Id;
            skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(id);
        }

        lvText.gameObject.SetActive(skillData == null);

        this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName[grade]);

        this.gradeText.color = (CommonUiContainer.Instance.itemGradeColor[grade]);

        int gradeText = 4 - (id % 4);

        gradeNumText.SetText($"{gradeText}등급");

        bg.sprite = CommonUiContainer.Instance.itemGradeFrame[grade];

        if (weaponData != null)
        {
            SubscribeWeapon();
        }
        else if (magicBookData != null)
        {
            SubscribeMagicBook();
        }
        else
        {
            SubscribeSkill();
        }

        uishiny.width = ((float)grade / 3f) * 0.8f;
        uishiny.brightness = ((float)grade / 3f) * 0.8f;
    }


    private void SubscribeWeapon()
    {
        disposable.Clear();

        ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        ServerData.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);

    }

    private void SubscribeMagicBook()
    {
        disposable.Clear();

        ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);

        ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenLevelChanged).AddTo(disposable);

    }

    private void SubscribeSkill()
    {
        disposable.Clear();

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].AsObservable().Subscribe(WhenCountChanged).AddTo(disposable);
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
            amountText.SetText($"({ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{weaponData.Requireupgrade})");
        }
        else if (magicBookData != null)
        {
            amountText.SetText($"({ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{magicBookData.Requireupgrade})");
        }
        else
        {
            amountText.SetText($"({ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value}/{skillData.Requireupgrade})");
        }
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
