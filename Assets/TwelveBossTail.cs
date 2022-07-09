using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwelveBossTail : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> foxTails;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int idx = GameManager.Instance.bossId - 30;

        for (int i = 0; i < foxTails.Count; i++)
        {
            foxTails[i].SetActive(idx >= i);
        }

    }
}
