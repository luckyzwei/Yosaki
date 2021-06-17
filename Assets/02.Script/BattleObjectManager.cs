using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolSet
{
    public string name;
    public PoolItem prefab;
    public int initNum;
}

public class BattleObjectManager : SingletonMono<BattleObjectManager>
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

    private void Start()
    {
        SpawnMap();
    }

    private void SpawnMap()
    {
        if (GameManager.Instance.IsNormalField)
        {
            GameObject mapObject = Resources.Load<GameObject>($"StageMap/{GameManager.Instance.CurrentStageData.Mappreset}");
            Instantiate<GameObject>(mapObject);
        }
        else
        {
            GameObject mapObject = null;

            if (GameManager.Instance.contentsType != GameManager.ContentsType.SingleRaid)
            {
                mapObject = Resources.Load<GameObject>($"ContentsMap/{GameManager.Instance.contentsType.ToString()}");
            }
            else
            {
                int currentBossIdx = GameManager.Instance.bossId;
                mapObject = Resources.Load<GameObject>($"ContentsMap/{GameManager.Instance.contentsType.ToString() + currentBossIdx.ToString()}");
            }

            Instantiate<GameObject>(mapObject);
        }
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

    public void SpawnDamageText(float damage, bool isCiritcal, Vector3 position, DamTextType type = DamTextType.Normal)
    {
        if (SettingData.ShowDamageFont.Value == 0) return;

        var damageText = damageTextProperty.GetItem();
        damageText.Initialize(damage, isCiritcal, type);
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
                Debug.LogError($"Pool prefab {name} is not exist");
#endif
                return null;
            }

            ObjectPool<PoolItem> newPool = new ObjectPool<PoolItem>(prefab, this.transform, 0);
            poolContainer.Add(name, newPool);

            return newPool.GetItem();
        }
    }
}
