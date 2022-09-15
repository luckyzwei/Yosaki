using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private class PlayerInfo
    {
        public Player player;
        public string nickName;
        public int weaponId;
        public int magicBookId;
        public int petId;
        public int costumeId;
        public int gumgiId;
    }

    private Dictionary<int, PlayerInfo> roomPlayerDatas = new Dictionary<int, PlayerInfo>();


    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;

    //private PhotonView PV;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private TextMeshProUGUI logText;


    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결
    //void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
    }
    #endregion


    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(PlayerData.Instance.NickName, new RoomOptions { MaxPlayers = 4 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        RoomPanel.SetActive(true);
        RoomRenewal();


        //ChatInput.text = "";
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PopupManager.Instance.ShowAlarmMessage($"방 생성 실패!\n{message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PopupManager.Instance.ShowAlarmMessage($"방 참가 실패!\n{message}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (roomPlayerDatas.ContainsKey(newPlayer.ActorNumber) == false)
        {
            roomPlayerDatas.Add(newPlayer.ActorNumber, new PlayerInfo() { player = newPlayer });
        }

        RoomRenewal();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (roomPlayerDatas.ContainsKey(otherPlayer.ActorNumber))
        {
            roomPlayerDatas[otherPlayer.ActorNumber] = null;
        }

        RoomRenewal();
    }

    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";


        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
    #endregion


    #region 이벤트들

    byte SendScore_Event = 0;
    byte StartBossContents_Event = 1;
    byte SendPlayerInfo = 2;

    public void SendScoreInfo()
    {
        double score = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        object[] objects = new object[] { PlayerData.Instance.NickName, score };

        PhotonNetwork.RaiseEvent(SendScore_Event, objects, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void SendStartGameAlarm()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            object[] objects = new object[] { "게임시작알림" };
            PhotonNetwork.RaiseEvent(StartBossContents_Event, objects, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SendScore_Event)
        {
            object[] data = (object[])photonEvent.CustomData;

            logText.SetText($"이벤트 받음 닉: {data[0].ToString()} 점수 : {(double)data[1]}");
            Debug.LogError($"이벤트 받음 닉: {data[0].ToString()} 점수 : {(double)data[1]}");
        }
        else if (photonEvent.Code == StartBossContents_Event)
        {
            logText.SetText($"게임시작 알림 받음!");
            Debug.LogError($"게임시작 알림 받음!");
        }
    }


    #endregion
}
