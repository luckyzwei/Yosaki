using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectManager
{
    public static PoolItem SpawnEffectAllTime(string effectName, Vector3 position, Transform parent = null, bool limitSpawnSize = false, int limitNum = 15)
    {
        if (SettingData.ShowEffect.Value == 0) return null;

        if (limitSpawnSize && BattleObjectManagerAllTime.Instance.HasPool(effectName))
        {
            if (BattleObjectManagerAllTime.Instance.GetSpawnedItemNum(effectName) > limitNum)
            {
                return null;
            }
        }

        PoolItem effect = null;

        if (parent == null)
        {
            effect = BattleObjectManagerAllTime.Instance.GetItem(effectName);
        }
        else
        {
            effect = BattleObjectManager.Instance.GetItem(effectName);
        }

        if (effect == null)
        {
            return null;
        }

        if (parent != null)
        {
            effect.transform.SetParent(parent);
        }

        effect.transform.position = position;

        return effect;
    }
}
