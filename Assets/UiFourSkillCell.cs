using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiFourSkillCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    private SkillTableData skillTableData;

    [SerializeField]
    private Image skillIcon;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;


    public void Initialize(SkillTableData skillTableData)
    {
        this.skillTableData = skillTableData;

        skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(skillTableData.Id);



        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData($"FS{skillTableData.Sonunlocklevel - 1}").AsObservable().Subscribe(e =>
        {
            //있으면
            if (e > 0)
            {
                lockMask.SetActive(false);
                description.SetText(skillTableData.Skilldesc + $"\n피해량 :  {Utils.ConvertBigNum(ServerData.skillServerTable.GetSkillDamagePer(skillTableData.Id, applySkillDamAbility: false) * 100f)}%");

            }
            else
            {
                lockMask.SetActive(true);
                lockDescription.SetText($"수미숲 기술 획득시 개방");
                description.SetText(skillTableData.Skilldesc + $"\n피해량 : {0}%");
                levelDescription.SetText($"LV : {0}");
            }
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).AsObservable().Subscribe(sumiLevel =>
        {
            if(ServerData.goodsTable.GetTableData($"FS{skillTableData.Sonunlocklevel - 1}").Value==0)
            {
                return;
            }
            levelDescription.SetText($"LV : {sumiLevel}");

        }).AddTo(this);
    }

}
