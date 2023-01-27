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
    private TextMeshProUGUI description_ring;

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
        description_ring.SetText(GetRingText(currentLevel));
        currentLevelTitle.SetText($"소환레벨 {currentLevel}");
    }

    private string GetWeaponText(int level)
    {
        string description = "무기\n";

        var tableDatas = TableManager.Instance.WeaponTable.dataArray;

        if (level == 1)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv1).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv1 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv1 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 2)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv2).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv2 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv2 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 3)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv3).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv3 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv3 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 4)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv4).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv4 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv4 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 5)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv5).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv5 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv5 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 6)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv6).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv6 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv6 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 7)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv7).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv7 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv7 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 8)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv8).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv8 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv8 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 9)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv9).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv9 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv9 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 10)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv10).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv10 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv10 / sum * 100f).ToString("F7")}%\n";
            }
        }

        return description;
    }

    private string GetMagicBookText(int level)
    {
        string description = "노리개\n";

        var tableDatas = TableManager.Instance.MagicBookTable.dataArray;

        if (level == 1)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv1).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv1 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv1 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 2)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv2).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv2 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv2 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 3)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv3).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv3 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv3 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 4)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv4).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv4 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv4 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 5)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv5).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv5 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv5 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 6)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv6).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv6 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv6 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 7)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv7).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv7 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv7 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 8)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv8).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv8 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv8 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 9)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv9).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv9 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv9 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 10)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv10).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv10 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Name} {(tableDatas[i].Gachalv10 / sum * 100f).ToString("F7")}%\n";
            }
        }

        return description;
    }
    private string GetRingText(int level)
    {
        string description = "반지\n";

        var tableDatas = TableManager.Instance.NewGachaTable.dataArray;

        if (level == 1)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv1).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv1 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv1 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 2)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv2).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv2 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv2 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 3)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv3).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv3 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv3 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 4)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv4).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv4 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv4 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 5)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv5).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv5 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv5 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 6)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv6).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv6 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv6 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 7)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv7).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv7 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv7 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 8)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv8).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv8 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv8 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 9)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv9).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv9 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv9 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 10)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv10).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv10 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv10 / sum * 100f).ToString("F7")}%\n";
            }
        }

        return description;
    }

    private string GetSkillText(int level)
    {
        string description = "기술\n";

        var tableDatas = TableManager.Instance.SkillTable.dataArray;

        if (level == 1)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv1).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv1 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv1 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 2)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv2).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv2 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv2 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 3)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv3).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv3 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv3 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 4)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv4).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv4 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv4 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 5)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv5).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv5 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv5 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 6)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv6).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv6 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv6 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 7)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv7).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv7 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv7 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 8)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv8).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv8 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv8 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 9)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv9).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv9 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv9 / sum * 100f).ToString("F7")}%\n";
            }
        }
        else if (level == 10)
        {
            float sum = tableDatas.Select(_1_1 => _1_1.Gachalv10).Sum();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                float prob = tableDatas[i].Gachalv10 / sum;
                if (prob == 0) continue;

                description += $"{tableDatas[i].Skillname} {(tableDatas[i].Gachalv10 / sum * 100f).ToString("F7")}%\n";
            }
        }

        return description;
    }
}
