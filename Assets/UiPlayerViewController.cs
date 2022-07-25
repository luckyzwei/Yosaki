using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiPlayerViewController : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer sonObject;


    [SerializeField]
    private GameObject sonObject_EFX;

    [SerializeField]
    private GameObject dogObject;

    [SerializeField]
    private GameObject marbleCircleObject;

    [SerializeField]
    private GameObject asuraObject;

    [SerializeField]
    private GameObject akGuiObject;

    [SerializeField]
    private GameObject tailObject;
    //
    [SerializeField]
    private GameObject hyonMu;
    [SerializeField]
    private GameObject baekHo;
    [SerializeField]
    private GameObject pet;
    [SerializeField]
    private GameObject orb;
    [SerializeField]
    private GameObject indra;


    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        SettingData.sonView.AsObservable().Subscribe(e =>
        {
            sonObject.enabled = e == 1;
            sonObject_EFX.SetActive(e == 1);
        }).AddTo(this);

        SettingData.dogView.AsObservable().Subscribe(e =>
        {
            dogObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.marbleCircleView.AsObservable().Subscribe(e =>
        {
            marbleCircleObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.asuarView.AsObservable().Subscribe(e =>
        {
            asuraObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.akGuiView.AsObservable().Subscribe(e =>
        {
            akGuiObject.SetActive(e == 1);

        }).AddTo(this);

        SettingData.tailView.AsObservable().Subscribe(e =>
        {
            tailObject.SetActive(e == 1);

        }).AddTo(this);
        //
        SettingData.hyonMu.AsObservable().Subscribe(e =>
        {
            hyonMu.SetActive(e == 1);

        }).AddTo(this);

        SettingData.baekHo.AsObservable().Subscribe(e =>
        {
            baekHo.SetActive(e == 1);

        }).AddTo(this);

        SettingData.pet.AsObservable().Subscribe(e =>
        {
            pet.SetActive(e == 1);

        }).AddTo(this);

        SettingData.orb.AsObservable().Subscribe(e =>
        {
            orb.SetActive(e == 1);

        }).AddTo(this);

        SettingData.indra.AsObservable().Subscribe(e =>
        {
            indra.SetActive(e == 1);

        }).AddTo(this);
    }

}
