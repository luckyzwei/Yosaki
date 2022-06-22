using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CommonResourceContainer
{
    private static List<Sprite> weaponSprites;
    private static List<Sprite> magicBookSprites;
    private static List<Sprite> maskSprites;

    public static Sprite GetRandomWeaponSprite()
    {
        if (weaponSprites == null)
        {
            var weaponIcons = Resources.LoadAll<Sprite>("Weapon/");
            weaponSprites = weaponIcons.ToList();


            weaponSprites.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        return weaponSprites[Random.Range(0, 21)];
    }

    public static Sprite GetWeaponSprite(int idx)
    {
        if (weaponSprites == null)
        {
            var weaponIcons = Resources.LoadAll<Sprite>("Weapon/");
            weaponSprites = weaponIcons.ToList();


            weaponSprites.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < weaponSprites.Count)
        {
            return weaponSprites[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetMaskSprite(int idx)
    {
        if (maskSprites == null)
        {
            var maksIcons = Resources.LoadAll<Sprite>("FoxMask/");
            maskSprites = maksIcons.ToList();


            maskSprites.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < maskSprites.Count)
        {
            return maskSprites[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetMagicBookSprite(int idx)
    {
        if (magicBookSprites == null)
        {
            var magicBookIcons = Resources.LoadAll<Sprite>("MagicBook/");
            magicBookSprites = magicBookIcons.ToList();

            magicBookSprites.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < magicBookSprites.Count)
        {
            return magicBookSprites[idx];
        }
        else
        {
            Debug.LogError($"MagicBook icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetSkillSprite(int idx)
    {
        if (magicBookSprites == null)
        {
            var magicBookIcons = Resources.LoadAll<Sprite>("Skill/");
            magicBookSprites = magicBookIcons.ToList();
        }

        if (idx < magicBookSprites.Count)
        {
            return magicBookSprites[idx];
        }
        else
        {
            Debug.LogError($"Skill icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetSkillIconSprite(int idx)
    {
        return GetSkillIconSprite(TableManager.Instance.SkillData[idx]);
    }

    public static Sprite GetSkillIconSprite(SkillTableData skillData)
    {
        return Resources.Load<Sprite>($"SkillIcon/{skillData.Skillicon}");
    }

    public static Sprite GetPassiveSkillIconSprite(int idx)
    {
        return GetPassiveSkillIconSprite(TableManager.Instance.PassiveSkill.dataArray[idx]);
    }

    public static Sprite GetPassiveSkillIconSprite(PassiveSkillData skillData)
    {
        return Resources.Load<Sprite>($"PassiveSkillIcon/{skillData.Id}");
    }

    public static Sprite GetSusanoIcon()
    {
        int susanoIdx = PlayerStats.GetSusanoGrade();

        if (susanoIdx != -1)
        {
            return Resources.Load<Sprite>($"Susano/{susanoIdx / 3}");
        }

        return null;
    }

}
