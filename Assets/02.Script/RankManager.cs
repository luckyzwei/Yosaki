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
    Level, Stage, Boss, Real_Boss, None
}

public class RankManager : SingletonMono<RankManager>
{
    private Dictionary<RankType, RankInfo> myRankInfo = new Dictionary<RankType, RankInfo>()
    {
        //채팅 표기 우선순위
        { RankType.Real_Boss,null },
        { RankType.Stage,null },
        { RankType.Level,null },
        { RankType.Boss,null }
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

    public const string Rank_Boss_Uuid = "87bf23b0-070e-11ec-85c3-37d5dee7a389";
    public const string Rank_Boss = "Rank_Boss_1";

    //public const string Rank_Real_Boss_Uuid = "1438d260-fec6-11eb-b9fc-c9829b653541";
    public const string Rank_Real_Boss_Uuid = "9baf8510-0b85-11ec-8132-edeaa3358991";
    public const string Rank_Real_Boss = "Rank_Boss_2";

    public ReactiveCommand<RankInfo> WhenMyLevelRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyStageRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyBossRankLoadComplete = new ReactiveCommand<RankInfo>();
    public ReactiveCommand<RankInfo> WhenMyRealBossRankLoadComplete = new ReactiveCommand<RankInfo>();

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

                myRankInfo = new RankInfo(nickName, rank, level, costumeId, petId, weaponId, magicBookId, fightPoint);
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

                var splitData = data["NickName"][ServerData.format_string].ToString().Split(CommonString.ChatSplitChar);

                string nickName = data["nickname"][ServerData.format_string].ToString();
                int rank = int.Parse(data["rank"][ServerData.format_Number].ToString());
                float score = float.Parse(data["score"][ServerData.format_Number].ToString());
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
            whenLoadSuccess_Stage?.Invoke(myRankInfo);
            WhenMyStageRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Stage] = myRankInfo;
        }
    }

    public void UpdateStage_Score(float score)
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
                float score = float.Parse(data["score"][ServerData.format_Number].ToString());
                score *= 1000f;
                int costumeId = int.Parse(splitData[0]);
                int petId = int.Parse(splitData[1]);
                int weaponId = int.Parse(splitData[2]);
                int magicBookId = int.Parse(splitData[3]);
                int fightPoint = int.Parse(splitData[4]);

                myRankInfo = new RankInfo(nickName, rank, score, costumeId, petId, weaponId, magicBookId, fightPoint);
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

    public void UpdateBoss_Score(float score)
    {
        if (UpdateRank() == false) return;

        if (this.myRankInfo[RankType.Boss] != null && score < this.myRankInfo[RankType.Boss].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= 0.001f;

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

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
                float score = float.Parse(data["score"][ServerData.format_Number].ToString());
                score *= 1000f;
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
            whenLoadSuccess_Real_Boss?.Invoke(myRankInfo);
            WhenMyRealBossRankLoadComplete.Execute(myRankInfo);

            this.myRankInfo[RankType.Real_Boss] = myRankInfo;
        }
    }

    public void UpdateRealBoss_Score(float score)
    {
        if (UpdateRank() == false) return;
        if (this.myRankInfo[RankType.Real_Boss] != null && score < this.myRankInfo[RankType.Real_Boss].Score)
        {
            Debug.LogError("점수가 더 낮음");
            return;
        }

        score *= 0.001f;

        Param param = new Param();
        param.Add("Score", score);

        int costumeIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        int fightPoint = (int)PlayerStats.GetTotalPower();
        int wingIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).Value;

        param.Add("NickName", $"{costumeIdx}{CommonString.ChatSplitChar}{petIdx}{CommonString.ChatSplitChar}{weaponIdx}{CommonString.ChatSplitChar}{magicBookIdx}{CommonString.ChatSplitChar}{fightPoint}{CommonString.ChatSplitChar}{PlayerData.Instance.NickName}{CommonString.ChatSplitChar}{wingIdx}");

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

    private bool UpdateRank()
    {
        return !PlayerData.Instance.NickName.Equals("블랙핑크") && !PlayerData.Instance.NickName.Equals("블랙핑크")
            && !PlayerData.Instance.NickName.Equals("테스트용");
    }
}
