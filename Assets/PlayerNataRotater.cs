using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerNataRotater : MonoBehaviour
{
    [SerializeField]
    private GameObject rotateObject;

    [SerializeField]
    private GameObject rotateObject_Gang;

    [SerializeField]
    private float rotateSpeed = 0f;

    private float currentAngle;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {

            rotateObject.SetActive(e == 35);
            //강림,일직,월직 이펙트
            rotateObject_Gang.SetActive(e == 48 || e == 51 || e == 53);

        }).AddTo(this);
    }

    void Update()
    {
        currentAngle += Time.deltaTime * rotateSpeed;

        if (rotateObject.activeInHierarchy)
        {
            rotateObject.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }

        if (rotateObject_Gang.activeInHierarchy)
        {
            rotateObject_Gang.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }

        if (currentAngle >= 360f)
        {
            currentAngle = currentAngle - 360f;
        }
    }
}
