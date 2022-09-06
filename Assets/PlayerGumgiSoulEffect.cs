using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGumgiSoulEffect : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gumgiSoulEffect;

    [SerializeField]
    private GameObject rootObject;

    void Start()
    {
        int grade = (int)(ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiSoulClear].Value / PlayerStats.gumgiSoulDivideNum);

        grade = Mathf.Min(grade, gumgiSoulEffect.Count - 1);

        gumgiSoulEffect.ForEach(e => e.SetActive(false));
        gumgiSoulEffect[grade].SetActive(true);
    }
}
