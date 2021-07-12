using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;
using UniRx;

public class UiProbPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description_weapon;

    [SerializeField]
    private TextMeshProUGUI description_magicBook;

    [SerializeField]
    private TextMeshProUGUI description_skill;

    [SerializeField]
    private TextMeshProUGUI currentLevelTitle;

    private ReactiveProperty<int> currentLevel = new ReactiveProperty<int>(1);

    private const int minLevel = 1;
    private const int maxLevel = 10;

    public void OnClickLeft()
    {
        if (currentLevel.Value == minLevel) return;
        currentLevel.Value--;
    }
    public void OnClickRight()
    {
        if (currentLevel.Value == maxLevel) return;
        currentLevel.Value++;
    }

    private void Awake()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        currentLevel.AsObservable().Subscribe(WhenCurrentLevelChanged).AddTo(this);
    }

    private void WhenCurrentLevelChanged(int currentLevel)
    {
        description_weapon.SetText(GetWeaponText(currentLevel));
        description_magicBook.SetText(GetMagicBookText(currentLevel));
        description_skill.SetText(GetSkillText(currentLevel));
        currentLevelTitle.SetText($"소환레벨 {currentLevel}");
    }

    private string GetWeaponText(int level)
    {
        string description = string.Empty;

        var weaponTableData = TableManager.Instance.WeaponTable.dataArray;

        Dictionary<string, float> datas = new Dictionary<string, float>()
        {
            { CommonString.ItemGrade_0, 0f},
            {  CommonString.ItemGrade_1, 0f} ,
            {  CommonString.ItemGrade_2, 0f } ,
            {  CommonString.ItemGrade_3,0f }
        };


        if (level == 1)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_1_1 => _1_1.Gachalv1).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_1_2 => _1_2.Gachalv1).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_1_3 => _1_3.Gachalv1).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_1_4 => _1_4.Gachalv1).Sum();
        }
        else if (level == 2)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_2_1 => _2_1.Gachalv2).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_2_2 => _2_2.Gachalv2).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_2_3 => _2_3.Gachalv2).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_2_4 => _2_4.Gachalv2).Sum();
        }
        else if (level == 3)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_3_1 => _3_1.Gachalv3).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_3_2 => _3_2.Gachalv3).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_3_3 => _3_3.Gachalv3).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_3_4 => _3_4.Gachalv3).Sum();
        }
        else if (level == 4)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_4_1 => _4_1.Gachalv4).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_4_2 => _4_2.Gachalv4).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_4_3 => _4_3.Gachalv4).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_4_4 => _4_4.Gachalv4).Sum();
        }
        else if (level == 5)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_5_1 => _5_1.Gachalv5).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_5_2 => _5_2.Gachalv5).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_5_3 => _5_3.Gachalv5).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_5_4 => _5_4.Gachalv5).Sum();
        }
        else if (level == 6)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_6_1 => _6_1.Gachalv6).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_6_2 => _6_2.Gachalv6).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_6_3 => _6_3.Gachalv6).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_6_4 => _6_4.Gachalv6).Sum();
        }
        else if (level == 7)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_7_1 => _7_1.Gachalv7).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_7_2 => _7_2.Gachalv7).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_7_3 => _7_3.Gachalv7).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_7_4 => _7_4.Gachalv7).Sum();
        }
        else if (level == 8)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_8_1 => _8_1.Gachalv8).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_8_2 => _8_2.Gachalv8).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_8_3 => _8_3.Gachalv8).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_8_4 => _8_4.Gachalv8).Sum();
        }
        else if (level == 9)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_9_1 => _9_1.Gachalv9).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_9_2 => _9_2.Gachalv9).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_9_3 => _9_3.Gachalv9).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_9_4 => _9_4.Gachalv9).Sum();
        }
        else if (level == 10)
        {
            datas[CommonString.ItemGrade_0] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_10_1 => _10_1.Gachalv10).Sum();
            datas[CommonString.ItemGrade_1] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_10_2 => _10_2.Gachalv10).Sum();
            datas[CommonString.ItemGrade_2] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_10_3 => _10_3.Gachalv10).Sum();
            datas[CommonString.ItemGrade_3] = weaponTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_10_4 => _10_4.Gachalv10).Sum();
        }

        var e = datas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value == 0f) continue;
            description += $"{e.Current.Key} {(e.Current.Value * 100f).ToString("F2")}%\n";
        }

        return description;
    }

    private string GetMagicBookText(int level)
    {
        string description = string.Empty;

        var magicBookTableData = TableManager.Instance.MagicBookTable.dataArray;

        Dictionary<string, float> datas = new Dictionary<string, float>()
        {
            {  CommonString.ItemGrade_0, 0f},
            {  CommonString.ItemGrade_1, 0f} ,
            {  CommonString.ItemGrade_2, 0f } ,
            {  CommonString.ItemGrade_3,0f }
        };


        if (level == 1)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_1_1 => _1_1.Gachalv1).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_1_2 => _1_2.Gachalv1).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_1_3 => _1_3.Gachalv1).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_1_4 => _1_4.Gachalv1).Sum();
        }
        else if (level == 2)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_2_1 => _2_1.Gachalv2).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_2_2 => _2_2.Gachalv2).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_2_3 => _2_3.Gachalv2).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_2_4 => _2_4.Gachalv2).Sum();
        }
        else if (level == 3)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_3_1 => _3_1.Gachalv3).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_3_2 => _3_2.Gachalv3).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_3_3 => _3_3.Gachalv3).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_3_4 => _3_4.Gachalv3).Sum();
        }
        else if (level == 4)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_4_1 => _4_1.Gachalv4).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_4_2 => _4_2.Gachalv4).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_4_3 => _4_3.Gachalv4).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_4_4 => _4_4.Gachalv4).Sum();
        }
        else if (level == 5)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_5_1 => _5_1.Gachalv5).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_5_2 => _5_2.Gachalv5).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_5_3 => _5_3.Gachalv5).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_5_4 => _5_4.Gachalv5).Sum();
        }
        else if (level == 6)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_6_1 => _6_1.Gachalv6).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_6_2 => _6_2.Gachalv6).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_6_3 => _6_3.Gachalv6).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_6_4 => _6_4.Gachalv6).Sum();
        }
        else if (level == 7)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_7_1 => _7_1.Gachalv7).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_7_2 => _7_2.Gachalv7).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_7_3 => _7_3.Gachalv7).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_7_4 => _7_4.Gachalv7).Sum();
        }
        else if (level == 8)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_8_1 => _8_1.Gachalv8).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_8_2 => _8_2.Gachalv8).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_8_3 => _8_3.Gachalv8).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_8_4 => _8_4.Gachalv8).Sum();
        }
        else if (level == 9)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_9_1 => _9_1.Gachalv9).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_9_2 => _9_2.Gachalv9).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_9_3 => _9_3.Gachalv9).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_9_4 => _9_4.Gachalv9).Sum();
        }
        else if (level == 10)
        {
            datas[CommonString.ItemGrade_0] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_10_1 => _10_1.Gachalv10).Sum();
            datas[CommonString.ItemGrade_1] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_10_2 => _10_2.Gachalv10).Sum();
            datas[CommonString.ItemGrade_2] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_10_3 => _10_3.Gachalv10).Sum();
            datas[CommonString.ItemGrade_3] = magicBookTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_10_4 => _10_4.Gachalv10).Sum();
        }

        var e = datas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value == 0f) continue;
            description += $"{e.Current.Key} {(e.Current.Value * 100f).ToString("F2")}%\n";
        }

        return description;
    }

    private string GetSkillText(int level)
    {
        string description = string.Empty;

        var skillTableData = TableManager.Instance.SkillTable.dataArray;

        Dictionary<string, float> datas = new Dictionary<string, float>()
        {
            {  CommonString.ItemGrade_0, 0f},
            {  CommonString.ItemGrade_1, 0f} ,
            {  CommonString.ItemGrade_2, 0f } ,
            {  CommonString.ItemGrade_3,0f }
        };


        if (level == 1)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_1_1 => _1_1.Gachalv1).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_1_2 => _1_2.Gachalv1).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_1_3 => _1_3.Gachalv1).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_1_4 => _1_4.Gachalv1).Sum();
        }
        else if (level == 2)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_2_1 => _2_1.Gachalv2).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_2_2 => _2_2.Gachalv2).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_2_3 => _2_3.Gachalv2).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_2_4 => _2_4.Gachalv2).Sum();
        }
        else if (level == 3)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_3_1 => _3_1.Gachalv3).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_3_2 => _3_2.Gachalv3).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_3_3 => _3_3.Gachalv3).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_3_4 => _3_4.Gachalv3).Sum();
        }
        else if (level == 4)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_4_1 => _4_1.Gachalv4).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_4_2 => _4_2.Gachalv4).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_4_3 => _4_3.Gachalv4).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_4_4 => _4_4.Gachalv4).Sum();
        }
        else if (level == 5)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_5_1 => _5_1.Gachalv5).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_5_2 => _5_2.Gachalv5).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_5_3 => _5_3.Gachalv5).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_5_4 => _5_4.Gachalv5).Sum();
        }
        else if (level == 6)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_6_1 => _6_1.Gachalv6).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_6_2 => _6_2.Gachalv6).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_6_3 => _6_3.Gachalv6).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_6_4 => _6_4.Gachalv6).Sum();
        }
        else if (level == 7)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_7_1 => _7_1.Gachalv7).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_7_2 => _7_2.Gachalv7).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_7_3 => _7_3.Gachalv7).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_7_4 => _7_4.Gachalv7).Sum();
        }
        else if (level == 8)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_8_1 => _8_1.Gachalv8).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_8_2 => _8_2.Gachalv8).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_8_3 => _8_3.Gachalv8).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_8_4 => _8_4.Gachalv8).Sum();
        }
        else if (level == 9)
        {
            datas[CommonString.ItemGrade_0] =skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_9_1 => _9_1.Gachalv9).Sum();
            datas[CommonString.ItemGrade_1] =skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_9_2 => _9_2.Gachalv9).Sum();
            datas[CommonString.ItemGrade_2] =skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_9_3 => _9_3.Gachalv9).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_9_4 => _9_4.Gachalv9).Sum();
        }
        else if (level == 10)
        {
            datas[CommonString.ItemGrade_0] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_0)).Select(_10_1 => _10_1.Gachalv10).Sum();
            datas[CommonString.ItemGrade_1] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_1)).Select(_10_2 => _10_2.Gachalv10).Sum();
            datas[CommonString.ItemGrade_2] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_2)).Select(_10_3 => _10_3.Gachalv10).Sum();
            datas[CommonString.ItemGrade_3] = skillTableData.Where(element => element.Name.Contains(CommonString.ItemGrade_3)).Select(_10_4 => _10_4.Gachalv10).Sum();
        }

        var e = datas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value == 0f) continue;
            description += $"{e.Current.Key} {(e.Current.Value * 100f).ToString("F2")}%\n";
        }

        return description;
    }
}
