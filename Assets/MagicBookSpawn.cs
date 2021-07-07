using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MagicBookSpawn : MonoBehaviour
{
    [SerializeField]
    private MagicBookIndicator magicBookIndicator;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private Transform magicbookParent;

    private MagicBookIndicator prefIndicator;

    IEnumerator Start()
    {
        yield return null;
        yield return null;

        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            if (prefIndicator != null)
            {
                GameObject.Destroy(prefIndicator.gameObject);
            }

            prefIndicator = Instantiate<MagicBookIndicator>(magicBookIndicator, magicbookParent);
            prefIndicator.Initialize(skeletonGraphic);
            prefIndicator.transform.localPosition = Vector3.zero;

        }).AddTo(this);
    }
}
