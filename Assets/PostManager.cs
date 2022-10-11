using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PostManager : SingletonMono<PostManager>
{
    public enum PostType
    {
        Android, IOS
    }

    public class PostInfo
    {
        public string Indate;
        public ObscuredFloat itemCount;
        public ObscuredInt itemType;
        public string titleText;
        public string contentText;
    }

    private List<PostInfo> postList = new List<PostInfo>();
    public List<PostInfo> PostList => postList;

    public ReactiveCommand WhenPostRefreshed = new ReactiveCommand();


    //SDK 문서

    //https://developer.thebackend.io/unity3d/guide/social/GetPostListV2/
    //https://developer.thebackend.io/unity3d/guide/social/postv2/ReceiveAdminPostItemV2/

    public void RefreshPost(bool retry = false)
    {
        bool isAndroid = true;

#if UNITY_IOS
        isAndroid = false;
#endif

        // example
        Backend.Social.Post.GetPostListV2((bro) =>
        {
            if (bro.IsSuccess())
            {
                postList.Clear();

                //우편 받았을때 데이터 파싱
                //
                var rows = bro.GetReturnValuetoJSON();

                if (rows.ContainsKey("fromAdmin"))
                {
                    var fromadmin = rows["fromAdmin"];

                    for (int i = 0; i < fromadmin.Count; i++)
                    {
                        var postInfo = fromadmin[i];

                        //랭킹보상
                        if (postInfo.ContainsKey("rankType"))
                        {
                            PostInfo post = new PostInfo();
                            post.Indate = postInfo["inDate"][ServerData.format_string].ToString();
                            post.itemCount = float.Parse(postInfo["itemCount"][ServerData.format_string].ToString());
                            post.itemType = int.Parse(postInfo["item"][ServerData.format_dic]["ItemType"][ServerData.format_string].ToString());
                            post.titleText = postInfo["title"][ServerData.format_string].ToString();
                            post.contentText = postInfo["content"][ServerData.format_string].ToString();

                            if (post.titleText.Contains("IOS"))
                            {
                                if (isAndroid == false)
                                {
                                    postList.Add(post);
                                }
                            }
                            else
                            {
                                postList.Add(post);
                            }

                        }
                        //일반보상
                        else
                        {
                            PostInfo post = new PostInfo();
                            post.Indate = postInfo["inDate"][ServerData.format_string].ToString();
                            post.itemCount = float.Parse(postInfo["itemCount"][ServerData.format_Number].ToString());
                            post.itemType = int.Parse(postInfo["item"][ServerData.format_dic]["ItemType"][ServerData.format_string].ToString());
                            post.titleText = postInfo["title"][ServerData.format_string].ToString();
                            post.contentText = postInfo["content"][ServerData.format_string].ToString();

                            if (post.titleText.Contains("IOS"))
                            {
                                if (isAndroid == false)
                                {
                                    postList.Add(post);
                                }
                            }
                            else
                            {
                                postList.Add(post);
                            }
                        }
                    }
                }

                WhenPostRefreshed.Execute();
            }
            else
            {
                if (retry)
                {
                    PopupManager.Instance.ShowYesNoPopup("우편 갱신 실패", "재시도 합까요?", () =>
                    {
                        RefreshPost(retry);
                    }, null);
                }
            }
        });
    }

    public void ReceivePost(PostInfo post)
    {

        Backend.Social.Post.ReceiveAdminPostItemV2(post.Indate, (bro) =>
        {
            // 이후 처리
            if (bro.IsSuccess())
            {
                // LogManager.Instance.SendLog("랭킹보상 수령 요청", $"{(Item_Type)((int)post.itemType)}");

                SoundManager.Instance.PlaySound("GoldUse");
                ServerData.GetPostItem((Item_Type)((int)post.itemType), post.itemCount);
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우편을 수령했습니다.", null);

                int findIdx = -1;
                for (int i = 0; i < postList.Count; i++)
                {
                    if (postList[i].Indate.Equals(post.Indate))
                    {
                        findIdx = i;
                        break;
                    }
                }

                if (findIdx != -1)
                {
                    postList.RemoveAt(findIdx);
                    WhenPostRefreshed.Execute();
                }

                //RefreshPost(true);
            }
            else
            {
                PopupManager.Instance.ShowYesNoPopup("수령 실패", "우편을 받지 못했습니다.\n다시 시도할까요?", () =>
                {
                    ReceivePost(post);
                }, null);
            }
        });

    }
}
