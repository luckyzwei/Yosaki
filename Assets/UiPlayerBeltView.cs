using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerBeltView : MonoBehaviour
{
    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic;

    [SerializeField]
    private Image icon;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;

        boneFollowerGraphic.SetBone("bone");

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CaveBeltView].AsObservable().Subscribe(e =>
        {
            if (e != -1)
            {
                icon.gameObject.SetActive(true);
                icon.sprite = CommonResourceContainer.GetBeltSprite(e);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }

        }).AddTo(this);
    }


}
