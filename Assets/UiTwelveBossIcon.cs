using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTwelveBossIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private List<Sprite> sprites;

    // Start is called before the first frame update
    void Start()
    {
        int id = GameManager.Instance.bossId;

        if (id >= 30 && id <= 38)
        {
            id = 30;
        }
        else if (id > 38)
        {
            id -= 8;
        }

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
