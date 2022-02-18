using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerGuildPetView : MonoBehaviour
{
    [SerializeField]
    private GameObject petView;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe() 
    {

        GuildManager.Instance.guildInfoData.AsObservable().Subscribe(e =>
        {

            petView.SetActive(e != null);

        }).AddTo(this);

    }
}
