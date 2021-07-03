﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Spine.Unity;

public class UiCostumeCell : MonoBehaviour
{
    private CostumeData costumeData;

    private bool subscribed = false;

    //실제로 장착중
    [SerializeField]
    private GameObject selectedObject;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI costumeName;

    public void Initialize(CostumeData costumeData)
    {
        this.costumeData = costumeData;

        SetCostumeSpine(costumeData.Id);

        costumeName.SetText(costumeData.Name);

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }

    private void SetCostumeSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

    private void Subscribe()
    {
        DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(idx =>
        {
            selectedObject.SetActive(idx == costumeData.Id);
        }).AddTo(this);
    }

    public void OnClickCostume()
    {
        if (DatabaseManager.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.Value == true)
        {
            DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value = costumeData.Id;

            //서버 저장
            DatabaseManager.equipmentTable.SyncData(EquipmentTable.CostumeLook);
        }
        else 
        {
            PopupManager.Instance.ShowAlarmMessage("외형이 없습니다.");
        }

      //  InitBoard();
    }
    private void InitBoard()
    {
       // UiCostume.Instance.WhenSelectIdxChanged(costumeData.Id);
        UiCostumeAbilityBoard.Instance.Initialize(costumeData);
    }
}
