using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectManager
{
    public static PoolItem SpawnEffect(string effectName, Vector3 position, Transform parent = null)
    {
        if (SettingData.ShowEffect.Value == 0) return null;

        var effect = BattleObjectManager.Instance.GetItem(effectName);

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
