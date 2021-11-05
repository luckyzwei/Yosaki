using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Spine.Unity;
using CodeStage.AntiCheat.ObscuredTypes;

public class PlayerPet2 : MonoBehaviour
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
    private GameObject effect;

    private void Awake()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Son_Level).AsObservable().Subscribe(level =>
        {
            rendererObject.SetActive(level > 0);

            int idx = GameBalance.GetSonIdx();

            animator.runtimeAnimatorController = CommonUiContainer.Instance.sonAnimators[GameBalance.GetSonIdx()];

            effect.SetActive(idx > 0);
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
