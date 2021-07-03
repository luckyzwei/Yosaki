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

    public const string Rank_Level_Uuid = "17624b40-96ef-11eb-82e9-ed26b867d4c8";
    public const string Rank_Level_TableName = "Rank_Level";

    public const string Rank_Boss_0_Uuid = "076d54b0-af67-11eb-89ee-792d48360867";
    public const string Rank_Boss_0_TableName = "Rank_Boss_0";

    public const string Rank_Boss_1_Uuid = "8bca2780-cddd-11eb-afd6-dbb065845072";
    public const string Rank_Boss_1_TableName = "Rank_Boss_1";

    public const string Rank_Infinity_Uuid = "c97b5160-b2e1-11eb-8e06-7129a9ed3736";
    public const string Rank_Infinity_TableName = "Rank_Infinity";

    public ReactiveCommand<RankInfo> WhenMyLevelRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyBoss0RankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyBoss1RankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenInfinityRankLoadComplete = new ReactiveCommand<RankInfo>();

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
#if UNITY_EDITOR
        return;
#endif

        Param param = new Param();
        param.Add("Level", DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value);

        int costumeIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingGrade).Value;

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

    #region bossRank0
    private ObscuredFloat prefBoss0Score = 0f;
    private Action<RankInfo> whenLoadSuccess_Boss0;
    public void RequestMyBoss0Rank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Boss0 = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Boss_0_Uuid, MyBoss0RankLoadComplete);
    }
    private void MyBoss0RankLoadComplete(BackendReturnObject bro)
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

        if (myRankInfo != null)
        {
            prefBoss0Score = myRankInfo.Score;
        }

        whenLoadSuccess_Boss0?.Invoke(myRankInfo);
        WhenMyBoss0RankLoadComplete.Execute(myRankInfo);

        this.myRankInfo[RankType.Boss] = myRankInfo;
    }

    public void UpdateBoss0_Score(float score)
    {
#if UNITY_EDITOR
        //return;
#endif

        //이전점수랑 비교
        if (score < prefBoss0Score)
        {
            return;
        }

        prefBoss0Score = score;
        //

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingGrade).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Boss_0_Uuid, Rank_Boss_0_TableName, RankTable_Boss0.Indate, param, bro =>
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

    #region bossRank1
    private ObscuredFloat prefBoss1Score = 0f;
    private Action<RankInfo> whenLoadSuccess_Boss1;
    public void RequestMyBoss1Rank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Boss1 = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Boss_1_Uuid, MyBoss1RankLoadComplete);
    }
    private void MyBoss1RankLoadComplete(BackendReturnObject bro)
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

        if (myRankInfo != null)
        {
            prefBoss1Score = myRankInfo.Score;
        }

        whenLoadSuccess_Boss1?.Invoke(myRankInfo);
        WhenMyBoss1RankLoadComplete.Execute(myRankInfo);

        this.myRankInfo[RankType.Boss1] = myRankInfo;
    }

    public void UpdateBoss1_Score(float score)
    {

        //이전점수랑 비교
        if (score < prefBoss1Score)
        {
            return;
        }

        prefBoss1Score = score;
        //

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingGrade).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Boss_1_Uuid, Rank_Boss_1_TableName, RankTable_Boss1.Indate, param, bro =>
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

    #region InfinityTower
    private Action<RankInfo> whenLoadSuccess_InfinityTower;
    public void RequestInfinityTowerRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_InfinityTower = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Infinity_Uuid, MyInfinityTowerRankLoadComplete);
    }
    private void MyInfinityTowerRankLoadComplete(BackendReturnObject bro)
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

        whenLoadSuccess_InfinityTower?.Invoke(myRankInfo);
        WhenInfinityRankLoadComplete.Execute(myRankInfo);
        this.myRankInfo[RankType.Infinity] = myRankInfo;
    }

    public void UpdatInfinityTower_Score(float floor)
    {
#if UNITY_EDITOR
        return;
#endif

        Param param = new Param();
        param.Add("Floor", floor);

        int costumeIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)DatabaseManager.userInfoTable.GetTableData(UserInfoTable.wingGrade).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Infinity_Uuid, RankTable_InfinityTower.tableName_InfinityTower, RankTable_InfinityTower.Indate, param, bro =>
        {
            // 이후처리
            if (bro.IsSuccess())
            {
                Debug.LogError($"랭킹 등록 성공! InfinityTower");
            }
            else
            {
                Debug.LogError($"랭킹 등록 실패 InfinityTower {bro.GetStatusCode()}");
            }

        });
    }
    #endregion



}
