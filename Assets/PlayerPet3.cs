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
    private Animator animator;

    [SerializeField]
    private GameObject rendererObject;

    [SerializeField]
    private List<GameObject> effects;

    private void Awake()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {

        ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).AsObservable().Subscribe(level =>
        {
            rendererObject.SetActive(HasFourSkillCheck());
            
            int idx = GameBalance.GetSonIdx();

            animator.runtimeAnimatorController = CommonUiContainer.Instance.sonAnimators[GameBalance.GetSonIdx()];

            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].SetActive(i == idx);
            }

        }).AddTo(this);
    }

    private bool HasFourSkillCheck()
    {
        if(ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).Value==1||
            ServerData.goodsTable.GetTableData(GoodsTable.FourSkill1).Value == 1 ||
            ServerData.goodsTable.GetTableData(GoodsTable.FourSkill2).Value == 1 ||
            ServerData.goodsTable.GetTableData(GoodsTable.FourSkill3).Value == 1
            )
        {
            return true;
        }
        else
        {
            return false;
        }    
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
