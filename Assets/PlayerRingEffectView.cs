using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerRingEffectView : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> effects;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.SoulRing].AsObservable().Subscribe(e =>
        {
            if (e < 12)
            {
                effects.ForEach(e => e.gameObject.SetActive(false));
            }
            else
            {
                int divide = (e / 4) - 3;

                for (int i = 0; i < effects.Count; i++)
                {
                    if (i == divide)
                    {
                        effects[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        effects[i].gameObject.SetActive(false);
                    }
                }
            }

        }).AddTo(this);
    }
}
