using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Spine.Unity;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerPet3 : MonoBehaviour
{
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private Transform playerPos;

    private ObscuredFloat moveSpeed = 13f;

    private Transform target;

    [SerializeField]
    private List<GameObject> rendererObjects;

    private void Awake()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {

        ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).AsObservable().Subscribe(e =>
        {
            rendererObjects[0].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.FourSkill1).AsObservable().Subscribe(e =>
        {
            rendererObjects[1].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.FourSkill2).AsObservable().Subscribe(e =>
        {
            rendererObjects[2].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.FourSkill3).AsObservable().Subscribe(e =>
        {
            rendererObjects[3].SetActive(e == 1);

        }).AddTo(this);
    }

    private void Initialize()
    {
        this.transform.parent = null;
    }


    private void OnEnable()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, targetPos.transform.position, Time.deltaTime * moveSpeed * 0.5f);

            if (playerPos.position.x > this.transform.position.x)
            {
                this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            }
            else
            {
                this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            }

            yield return null;
        }
    }

}
