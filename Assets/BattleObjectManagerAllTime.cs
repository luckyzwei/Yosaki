using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObjectManagerAllTime : SingletonMono<BattleObjectManagerAllTime>
{
    [SerializeField]
    private List<PoolSet> poolSets;

    private Dictionary<string, ObjectPool<PoolItem>> poolContainer = new Dictionary<string, ObjectPool<PoolItem>>();
    public Dictionary<string, ObjectPool<PoolItem>> PoolContainer => poolContainer;

    [SerializeField]
    private DamageText damageTextPrefab;

    public ObjectProperty<DamageText> damageTextProperty { get; set; }

    [SerializeField]
    private DropItem dropItemPrefab;
    public ObjectProperty<DropItem> dropItemProperty { get; set; }


    private new void Awake()
    {
        base.Awake();
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSets.Count; i++)
        {
            ObjectPool<PoolItem> pool = new ObjectPool<PoolItem>(poolSets[i].prefab, this.transform, poolSets[i].initNum);
            poolContainer.Add(poolSets[i].name, pool);
        }

        damageTextProperty = new ObjectProperty<DamageText>(damageTextPrefab, this.transform, 1);
        dropItemProperty = new ObjectProperty<DropItem>(dropItemPrefab, this.transform, 1);
    }

    private float zOffset = 0f;

    public void SpawnDamageText(double damage, Vector3 position, DamTextType type = DamTextType.Normal)
    {
        if (SettingData.ShowDamageFont.Value == 0) return;
        if (damageTextProperty.Pool.OutPool.Count > GameBalance.MaxDamTextNum) return;

        var damageText = damageTextProperty.GetItem();
        damageText.Initialize(damage, type);
        damageText.transform.position = new Vector3(position.x, position.y, position.z + zOffset);

        zOffset += 0.001f;
        if (zOffset > 1f)
        {
            zOffset = 0f;
        }

    }
    public PoolItem GetItem(string name)
    {
        if (poolContainer.TryGetValue(name, out var pool))
        {
            return pool.GetItem();
        }
        else
        {
            var prefab = Resources.Load<PoolItem>(name);

            if (prefab == null)
            {
#if UNITY_EDITOR
                // Debug.LogError($"Pool prefab {name} is not exist");
#endif
                return null;
            }

            ObjectPool<PoolItem> newPool = new ObjectPool<PoolItem>(prefab, this.transform, 0);
            poolContainer.Add(name, newPool);

            return newPool.GetItem();
        }
    }

    public bool HasPool(string name)
    {
        return poolContainer.ContainsKey(name);
    }

    public int GetSpawnedItemNum(string name)
    {
        return poolContainer[name].OutPool.Count;
    }
}
