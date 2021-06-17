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
    private TextMeshProUGUI currentLevelTitle;

    private ReactiveProperty<int> currentLevel = new ReactiveProperty<int>(1);

    private const int minLevel = 1;
    private const int maxLevel = 5;

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
        currentLevelTitle.SetText($"소환레벨 {currentLevel}");
    }

    private string GetWeaponText(int level)
    {
        string description = string.Empty;

        var weaponTableData = TableManager.Instance.WeaponTable.dataArray;

        Dictionary<string, float> datas = new Dictionary<string, float>()
        {
            { "하급", 0f},
            { "일반", 0f} ,
            { "레어", 0f } ,
            { "유니크",0f }
        };


        if (level == 1)
        {
                datas["하급"] = weaponTableData.Where(element => element.Name.Contains("하급")).Select(_1_1 => _1_1.Gachalv1).Sum();
                datas["일반"] = weaponTableData.Where(element => element.Name.Contains("일반")).Select(_1_2 => _1_2.Gachalv1).Sum();
                datas["레어"] = weaponTableData.Where(element => element.Name.Contains("레어")).Select(_1_3 => _1_3.Gachalv1).Sum();
                datas["유니크"] = weaponTableData.Where(element => element.Name.Contains("유니크")).Select(_1_4 => _1_4.Gachalv1).Sum();
        }
        else if (level == 2)
        {
                datas["하급"] = weaponTableData.Where(element => element.Name.Contains("하급")).Select(_2_1 => _2_1.Gachalv2).Sum();
                datas["일반"] = weaponTableData.Where(element => element.Name.Contains("일반")).Select(_2_2 => _2_2.Gachalv2).Sum();
                datas["레어"] = weaponTableData.Where(element => element.Name.Contains("레어")).Select(_2_3 => _2_3.Gachalv2).Sum();
                datas["유니크"] = weaponTableData.Where(element => element.Name.Contains("유니크")).Select(_2_4 => _2_4.Gachalv2).Sum();
        }
        else if (level == 3)
        {
                datas["하급"] = weaponTableData.Where(element => element.Name.Contains("하급")).Select(_3_1 => _3_1.Gachalv3).Sum();
                datas["일반"] = weaponTableData.Where(element => element.Name.Contains("일반")).Select(_3_2 => _3_2.Gachalv3).Sum();
                datas["레어"] = weaponTableData.Where(element => element.Name.Contains("레어")).Select(_3_3 => _3_3.Gachalv3).Sum();
                datas["유니크"] = weaponTableData.Where(element => element.Name.Contains("유니크")).Select(_3_4 => _3_4.Gachalv3).Sum();
        }
        else if (level == 4)
        {
                datas["하급"] = weaponTableData.Where(element => element.Name.Contains("하급")).Select(_4_1 => _4_1.Gachalv4).Sum();
                datas["일반"] = weaponTableData.Where(element => element.Name.Contains("일반")).Select(_4_2 => _4_2.Gachalv4).Sum();
                datas["레어"] = weaponTableData.Where(element => element.Name.Contains("레어")).Select(_4_3 => _4_3.Gachalv4).Sum();
                datas["유니크"] = weaponTableData.Where(element => element.Name.Contains("유니크")).Select(_4_4 => _4_4.Gachalv4).Sum();
        }
        else if (level == 5)
        {
                datas["하급"] = weaponTableData.Where(element => element.Name.Contains("하급")).Select(_5_1 => _5_1.Gachalv5).Sum();
                datas["일반"] = weaponTableData.Where(element => element.Name.Contains("일반")).Select(_5_2 => _5_2.Gachalv5).Sum();
                datas["레어"] = weaponTableData.Where(element => element.Name.Contains("레어")).Select(_5_3 => _5_3.Gachalv5).Sum();
                datas["유니크"] = weaponTableData.Where(element => element.Name.Contains("유니크")).Select(_5_4 => _5_4.Gachalv5).Sum();
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
            { "하급", 0f},
            { "일반", 0f} ,
            { "레어", 0f } ,
            { "유니크",0f }
        };


        if (level == 1)
        {
            datas["하급"] = magicBookTableData.Where(element => element.Name.Contains("하급")).Select(_1_1 => _1_1.Gachalv1).Sum();
            datas["일반"] = magicBookTableData.Where(element => element.Name.Contains("일반")).Select(_1_2 => _1_2.Gachalv1).Sum();
            datas["레어"] = magicBookTableData.Where(element => element.Name.Contains("레어")).Select(_1_3 => _1_3.Gachalv1).Sum();
            datas["유니크"] = magicBookTableData.Where(element => element.Name.Contains("유니크")).Select(_1_4 => _1_4.Gachalv1).Sum();
        }
        else if (level == 2)
        {
            datas["하급"] = magicBookTableData.Where(element => element.Name.Contains("하급")).Select(_2_1 => _2_1.Gachalv2).Sum();
            datas["일반"] = magicBookTableData.Where(element => element.Name.Contains("일반")).Select(_2_2 => _2_2.Gachalv2).Sum();
            datas["레어"] = magicBookTableData.Where(element => element.Name.Contains("레어")).Select(_2_3 => _2_3.Gachalv2).Sum();
            datas["유니크"] = magicBookTableData.Where(element => element.Name.Contains("유니크")).Select(_2_4 => _2_4.Gachalv2).Sum();
        }
        else if (level == 3)
        {
            datas["하급"] = magicBookTableData.Where(element => element.Name.Contains("하급")).Select(_3_1 => _3_1.Gachalv3).Sum();
            datas["일반"] = magicBookTableData.Where(element => element.Name.Contains("일반")).Select(_3_2 => _3_2.Gachalv3).Sum();
            datas["레어"] = magicBookTableData.Where(element => element.Name.Contains("레어")).Select(_3_3 => _3_3.Gachalv3).Sum();
            datas["유니크"] = magicBookTableData.Where(element => element.Name.Contains("유니크")).Select(_3_4 => _3_4.Gachalv3).Sum();
        }
        else if (level == 4)
        {
            datas["하급"] = magicBookTableData.Where(element => element.Name.Contains("하급")).Select(_4_1 => _4_1.Gachalv4).Sum();
            datas["일반"] = magicBookTableData.Where(element => element.Name.Contains("일반")).Select(_4_2 => _4_2.Gachalv4).Sum();
            datas["레어"] = magicBookTableData.Where(element => element.Name.Contains("레어")).Select(_4_3 => _4_3.Gachalv4).Sum();
            datas["유니크"] = magicBookTableData.Where(element => element.Name.Contains("유니크")).Select(_4_4 => _4_4.Gachalv4).Sum();
        }
        else if (level == 5)
        {
            datas["하급"] = magicBookTableData.Where(element => element.Name.Contains("하급")).Select(_5_1 => _5_1.Gachalv5).Sum();
            datas["일반"] = magicBookTableData.Where(element => element.Name.Contains("일반")).Select(_5_2 => _5_2.Gachalv5).Sum();
            datas["레어"] = magicBookTableData.Where(element => element.Name.Contains("레어")).Select(_5_3 => _5_3.Gachalv5).Sum();
            datas["유니크"] = magicBookTableData.Where(element => element.Name.Contains("유니크")).Select(_5_4 => _5_4.Gachalv5).Sum();
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
