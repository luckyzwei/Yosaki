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
    Level, Stage, Boss, Real_Boss, Relic, MiniGame, None
}

public class RankManager : SingletonMono<RankManager>
{
    private Dictionary<RankType, RankInfo> myRankInfo = new Dictionary<RankType, RankInfo>()
    {
        //채팅 표기 우선순위
        { RankType.Real_Boss,null },
        { RankType.Stage,null },
        { RankType.Level,null },
        { RankType.Boss,null },
        { RankType.Relic,null },
        { RankType.MiniGame,null }
    };

    public Dictionary<RankType, RankInfo> MyRankInfo => myRankInfo;

    public class RankInfo
    {
        public RankInfo(string NickName, string GuildName, int Rank, double Score, int costumeIdx, int petIddx, int weaponIdx, int magicbookIdx, int fightPointIdx)
        {
#if UNITY_ANDROID
            this.NickName = NickName;
#endif

#if UNITY_IOS
            this.NickName = NickName.Replace(CommonString.IOS_nick, "");
#endif
            this.Rank = Rank;
            this.Score = Score;
            this.costumeIdx = costumeIdx;
            this.petIddx = petIddx;
            this.weaponIdx = weaponIdx;
            this.magicbookIdx = magicbookIdx;
            this.fightPointIdx = fightPointIdx;
            this.GuildName = GuildName;
        }

        public string NickName;
        public string GuildName;
        public int Rank;
        public double Score;
        public int costumeIdx;
        public int petIddx;
        public int weaponIdx;
        public int magicbookIdx;
        public int fightPointIdx;
    }

#if UNITY_ANDROID
    public const string Rank_Level_Uuid = "c1d70840-de7f-11eb-bc74-95875190be29";
    public const string Rank_Stage_Uuid = "68d8acb0-de81-11eb-9e66-25cb0ae9020d";
    public const string Rank_Boss_Uuid = "fb8e7d90-7b12-11ec-ac2b-23138d1b520c";
    public const string Rank_Real_Boss_Uuid = "12859680-7e9e-11ec-9993-b3682868da4d";
    public const string Rank_Relic_Uuid = "0453f560-2779-11ec-9b46-299116fee741";
    public const string Rank_MiniGame_Uuid = "e1dced80-5954-11ec-b084-d1a61d5ec8e2";
    public const string Rank_Guild_Uuid = "ff017920-698b-11ec-b243-8d1fccc57e3d";
    public const string Rank_Guild_Reset_Uuid = "be50a6d0-698b-11ec-b243-8d1fccc57e3d";

    public const string Rank_Level_TableName = "Rank_Level";
    public const string Rank_Stage = "Rank_Stage";
    public const string Rank_Boss = "Last_Cat";
    public const string Rank_Real_Boss = "Rank_Boss_12";
    public const string Rank_Relic = "Rank_Relic";
    public const string Rank_MiniGame = "Rank_MiniGame";
#endif

#if UNITY_IOS
    public const string Rank_Level_Uuid = "ea8c9430-38cf-11ec-955f-fb3c68e97f2a";
    public const string Rank_Stage_Uuid = "f865e900-31b6-11ec-b4ab-713be46ddb60";
    public const string Rank_Boss_Uuid = "0519e160-7b13-11ec-8ee8-57ab7b5923a7";
    public const string Rank_Real_Boss_Uuid = "2cb143b0-7e9e-11ec-9b0e-4b4052560415";
    public const string Rank_Relic_Uuid = "1ce07110-31b7-11ec-be95-537d9b90903a";
    public const string Rank_MiniGame_Uuid = "edec1600-5954-11ec-8846-eb56e6e68bb1";
    public const string Rank_Guild_Uuid = "10619010-698c-11ec-b243-8d1fccc57e3d";
    public const string Rank_Guild_Reset_Uuid = "d248cdc0-698b-11ec-b243-8d1fccc57e3d";

    public const string Rank_Level_TableName = "Level_Rank_IOS";
    public const string Rank_Stage = "Rank_Stage_IOS";
    public const string Rank_Boss = "Last_Cat_IOS";
    public const string Rank_Real_Boss = "Rank_Boss_12_IOS";
    public const string Rank_Relic = "Rank_Relic_IOS";
    public const string Rank_MiniGame = "Rank_MiniGame_IOS";
#endif



    public ReactiveCommand<RankInfo> WhenMyLevelRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyStageRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyBossRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyRealBossRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyRelicRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyMiniGameRankLoadComplete = new ReactiveCommand<RankInfo>();

    public void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e =>
             {
                 UpdateUserRank_Level();
             }).AddTo(this);
    }

    public void GetRankerList(string uuid, int count, Backend.BackendCallback callback)
    {
        UiRankView.rank1Count = 0;
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

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                int level = int.Parse(data["score"][ServerData.format_Number].ToString());
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, level, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        if (myRankInfo != null)
        {
            this.myRankInfo[RankType.Level] = myRankInfo;

            WhenMyLevelRankLoadComplete.Execute(myRankInfo);
        }
    }

    public void UpdateUserRank_Level()
    {
        if (UpdateRank() == false) return;

        Param param = new Param();
        param.Add("Level", ServerData.statusTable.GetTableData(StatusTable.Level).Value);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}");

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

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Stage?.Invoke(myRankInfo);
            WhenMyStageRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Stage] = myRankInfo;
        }
    }

    public void UpdateStage_Score(double score)
    {
        if (UpdateRank() == false) return;
        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}");

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


    #region Boss
    private Action<RankInfo> whenLoadSuccess_Boss;
    public void RequestMyBossRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Boss = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Boss_Uuid, MyBossRankLoadComplete);
    }
    private void MyBossRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                score *= GameBalance.BossScoreConvertToOrigin;
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        //소탕팝업뜸이래야
        whenLoadSuccess_Boss?.Invoke(myRankInfo);

        if (myRankInfo != null)
        {
            WhenMyBossRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Boss] = myRankInfo;
        }
    }

    public void UpdateBoss_Score(double score)
    {
        //if (UpdateRank() == false) return;

        if (this.myRankInfo[RankType.Boss] != null && score < this.myRankInfo[RankType.Boss].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= GameBalance.BossScoreSmallizeValue;

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Boss_Uuid, Rank_Boss, RankTable_Boss.Indate, param, bro =>
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

    #region RealBoss
    private Action<RankInfo> whenLoadSuccess_Real_Boss;
    public void RequestMyRealBossRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Real_Boss = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Real_Boss_Uuid, MyRealBossRankLoadComplete);
    }
    private void MyRealBossRankLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                score *= GameBalance.BossScoreConvertToOrigin;
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }


                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Real_Boss?.Invoke(myRankInfo);
            WhenMyRealBossRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Real_Boss] = myRankInfo;
        }
    }

    public void UpdateRealBoss_Score(double score)
    {
        //if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.Real_Boss] != null && score < this.myRankInfo[RankType.Real_Boss].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= GameBalance.BossScoreSmallizeValue;

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Real_Boss_Uuid, Rank_Real_Boss, RankTable_Real_Boss.Indate, param, bro =>
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

    #region Relic
    private Action<RankInfo> whenLoadSuccess_Relic;
    public void RequestMyRelicRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_Relic = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_Relic_Uuid, MyRelicLoadComplete);
    }
    private void MyRelicLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
            }
        }

        if (myRankInfo != null)
        {
            whenLoadSuccess_Relic?.Invoke(myRankInfo);
            WhenMyRelicRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Relic] = myRankInfo;
        }
    }

    public void UpdateRelic_Score(double score)
    {
        if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.Relic] != null && score < this.myRankInfo[RankType.Relic].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_Relic_Uuid, Rank_Relic, RankTable_YoguiSogul.Indate, param, bro =>
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

    #region MiniGame
    private Action<RankInfo> whenLoadSuccess_MiniGame;
    public void RequestMyMiniGameRank(Action<RankInfo> whenLoadSuccess = null)
    {
        this.whenLoadSuccess_MiniGame = whenLoadSuccess;

        Backend.URank.User.GetMyRank(RankManager.Rank_MiniGame_Uuid, MyMiniGameLoadComplete);
    }

    public void ResetMiniGameScore()
    {
        SendQueue.Enqueue(Backend.URank.User.GetMyRank, RankManager.Rank_MiniGame_Uuid, MyMiniGameLoadComplete);
    }

    private void MyMiniGameLoadComplete(BackendReturnObject bro)
    {
        RankInfo myRankInfo = null;

        if (bro.IsSuccess())
        {
            var rows = bro.Rows();

            if (rows.Count > 0)
            {
                JsonData data = rows[0];

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                double score = double.Parse(data["score"][ServerData.format_Number].ToString());
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                string guildName = string.Empty;
                if (splitData.Length >= 8)
                {
                    guildName = splitData[7];
                }

                myRankInfo = new RankInfo(nickName, guildName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
            }

            if (myRankInfo != null)
            {
                whenLoadSuccess_MiniGame?.Invoke(myRankInfo);
                WhenMyMiniGameRankLoadComplete.Execute(myRankInfo);

                this.myRankInfo[RankType.MiniGame] = myRankInfo;
            }

        }
        else
        {
            if (bro.GetStatusCode().Equals("404"))
            {
                if (this.myRankInfo[RankType.MiniGame] != null)
                {
                    this.myRankInfo[RankType.MiniGame].Score = 0f;
                }
            }
        }


    }

    public void UpdateMiniGame_Score(double score)
    {
        if (this.myRankInfo[RankType.MiniGame] != null && score < this.myRankInfo[RankType.MiniGame].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}{CommonString.ChatSplitChar}{GuildManager.Instance.myGuildName}");

        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, Rank_MiniGame_Uuid, Rank_MiniGame, RankTable_MiniGame.Indate, param, bro =>
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

    private bool UpdateRank()
    {
        return !PlayerData.Instance.NickName.Equals("로꼬")
         && !PlayerData.Instance.NickName.Equals("테스트용2")
         && !PlayerData.Instance.NickName.Equals("블랙핑크")
         && !PlayerData.Instance.NickName.Equals("BiBi")
            && !PlayerData.Instance.NickName.Equals("테스트용");
    }
}
