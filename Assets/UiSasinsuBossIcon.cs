using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSasinsuBossIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private List<Sprite> sprites;

    // Start is called before the first frame update
    void Start()
    {
        int id = GameManager.Instance.bossId;

        if (id >= sprites.Count)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.sprite = sprites[id];
        }

    }
}
