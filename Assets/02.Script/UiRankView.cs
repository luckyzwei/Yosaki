using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRankView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text1;

    [SerializeField]
    private TextMeshProUGUI text2;

    [SerializeField]
    private TextMeshProUGUI text3;

    [SerializeField]
    private List<GameObject> rankList;

    public static int rank1Count = 0;

    public void Initialize(string text1, string text2, string text3, int rank, int costumeId, int petId, int weaponId, int magicBookId, int fightpoint)
    {
        this.text1.SetText(text1);
        this.text2.SetText(text2);
        this.text3.SetText(text3);

        this.text1.gameObject.SetActive(rank != 1 && rank != 2 && rank != 3);

        rankList[0].SetActive(rank == 1);
        rankList[1].SetActive(rank == 2);
        rankList[2].SetActive(rank == 3);

        UiTopRankerCell rankerCell = null;

        if (rank == 1)
        {
            if (rank1Count == 0)
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[0];
            }
            else if (rank1Count == 1) 
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[1];
            }
            else if (rank1Count == 2) 
            {
                rankerCell = UiTopRankerView.Instance.RankerCellList[2];
            }

            rank1Count++;
        }
        else if (rank == 2)
        {
            rankerCell = UiTopRankerView.Instance.RankerCellList[1];
        }
        else if (rank == 3)
        {
            rankerCell = UiTopRankerView.Instance.RankerCellList[2];
        }

        if (rankerCell != null)
        {
            rankerCell.gameObject.SetActive(true);
            rankerCell.Initialize(text2, text3, costumeId, petId, weaponId, magicBookId, fightpoint);
        }
    }
}
