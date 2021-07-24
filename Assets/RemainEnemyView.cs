using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class RemainEnemyView : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI remainEnemyText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject spawnGaugeObject;

    [SerializeField]
    private Image spawnGauge;

    private string animName = "Play";

    IEnumerator Start()
    {
        rootObject.SetActive(false);
        yield return null;
        UpdateText();
        yield return null;
        Initialize();
    }

    private void Initialize()
    {
        rootObject.SetActive(MapInfo.Instance != null);

        if (MapInfo.Instance == null)
        {
            spawnGaugeObject.SetActive(false);
            return;
        }

        MapInfo.Instance.whenSpawnedEnemyCountChanged.AsObservable().Subscribe(WhenEnemyCountChanged).AddTo(this);

        float spawnDelay = GameManager.Instance.CurrentStageData.Spawndelay;

        if (GameManager.contentsType == GameManager.ContentsType.NormalField && spawnDelay != 0f)
        {
            float divideNum = spawnDelay + GameBalance.spawnIntervalTime * GameBalance.spawnDivideNum;
            MapInfo.Instance.spawnGaugeValue.AsObservable().Subscribe(e =>
            {
                spawnGauge.fillAmount = e / divideNum;
            }).AddTo(this);
        }
        else
        {
            spawnGaugeObject.SetActive(false);
        }

    }

    private void WhenEnemyCountChanged(Unit unit)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (MapInfo.Instance == null) return;
      //  animator.SetTrigger(animName);
        remainEnemyText.SetText($"몹 {MapInfo.Instance.SpawnedEnemyList.Count.ToString()}");
    }
}
