using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Spine.Unity;

public class MagicBookIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform magicBookObject;

    //[SerializeField]
    //private Transform followingPoint;

    //[SerializeField]
    //private float followSpeed = 0.1f;

    [SerializeField]
    private Image magicBookIcon;

    [SerializeField]
    private BoneFollowerGraphic boneFollowerGraphic;

    [SerializeField]
    private List<GameObject> sinMulEffect;

    public void Initialize(SkeletonGraphic skeletonGraphic)
    {
        boneFollowerGraphic.skeletonGraphic = skeletonGraphic;
    }

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].AsObservable().Subscribe(WhenMagicBookEquipInfoChanged).AddTo(this);
    }
    private void WhenMagicBookEquipInfoChanged(int idx)
    {
        //맨처음 미보유
        if (idx == -1)
        {
            magicBookObject.gameObject.SetActive(false);
            return;
        }
        else
        {
            magicBookObject.gameObject.SetActive(true);
        }

        magicBookIcon.sprite = CommonResourceContainer.GetMagicBookSprite(idx);

        if (idx < 16)
        {
            sinMulEffect.ForEach(e => e.SetActive(false));
        }
        else if (idx == 20)
        {
            int effectIdx = 4;

            if (idx == 20)
            {
                effectIdx = 4;
            }

            for (int i = 0; i < sinMulEffect.Count; i++)
            {
                sinMulEffect[i].SetActive(i == effectIdx);
            }
        }
        else
        {
            int effectIdx = idx % 4;
            for (int i = 0; i < sinMulEffect.Count; i++)
            {
                sinMulEffect[i].SetActive(i == effectIdx);
            }
        }

    }

    //void Update()
    //{
    //    magicBookObject.transform.position = Vector2.Lerp(magicBookObject.transform.position, this.followingPoint.position, Time.deltaTime * followSpeed);
    //}


}
