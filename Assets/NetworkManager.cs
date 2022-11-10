using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using TMPro;
using UniRx;
using System.Linq;
using static GameManager;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public enum PlayerState
    {
        Lobby, Room, Loading, Playing, End
    }

    public enum MatchingPlatform
    {
        And, IOS
    }

    public class PlayerInfo
    {
        public Player player;
        public string nickName;
        public int weaponId;
        public int magicBookId;
        public int petId;
        public int costumeId;
        public int gumgi;
        public string guildName;
        public int mask;
        public int horn;
        public double score;
        public bool endGame = false;
        public Vector3 currentPos;
        public int level;
        public MatchingPlatform platform;
        public bool leftRoom = false;
    }

    public ReactiveProperty<PlayerState> playerState = new ReactiveProperty<PlayerState>(PlayerState.Lobby);


    private Dictionary<int, PlayerInfo> roomPlayerDatas = new Dictionary<int, PlayerInfo>();

    public Dictionary<int, PlayerInfo> RoomPlayerDatas => roomPlayerDatas;

    [SerializeField]
    private List<UiTopRankerCell> playerView_Room;

    [SerializeField]
    private List<UiTopRankerCell> playerView_Result;


    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public TextMeshProUGUI RoomInfoText;


    public GameObject scoreBoardPanel;

    [Header("ETC")]
    public Text StatusText;

    //private PhotonView PV;

    List<RoomInfo> roomList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    [SerializeField]
    private Button makeRoomButton;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private Button connectButton;

    [SerializeField]
    private TextMeshProUGUI logText;

    [SerializeField]
    private Toggle visibleRoomToggle;

    [SerializeField]
    private TMP_InputField roomNameInput;

    [SerializeField]
    private TMP_InputField roomNameInput_make;

    private Coroutine startGameRoutine;

    [SerializeField]
    private GameObject loadingMask;

    [SerializeField]
    private TMP_InputField chatInput;

    [SerializeField]
    private TextMeshProUGUI chatText;

    [SerializeField]
    private List<string> chatList = new List<string>();

    [SerializeField]
    private Button chatButton;

    [SerializeField]
    private GameObject partyRaidResultBoard;

    public ReactiveCommand whenScoreInfoReceived = new ReactiveCommand();

    private string AndPlatformKey = "AND";
    private string IOSPlatformKey = "IOS";
    private MatchingPlatform myPlatform;

    public ContentsType contentsType;


    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자


    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (roomList.Count % CellBtn.Length == 0) ? roomList.Count / CellBtn.Length : roomList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < roomList.Count) ? true : false;
            CellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText((multiple + i < roomList.Count) ? roomList[multiple + i].Name : "");
            CellBtn[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText((multiple + i < roomList.Count) ? roomList[multiple + i].PlayerCount + "/" + roomList[multiple + i].MaxPlayers : "");

            if (multiple + i < roomList.Count &&
                roomList[multiple + i].CustomProperties != null &&
               roomList[multiple + i].CustomProperties.Count != 0)
            {
                CellBtn[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText((multiple + i < roomList.Count) ? (string)roomList[multiple + i].CustomProperties["M"]:"");
                CellBtn[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("방장 : ");
            }
            else
            {
                CellBtn[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("");
                CellBtn[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("");
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!this.roomList.Contains(roomList[i])) this.roomList.Add(roomList[i]);
                else this.roomList[this.roomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (this.roomList.IndexOf(roomList[i]) != -1) this.roomList.RemoveAt(this.roomList.IndexOf(roomList[i]));
        }
        MyListRenewal();

        roomUpdateButton.interactable = true;
    }
    #endregion


    #region 서버연결
    void Awake()
    {
        StartCoroutine(UpdateServerState());
    }

    IEnumerator UpdateServerState()
    {
        WaitForSeconds updateDelay = new WaitForSeconds(5.0f);

        while (true)
        {
            if (playerState.Value == PlayerState.Lobby && PhotonNetwork.IsConnectedAndReady)
            {
                StatusText.text = PhotonNetwork.NetworkClientState.ToString();
                LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
            }

            yield return updateDelay;
        }
    }

    [SerializeField]
    private GameObject connectMask;

    public void Connect()
    {
        int costume = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int weapon = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicbook = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        int pet = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int gumgi = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value;
        int mask = ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value;
        int horn = ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value;
        string guildName = GuildManager.Instance.myGuildName;
        int level = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        //닉네임,코스튬,무기,마법책,펫,검기,길드명
        string platform = string.Empty;
#if UNITY_ANDROID
        platform = AndPlatformKey;
        myPlatform = MatchingPlatform.And;
#endif
#if UNITY_IOS
        platform = IOSPlatformKey;
        myPlatform = MatchingPlatform.IOS;
#endif
        PhotonNetwork.LocalPlayer.NickName = $"{PlayerData.Instance.NickName},{costume},{weapon},{magicbook},{pet},{gumgi},{guildName},{mask},{level},{platform}";

        connectMask.SetActive(true);

        connectButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }


    public MatchingPlatform GetPlayerMatchingPlatform(string nickName)
    {
        if (nickName.Contains(AndPlatformKey))
        {
            return MatchingPlatform.And;
        }
        else
        {
            return MatchingPlatform.IOS;
        }
    }

    public PlayerInfo GetPlayerInfo(string msg)
    {
        var splits = msg.Split(',');
        PlayerInfo ret = new PlayerInfo();
        ret.nickName = splits[0];
        ret.costumeId = int.Parse(splits[1]);
        ret.weaponId = int.Parse(splits[2]);
        ret.magicBookId = int.Parse(splits[3]);
        ret.petId = int.Parse(splits[4]);
        ret.gumgi = int.Parse(splits[5]);
        ret.guildName = splits[6];
        ret.mask = int.Parse(splits[7]);
        ret.level = int.Parse(splits[8]);
        ret.platform = splits[9] == AndPlatformKey ? MatchingPlatform.And : MatchingPlatform.IOS;

        return ret;
    }

    public override void OnConnectedToMaster()
    {
#if UNITY_ANDROID
        var lobbyType = new TypedLobby("And", LobbyType.Default);
        PhotonNetwork.JoinLobby(lobbyType);
#endif

#if UNITY_IOS
        var lobbyType = new TypedLobby("IOS", LobbyType.Default);
        PhotonNetwork.JoinLobby(lobbyType);
#endif
    }

    public override void OnJoinedLobby()
    {
        connectMask.SetActive(false);
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        scoreBoardPanel.SetActive(false);
        playerState.Value = PlayerState.Lobby;

        roomList.Clear();

        CheckGuildRaidEnter();
    }

    private void CheckGuildRaidEnter()
    {
        if (contentsType == ContentsType.PartyRaid_Guild)
        {
            JoinOrCreateGuildRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ShowDisconnectMessage(cause);
        ResetButtons();
        roomPlayerDatas.Clear();

        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        scoreBoardPanel.SetActive(false);
        partyRaidResultBoard.SetActive(false);
    }

    private void ResetButtons()
    {
        makeRoomButton.interactable = true;
        connectButton.interactable = true;
    }

    private void ShowDisconnectMessage(DisconnectCause cause)
    {


        switch (cause)
        {
            case DisconnectCause.None:
                break;
            case DisconnectCause.ExceptionOnConnect:
                break;
            case DisconnectCause.DnsExceptionOnConnect:
                break;
            case DisconnectCause.ServerAddressInvalid:
                break;
            case DisconnectCause.Exception:
                break;
            case DisconnectCause.ServerTimeout:
                break;
            case DisconnectCause.ClientTimeout:
                break;
            case DisconnectCause.DisconnectByServerLogic:
                break;
            case DisconnectCause.DisconnectByServerReasonUnknown:
                break;
            case DisconnectCause.InvalidAuthentication:
                break;
            case DisconnectCause.CustomAuthenticationFailed:
                break;
            case DisconnectCause.AuthenticationTicketExpired:
                break;
            case DisconnectCause.MaxCcuReached:
                break;
            case DisconnectCause.InvalidRegion:
                break;
            case DisconnectCause.OperationNotAllowedInCurrentState:
                break;
            case DisconnectCause.DisconnectByClientLogic:
                {
                    //내가 disconnect 호출
                    return;
                }
                break;
            case DisconnectCause.DisconnectByOperationLimit:
                break;
            case DisconnectCause.DisconnectByDisconnectMessage:
                break;
            case DisconnectCause.ApplicationQuit:
                break;
        }

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"연결 끊김 {cause.ToString()}", null);
    }
    #endregion

    public void ShowPartyRaidResultPopup()
    {
        partyRaidResultBoard.SetActive(true);
    }


    #region 방
    public void JoinOrCreateGuildRoom()
    {
        if (GuildManager.Instance.hasGuild.Value == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"문파가 없습니다");
            return;
        }

        PhotonNetwork.JoinOrCreateRoom(GuildManager.Instance.myGuildName + CommonString.GuildText, new RoomOptions { MaxPlayers = 10, IsVisible = false }, lobbyType);

    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput_make.text))
        {
            PopupManager.Instance.ShowAlarmMessage($"파티 이름을 입력 해 주세요");
            return;
        }

        if (Utils.HasBadWord(roomNameInput_make.text) || roomNameInput_make.text.Contains(CommonString.GuildText))
        {
            PopupManager.Instance.ShowAlarmMessage($"부적절한 이름이 포함되어 있습니다.");
            return;
        }

        makeRoomButton.interactable = false;



        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;
        roomOption.IsVisible = !visibleRoomToggle.isOn;


        Hashtable hashtables = new Hashtable();
        hashtables.Add("M", Utils.GetOriginNickName(PlayerData.Instance.NickName));

        string[] propertiesListedInLobby = new string[1];
        propertiesListedInLobby[0] = "M";

        roomOption.CustomRoomProperties = hashtables;
        roomOption.CustomRoomPropertiesForLobby = propertiesListedInLobby;

        PhotonNetwork.CreateRoom(roomNameInput_make.text, roomOption);
    }

    //방 참가
    //일반적인 방법(방에서 클릭)
    public void MyListClick(int num)
    {
        if (num == -2)
        {
            --currentPage;
        }
        else if (num == -1)
        {
            ++currentPage;
        }
        else
        {
            PhotonNetwork.JoinRoom(roomList[multiple + num].Name);
        }

        MyListRenewal();
    }


    //랜덤룸 참가
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //이름 검색으로 참가
    public void JoinRoomByName()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            PopupManager.Instance.ShowAlarmMessage("파티 이름을 입력 해주세요");
        }
        else
        {

            PhotonNetwork.JoinRoom(roomNameInput.text);
        }
    }

    //방 나가기
    public void LeaveRoom()
    {
        contentsType = ContentsType.PartyRaid;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        RoomPanel.SetActive(true);
        scoreBoardPanel.SetActive(false);
        makeRoomButton.interactable = true;
        playerState.Value = PlayerState.Room;
        startGameButton.interactable = true;

        ResetRoomState();
        RoomRenewal();
        UpdatePlayerInfoList();

        PlatformCheck();
        GuildCheck();
    }

    private void GuildCheck()
    {
        if (IsGuildBoss() == false) return;

        if (string.IsNullOrEmpty(GuildManager.Instance.myGuildName))
        {
            LeaveRoom();
            return;
        }

        if (PhotonNetwork.CurrentRoom.Name.Contains(GuildManager.Instance.myGuildName) == false)
        {
            LeaveRoom();
            return;
        }
    }

    private void PlatformCheck()
    {
#if UNITY_ANDROID
        var e = roomPlayerDatas.GetEnumerator();

        bool hasIosPlatformUser = false;

        while (e.MoveNext())
        {
            if (e.Current.Value.platform == MatchingPlatform.IOS)
            {
                hasIosPlatformUser = true;
                break;
            }
        }

        if (hasIosPlatformUser && myPlatform != MatchingPlatform.IOS)
        {
            LeaveRoom();
        }
#endif
#if UNITY_IOS
        var e = roomPlayerDatas.GetEnumerator();

        bool hasAndroidPlatformUser = false;

        while (e.MoveNext())
        {
            if (e.Current.Value.platform == MatchingPlatform.And)
            {
                hasAndroidPlatformUser = true;
                break;
            }
        }

        if (hasAndroidPlatformUser && myPlatform != MatchingPlatform.And)
        {
            LeaveRoom();
        }
#endif
    }


    public void UpdatePlayerInfoList()
    {
        //데이터 안빠지게
        if (playerState.Value == PlayerState.Playing || playerState.Value == PlayerState.End)
        {
            return;
        }

        roomPlayerDatas.Clear();
        var players = PhotonNetwork.PlayerList;

        if (players != null && players.Length != 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                var playerInfoData = GetPlayerInfo(players[i].NickName);

                if (roomPlayerDatas.ContainsKey(players[i].ActorNumber) == false)
                {
                    roomPlayerDatas.Add(players[i].ActorNumber, playerInfoData);
                }
            }
        }

        var keys = roomPlayerDatas.Keys.ToList();

        for (int i = 0; i < playerView_Room.Count; i++)
        {
            if (i < keys.Count)
            {
                playerView_Room[i].gameObject.SetActive(true);

                var playerInfo = roomPlayerDatas[keys[i]];

                playerView_Room[i].Initialize(playerInfo.nickName, string.Empty, playerInfo.costumeId, playerInfo.petId, playerInfo.weaponId, playerInfo.magicBookId, playerInfo.gumgi, playerInfo.guildName, playerInfo.mask, playerInfo.horn);
                playerView_Room[i].SetLevelText(playerInfo.level);

                playerView_Result[i].gameObject.SetActive(true);

                playerView_Result[i].Initialize(playerInfo.nickName, string.Empty, playerInfo.costumeId, playerInfo.petId, playerInfo.weaponId, playerInfo.magicBookId, playerInfo.gumgi, playerInfo.guildName, playerInfo.mask, playerInfo.horn);
            }
            else
            {
                playerView_Room[i].gameObject.SetActive(false);

                playerView_Result[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateResultScore()
    {
        var keys = roomPlayerDatas.Keys.ToList();

        for (int i = 0; i < playerView_Room.Count; i++)
        {
            if (i < keys.Count)
            {
                var playerInfo = roomPlayerDatas[keys[i]];

                playerView_Result[i].gameObject.SetActive(true);

                playerView_Result[i].UpdatePartyRaidScore(playerInfo.score, playerInfo.endGame);

            }
            else
            {
                playerView_Result[i].gameObject.SetActive(false);
            }
        }
    }

    private void ResetRoomState()
    {
        startGameRoutine = null;
        roomPlayerDatas.Clear();
        chatText.SetText(string.Empty);
        chatList.Clear();
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (message.Equals("A game with the specified id already exist."))
        {

            PopupManager.Instance.ShowAlarmMessage($"이미 존재하는 방 입니다!");
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"파티 생성 실패!\n{message}");

        }

        makeRoomButton.interactable = true;

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (message.Equals("No match found") || message.Equals("Game does not exist"))
        {
            PopupManager.Instance.ShowAlarmMessage($"파티가 없습니다.");

        }
        else
        {

        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (message.Equals("Game does not exist"))
        {
            PopupManager.Instance.ShowAlarmMessage($"파티가 없습니다.");
        }
        else if (message.Equals("Game full"))
        {
            PopupManager.Instance.ShowAlarmMessage($"인원이 가득 찼습니다.");
        }
        else
        {

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerInfoList();
        RoomRenewal();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerInfoList();
        UpdateRecommendButton(otherPlayer);
        RoomRenewal();
    }

    private void UpdateRecommendButton(Player otherPlayer)
    {
        if (roomPlayerDatas.ContainsKey(otherPlayer.ActorNumber))
        {
            for (int i = 0; i < playerView_Result.Count; i++)
            {
                if (Utils.GetOriginNickName(roomPlayerDatas[otherPlayer.ActorNumber].nickName).Equals(Utils.GetOriginNickName(playerView_Result[i].recNickName)))
                {
                    playerView_Result[i].OnPlayerLeftInPartyRaid();
                    break;
                }
            }
        }
    }

    void RoomRenewal()
    {
        RoomInfoText.SetText(PhotonNetwork.CurrentRoom.Name);

        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
    #endregion


    #region 이벤트들

    byte SendScore_Event = 0;
    byte StartBossContents_Event = 1;
    byte SendChat_Event = 2;
    byte SendRecommend_Event = 3;

    public void SendRecommend(string nickName)
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.canRecommendCount].Value <= 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"이번주는 더이상 추천하실 수 없습니다\n매주 {GameBalance.recommendCountPerWeek}회 추천 가능!", null);
            return;
        }

        if (Utils.GetOriginNickName(PlayerData.Instance.NickName).Equals(Utils.GetOriginNickName(nickName)))
        {
            PopupManager.Instance.ShowAlarmMessage("자기 자신은 추천 하실 수 없습니다.");
            return;
        }

        Debug.LogError("Send Recommend@@@");

        ServerData.userInfoTable.TableDatas[UserInfoTable.canRecommendCount].Value--;
        ServerData.userInfoTable.UpData(UserInfoTable.canRecommendCount, false);

        object[] objects = new object[] { Utils.GetOriginNickName(nickName), Utils.GetOriginNickName(PlayerData.Instance.NickName) };

        PhotonNetwork.RaiseEvent(SendRecommend_Event, objects, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void SendScoreInfo(double score, bool end = false)
    {
        var playerPos = PlayerMoveController.Instance.transform.position;

        string posInfo = $"{playerPos.x},{playerPos.y}";

        object[] objects = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, score, end, posInfo };

        PhotonNetwork.RaiseEvent(SendScore_Event, objects, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void SendStartGameAlarm()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PopupManager.Instance.ShowYesNoPopup("알림", "토벌을 시작 할까요?", () =>
            {

                if (IsGuildBoss() == false)
                {
                    if (roomPlayerDatas.Count > 4)
                    {
                        PopupManager.Instance.ShowAlarmMessage("인원이 초과됐습니다.");
                        return;
                    }
                }
                else
                {
                    if (roomPlayerDatas.Count > 10)
                    {
                        PopupManager.Instance.ShowAlarmMessage("인원이 초과됐습니다.");
                        return;
                    }
                }



                startGameButton.interactable = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                object[] objects = new object[] { "게임시작알림" };
                PhotonNetwork.RaiseEvent(StartBossContents_Event, objects, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);

            }, null);
        }
    }

    public void SendChat()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.chatBan).Value == 1f)
        {
            PopupManager.Instance.ShowAlarmMessage("채팅이 차단된 상태입니다.");
            return;
        }

        if (string.IsNullOrEmpty(chatInput.text))
        {
            PopupManager.Instance.ShowAlarmMessage("메세지를 입력해 주세요!");
            return;
        }

        if (Utils.HasBadWord(chatInput.text))
        {
            PopupManager.Instance.ShowAlarmMessage("부적절한 메세지가 포함되어 있습니다.");
            return;
        }

        if (chatInput.text.Length >= 19)
        {
            PopupManager.Instance.ShowAlarmMessage("채팅은 20자 까지만 발송하실 수 있습니다.");
            return;
        }


        object[] objects = new object[] { $"{PlayerData.Instance.NickName.Replace(CommonString.IOS_nick, "")} : {chatInput.text}" };

        PhotonNetwork.RaiseEvent(SendChat_Event, objects, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);

        chatInput.text = string.Empty;

        StartCoroutine(ChatCoolRoutine());
    }

    private IEnumerator ChatCoolRoutine()
    {
        chatButton.interactable = false;
        yield return new WaitForSeconds(1.0f);
        chatButton.interactable = true;
    }

    private char splitComma = ',';
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SendScore_Event)
        {
            object[] data = (object[])photonEvent.CustomData;

            int actorNum = (int)data[0];
            double score = (double)data[1];
            bool endGame = (bool)data[2];
            string[] currentPos = ((string)data[3]).Split(splitComma);

            if (roomPlayerDatas.ContainsKey(actorNum))
            {
                roomPlayerDatas[actorNum].score = score;
                roomPlayerDatas[actorNum].endGame = endGame;

                if (currentPos.Length >= 2)
                {
                    roomPlayerDatas[actorNum].currentPos = new Vector3(float.Parse(currentPos[0]), float.Parse(currentPos[1]), 0);
                }

                UpdateScoreBoard();

                whenScoreInfoReceived.Execute();
            }
        }
        else if (photonEvent.Code == StartBossContents_Event)
        {
            // logText.SetText($"게임시작 알림 받음!");
            Debug.LogError($"게임시작 알림 받음!");

            if (startGameRoutine == null)
            {
                startGameRoutine = StartCoroutine(StartGameRoutine());
            }
        }
        else if (photonEvent.Code == SendChat_Event)
        {
            object[] data = (object[])photonEvent.CustomData;

            string message = (string)data[0];

            //
            chatList.Add(message);

            if (chatList.Count >= chatMax)
            {
                chatList.RemoveAt(0);
            }

            UpdateChatText();
        }
        else if (photonEvent.Code == SendRecommend_Event)
        {
            object[] data = (object[])photonEvent.CustomData;

            string targetNickName = (string)data[0];

            string recommendedNickName = (string)data[1];

            if (Utils.GetOriginNickName(PlayerData.Instance.NickName).Equals(targetNickName))
            {
                Debug.LogError("추천 받음");

                PopupManager.Instance.ShowAlarmMessage($"{recommendedNickName}님에게 추천 받았습니다!");

                var rewardData = ServerData.bossServerTable.TableDatas["b68"];

                if (string.IsNullOrEmpty(rewardData.score.Value))
                {
                    rewardData.score.Value = "1";
                }
                else
                {
                    int prefScore = int.Parse(rewardData.score.Value);

                    prefScore++;

                    rewardData.score.Value = prefScore.ToString();
                }

                ServerData.bossServerTable.UpdateData("b68");
            }
        }
    }

    private int chatMax = 12;

    private void UpdateChatText()
    {
        string message = string.Empty;

        for (int i = 0; i < chatMax; i++)
        {
            if (i < chatList.Count)
            {
                message += $"{chatList[i]}\n";
            }
            else
            {
                break;
            }
        }

        chatText.SetText(message);
    }

    private void UpdateScoreBoard()
    {
        if (PartyRaidTotalScoreBoard.Instance != null)
        {
            PartyRaidTotalScoreBoard.Instance.UpdateScoreBoard(roomPlayerDatas);
        }

        if (PartyRaidResultPopup.Instance != null && playerState.Value == PlayerState.End)
        {
            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
    }


    private IEnumerator StartGameRoutine()
    {
        loadingMask.SetActive(true);

        int startTime = 5;

        while (true)
        {
            for (int i = 0; i < startTime; i++)
            {
                PopupManager.Instance.ShowAlarmMessage($"게임이 {startTime - i}초뒤 시작됩니다.");
                yield return new WaitForSeconds(1.0f);
            }

            break;
        }

        //게임 씬 로드 + 게임 시작

        loadingMask.SetActive(false);

        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        playerState.Value = PlayerState.Playing;

        if (IsGuildBoss())
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.PartyRaid_Guild);
        }
        else
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.PartyRaid);
        }
    }

    public bool IsGuildBoss()
    {
        return PhotonNetwork.CurrentRoom.Name.Contains(CommonString.GuildText);
    }

    public double GetTotalScore()
    {
        double ret = 0f;

        var e = roomPlayerDatas.GetEnumerator();

        while (e.MoveNext())
        {
            ret += e.Current.Value.score;
        }

        return ret;
    }
    [SerializeField]
    private Button roomUpdateButton;

    TypedLobby lobbyType;

    public void OnClickRoomUpdate()
    {
        roomUpdateButton.interactable = false;

#if UNITY_ANDROID
        lobbyType = new TypedLobby("And", LobbyType.Default);
#endif

#if UNITY_IOS
        lobbyType = new TypedLobby("IOS", LobbyType.Default);
#endif

        PhotonNetwork.GetCustomRoomList(lobbyType, "C0");
    }


    #endregion
}
