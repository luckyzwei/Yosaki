using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CommonResourceContainer
{
    private static List<Sprite> weaponSprites;
    private static List<Sprite> magicBookSprites;
    private static List<Sprite> maskSprites;
    private static List<Sprite> beltSprites;
    private static List<Sprite> hornSprites;
    private static List<Sprite> hellIcons;
    private static List<Sprite> dragonBall;
    private static List<Sprite> foxCup;
    private static List<Sprite> chunIcons;

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
    public static Sprite GetBeltSprite(int idx)
    {
        idx = idx / 3;

        if (beltSprites == null)
        {
            var beltIcons = Resources.LoadAll<Sprite>("CaveBelt/");
            beltSprites = beltIcons.ToList();


            beltSprites.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < beltSprites.Count)
        {
            return beltSprites[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }
    public static Sprite GetHornSprite(int idx)
    {
        if (hornSprites == null)
        {
            var hornIcons = Resources.LoadAll<Sprite>("DokebiHorn/");
            hornSprites = hornIcons.ToList();


            hornSprites.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < hornSprites.Count)
        {
            return hornSprites[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetDragonBallSprite(int idx)
    {
        if (dragonBall == null)
        {
            var maksIcons = Resources.LoadAll<Sprite>("DragonBall/");
            dragonBall = maksIcons.ToList();


            dragonBall.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < dragonBall.Count)
        {
            return dragonBall[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetFoxCupSprite(int idx)
    {
        if (foxCup == null)
        {
            var maksIcons = Resources.LoadAll<Sprite>("FoxCup/");
            foxCup = maksIcons.ToList();


            foxCup.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < foxCup.Count)
        {
            return foxCup[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetHellIconSprite(int idx)
    {
        if (hellIcons == null)
        {
            var maksIcons = Resources.LoadAll<Sprite>("HellIcons/");
            hellIcons = maksIcons.ToList();


            hellIcons.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < hellIcons.Count)
        {
            return hellIcons[idx];
        }
        else
        {
            Debug.LogError($"Weapon icon {idx} is not exist");
            return null;
        }
    }

    public static Sprite GetChunIconSprite(int idx)
    {
        if (chunIcons == null)
        {
            var maksIcons = Resources.LoadAll<Sprite>("ChunIcons/");
            chunIcons = maksIcons.ToList();


            chunIcons.Sort((a, b) =>
            {
                if (int.Parse(a.name) < int.Parse(b.name)) return -1;

                return 1;

            });
        }

        if (idx < chunIcons.Count)
        {
            return chunIcons[idx];
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
    public static Sprite GetNewGachaIconSprite(int idx)
    {
        return GetNewGachaIconSprite(TableManager.Instance.NewGachaData[idx]);
    }

    public static Sprite GetNewGachaIconSprite(NewGachaTableData newGachaData)
    {
        return Resources.Load<Sprite>($"NewGachaIcon/{newGachaData.Skillicon}");
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
