﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using static NetworkManager;
using Photon.Pun;

public class PartyRaidOtherPlayerView_Ingame : MonoBehaviour
{
    [SerializeField]
    private List<UiTopRankerCell> playerView_Room;

    [SerializeField]
    private List<Transform> flippedObject;

    [SerializeField]
    private float yOffset;

    private Dictionary<int, PlayerInfo> roomPlayerDatas;
    private List<int> keys;

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        roomPlayerDatas = PartyRaidManager.Instance.NetworkManager.RoomPlayerDatas;

        keys = roomPlayerDatas.Keys.ToList();

        for (int i = 0; i < playerView_Room.Count; i++)
        {
            if (i < keys.Count)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber != keys[i])
                {
                    playerView_Room[i].gameObject.SetActive(true);

                    var playerInfo = roomPlayerDatas[keys[i]];

                    playerView_Room[i].Initialize(playerInfo.nickName, string.Empty, playerInfo.costumeId, playerInfo.petId, playerInfo.weaponId, playerInfo.magicBookId, playerInfo.gumgi, playerInfo.guildName, playerInfo.mask);
                }
                else 
                {
                    playerView_Room[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerView_Room[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < playerView_Room.Count; i++)
        {
            if (i < roomPlayerDatas.Count)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber != keys[i])
                {
                    playerView_Room[i].gameObject.SetActive(true);

                    float prefX = playerView_Room[i].transform.position.x;

                    playerView_Room[i].transform.position = Vector3.Lerp(playerView_Room[i].transform.position, roomPlayerDatas[keys[i]].currentPos + Vector3.down * yOffset, Time.deltaTime * 2f);

                    if (roomPlayerDatas[keys[i]].currentPos.x >= playerView_Room[i].transform.position.x)
                    {
                        flippedObject[i].transform.localScale = new Vector3(-Mathf.Abs(flippedObject[i].transform.localScale.x), flippedObject[i].transform.localScale.y, flippedObject[i].transform.localScale.z);
                    }
                    else
                    {
                        flippedObject[i].transform.localScale = new Vector3(Mathf.Abs(flippedObject[i].transform.localScale.x), flippedObject[i].transform.localScale.y, flippedObject[i].transform.localScale.z);
                    }
                }
                else 
                {
                    playerView_Room[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerView_Room[i].gameObject.SetActive(false);
            }
        }
    }

}
