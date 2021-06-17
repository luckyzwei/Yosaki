using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MagicBookIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform magicBookObject;

    [SerializeField]
    private Transform followingPoint;

    [SerializeField]
    private float followSpeed = 0.1f;

    [SerializeField]
    private SpriteRenderer magicBookIcon;

    // Start is called before the first frame update
    void Start()
    {
        magicBookObject.transform.parent = null;
        magicBookObject.transform.position = this.transform.position;
        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(WhenMagicBookEquipInfoChanged).AddTo(this);
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
    }

    void Update()
    {
        magicBookObject.transform.position = Vector2.Lerp(magicBookObject.transform.position, this.followingPoint.position, Time.deltaTime * followSpeed);
    }


}
