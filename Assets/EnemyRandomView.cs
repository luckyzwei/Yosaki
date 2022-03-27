using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomView : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> viewList;

    private void OnEnable()
    {
        int rand = Random.Range(0, viewList.Count);

        for (int i = 0; i < viewList.Count; i++)
        {
            viewList[i].SetActive(i == rand);
        }
    }
}
