using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;
using System;
using UniRx;
using LitJson;

public enum RankType
{
    Level, Boss, Infinity,Boss1, None
}

public class RankManager : SingletonMono<RankManager>
{
    private Dictionary<RankType, RankInfo> myRankInfo = new Dictionary<RankType, RankInfo>()
    {
        //채팅 표기 우선순위
        { RankType.Boss1,null },
        { RankType.Boss,null },
        { RankType.Level,null },
        { RankType.Infinity,null }
    };

    public Dictionary<RankType, RankInfo> MyRankInfo => myRankInfo;

    public class RankInfo
    {
        public RankInfo(string NickName, int Rank, float Score, int costumeIdx, int petIddx, int weaponIdx, int magicbookIdx, int fightPointIdx)
        {
            this.NickName = NickName;
            this.Rank = Rank;
            this.Score = Score;
            this.costumeIdx = costumeIdx;
            this.petIddx = petIddx;
            this.weaponIdx = weaponIdx;
            this.magicbookIdx = magicbookIdx;
            this.fightPointIdx = fightPointIdx;
        }

        public string NickName;
        public int Rank;
        public float Score;
        public int costumeIdx;
        public int petIddx;
        public int weaponIdx;
        public int magicbookIdx;
        public int fightPointIdx;
    }

    public const string Rank_Level_Uuid = "c1d70840-de7f-11eb-bc74-95875190be29";
    public const string Rank_Level_TableName = "Rank_Level";

    public const string Rank_Stage_Uuid = "68d8acb0-de81-11eb-9e66-25cb0ae9020d";
    public const string Rank_Stage = "Rank_Stage";

    public ReactiveCommand<RankInfo> WhenMyLevelRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyStageRankLoadComplete = new ReactiveCommand<RankInfo>();

    public void Subscribe()
    {
        DatabaseManager.statusTable.GetTableData(StatusTable.Level).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e =>
             {
                 UpdateUserRank_Level();
             }).AddTo(this);
    }

    public void GetRankerList_level(string uuid, int count, Backend.BackendCallback callback)
    {
        Backend.URank.User.GetRankList(uuid, count, callback);
    }

    #region LevelRank
    public void RequestMyLevelRank()
    {
        Backend.URank.User.GetMyRank(RankManager.Rank_Level_Uuid, MyLevelRankLoadComplete);
    }

    private void MyLevelRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][DatabaseManager.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = splitData[5];
                int rank = int.Parse(data["rank"][DatabaseManager.format_Number].ToString());
                int level = int.Parse(data["score"][DatabaseManager.format_Number].ToString());
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                myRankInfo = new RankInfo(nickName, rank, level, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        this.myRankInfo[RankType.Level] = myRankInfo;

        WhenMyLevelRankLoadComplete.Execute(myRankInfo);
    }

    public void UpdateUserRank_Level()
    {
        Param param = new Param();
        param.Add("Level", DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value);

        int costumeIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Level_Uuid, Rank_Level_TableName, RankTable_Level.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                // Debug.LogError($"랭킹 등록 성공! UpdateUserRank_Level");
            }
            else
            {
                //  Debug.LogError($"랭킹 등록 실패 UpdateUserRank_Level {bro.GetStatusCode()}");
            }

        });
    }
    #endregion

    #region Stage
    private Action<RankInfo> whenLoadSuccess_Stage;
    public void RequestMyStageRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Stage = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Stage_Uuid, MyStageRankLoadComplete);
    }
    private void MyStageRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][DatabaseManager.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = splitData[5];
                int rank = int.Parse(data["rank"][DatabaseManager.format_Number].ToString());
                float score = float.Parse(data["score"][DatabaseManager.format_Number].ToString());
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                myRankInfo = new RankInfo(nickName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        whenLoadSuccess_Stage?.Invoke(myRankInfo);
        WhenMyStageRankLoadComplete.Execute(myRankInfo);

        this.myRankInfo[RankType.Boss] = myRankInfo;
    }

    public void UpdateStage_Score(float score)
    {
#if UNITY_EDITOR
        //return;
#endif
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Stage_Uuid, Rank_Stage, RankTable_Stage.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! UpdateBoss0_Score");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 UpdateBoss0_Score {bro.GetStatusCode()}");
            }

        });
    }
    #endregion
}
