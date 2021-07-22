using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiAutoSkillSelector : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> toggleObject;

    [SerializeField]
    private List<GameObject> toggleCheckBoxObject;

    //[SerializeField]
    //private GameObject jumpAutoObject;

    //[SerializeField]
    //private GameObject jumpToggleObject;


    void Start()
    {
        SkillCoolTimeManager.LoadSelectedSkill();
       // Subscribe();
    }

    private void Subscribe()
    {
        //AutoManager.Instance.AutoMode.AsObservable().Subscribe(e =>
        //{
        //    toggleObject.ForEach(element => element.gameObject.SetActive(e));
        //    //jumpAutoObject.gameObject.SetActive(e);
        //}).AddTo(this);

        //var list = SkillCoolTimeManager.registeredSkillIdx;

        //for (int i = 0; i < list.Count; i++)
        //{
        //    int idx = i;
        //    list[i].AsObservable().Subscribe(id =>
        //    {
        //        toggleCheckBoxObject[idx].SetActive(id == 1);
        //    }).AddTo(this);
        //}
    }

}
