using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSusanoBuff : MonoBehaviour
{
    public static bool isImmune = false;

    [SerializeField]
    private Transform buttonObject;

    [SerializeField]
    private Image buffRemainObject;

    [SerializeField]
    private Image buffIcon;

    [SerializeField]
    private TextMeshProUGUI buffRemainSecDesc;

    // Start is called before the first frame update
    void Start()
    {
        bool isNormalField = GameManager.Instance.IsNormalField;
        int susanoIdx = PlayerStats.GetSusanoGrade();


        buffRemainObject.gameObject.SetActive(false);

        if (susanoIdx == -1)
        {
            buttonObject.gameObject.SetActive(false);
        }
        else
        {
            buffIcon.sprite = CommonResourceContainer.GetSusanoIcon();
            buttonObject.gameObject.SetActive(isNormalField == false);
        }
    }

    public void OnClickBuffButton()
    {
        int susanoGrade = PlayerStats.GetSusanoGrade();
        if (susanoGrade == -1) PopupManager.Instance.ShowAlarmMessage("사용하실 수 없습니다.");

        buttonObject.gameObject.SetActive(false);

        StartCoroutine(ImmuneRoutine());

    }

    private IEnumerator ImmuneRoutine()
    {
        int susanoGrade = PlayerStats.GetSusanoGrade();

        var tableData = TableManager.Instance.susanoTable.dataArray[susanoGrade];

        float tick = 0f;

        isImmune = true;

        buffRemainObject.gameObject.SetActive(true);

        int prefSec = -1;

        while (tick < tableData.Buffsec)
        {
            tick += Time.deltaTime;

            int currentSec = (int)(tableData.Buffsec - tick);

            if (prefSec != currentSec)
            {
                buffRemainSecDesc.SetText($"무적 {currentSec}초");
            }

            prefSec = (int)(tableData.Buffsec - tick);

            yield return null;
        }

        buffRemainObject.gameObject.SetActive(false);

        isImmune = false;
    }

}
