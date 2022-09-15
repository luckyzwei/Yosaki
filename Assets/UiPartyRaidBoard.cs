using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UniRx;
using static UnityEngine.UI.CanvasScaler;

public class UiPartyRaidBoard : MonoBehaviourPunCallbacks
{
    private enum ServerState
    {
        Connecting, Connected, Disconnected
    }

    [SerializeField]
    private TextMeshProUGUI currentStateDesc;

    [SerializeField]
    private TextMeshProUGUI currentStateDesc_updated;

    private ReactiveProperty<ServerState> serverState = new ReactiveProperty<ServerState>(ServerState.Disconnected);

    private List<Player> players = new List<Player>();

    private void Awake()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        serverState.AsObservable().Subscribe(state =>
        {
            switch (state)
            {
                case ServerState.Connecting:
                    {
                        currentStateDesc.SetText("로비 입장중..");
                    }
                    break;
                case ServerState.Connected:
                    {
                        currentStateDesc.SetText("로비 입장 성공!");

                    }
                    break;
                case ServerState.Disconnected:
                    {
                        currentStateDesc.SetText("연결 끊김");
                        players.Clear();
                    }
                    break;

            }
        }).AddTo(this);
    }

    private new void OnEnable()
    {
        base.OnEnable();
        ConnectToServer();
    }

    private new void OnDisable()
    {
        base.OnDisable();
        Disconnect();
    }

#if UNITY_EDITOR
    private void Update()
    {
        currentStateDesc_updated.SetText(PhotonNetwork.NetworkClientState.ToString());
    }
#endif

    /// <summary>
    /// 서버에 접속
    /// </summary>
    public void ConnectToServer()
    {
        if (serverState.Value != ServerState.Disconnected)
        {
            return;
        }

        serverState.Value = ServerState.Connecting;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PrintLog("포톤 마스터서버 접속됨");
        PhotonNetwork.LocalPlayer.NickName = PlayerData.Instance.NickName;
        PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues(PlayerData.Instance.NickName);
        JoinLobby();
    }
    /// <summary>
    /// 연결 끊기
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PrintLog($"서버 연결 끊김 {cause.ToString()}");
        serverState.Value = ServerState.Disconnected;
    }
    /// <summary>
    /// 로비 접속
    /// </summary>
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        serverState.Value = ServerState.Connected;
        PrintLog("로비 접속 성공!");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(PlayerData.Instance.NickName, GetRoomOptions(4, true));
    }

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(PlayerData.Instance.NickName, GetRoomOptions(4, true), null);
    }

    //isvisible 옵션으로 비밀방 만들수있음
    private RoomOptions GetRoomOptions(byte maxPlayer, bool isVisible)
    {
        return new RoomOptions { MaxPlayers = maxPlayer, IsVisible = isVisible };
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        PrintLog("방 생성 성공!");
    }

    public override void OnJoinedRoom()
    {
        PrintLog("방 참가 성공!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PrintLog($"방 생성 실패! {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PrintLog($"방 참가 실패! {message}");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void PrintLog(string log)
    {
#if UNITY_EDITOR
        Debug.LogError(log);
#endif
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

    }
}
