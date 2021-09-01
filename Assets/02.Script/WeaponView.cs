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
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_Weapon[grade]);
        }
        else if (magicBookData != null)
        {
            grade = magicBookData.Grade;
            id = magicBookData.Id;
            weaponIcon.sprite = CommonResourceContainer.GetMagicBookSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_Norigae[grade]);
        }
        else if (skillData != null)
        {
            grade = skillData.Skillgrade;
            id = skillData.Id;
            skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(id);
            this.gradeText.SetText(CommonUiContainer.Instance.ItemGradeName_Weapon[grade]);
        }

        lvText.gameObject.SetActive(skillData == null);

        this.gradeText.color = (CommonUiContainer.Instance.itemGradeColor[grade]);

        int gradeText = 4 - (id % 4);

        if (magicBookData == null)
        {
            gradeNumText.SetText($"{gradeText}등급");
        }
        else
        {
            if (magicBookData.Id / 4 != 4)
            {
                gradeNumText.SetText($"{gradeText}등급");

            }
            else
            {
                int remain = magicBookData.Id % 4;

                if (remain == 0)
                {
                    gradeNumText.SetText($"<color=green>현무</color>");
                }
                else if (remain == 1)
                {
                    gradeNumText.SetText($"<color=white>백호</color>");
                }
                else if (remain == 2)
                {
                    gradeNumText.SetText($"<color=red>주작</color>");
                }
                else if (remain == 3)
                {
                    gradeNumText.SetText($"<color=blue>청룡</color>");
                }
            }
        }

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

        //uishiny.width = ((float)grade / 3f) * 0.6f;
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
            int require = weaponData.Id < 20 ? weaponData.Requireupgrade : 1;
            amountText.SetText($"({ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{require})");
        }
        else if (magicBookData != null)
        {
            int require = magicBookData.Id < 16 ? magicBookData.Requireupgrade : 1;
            amountText.SetText($"({ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{require})");
        }
        else
        {
            int skillAwakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillData.Id].Value;
            int requireNum = skillAwakeNum == 0 ? 1 : 10;
            amountText.SetText($"({ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillData.Id].Value}/{requireNum})");
        }
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
