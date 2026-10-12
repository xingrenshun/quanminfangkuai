using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
/// <summary>
/// 抖音排行榜功能实现类
/// 提供简单的分数上传和排行榜显示功能
/// </summary>
public class PaiHangBang : MonoBehaviour
{
    // 排行榜面板
    public GameObject rankPanel;
    // 分享按钮
    public Button btn_Share;

    // 测试用的分数
    private int testScore = 100;
    private void Start()
    {

        if (btn_Share != null)
            btn_Share.onClick.AddListener(ShareGame);
    }

    /// <summary>
    /// 打开排行榜面板
    /// </summary>
    public void OpenRankPanel()
    {
        if (rankPanel != null)
            rankPanel.SetActive(true);

        // 显示排行榜数据
        ShowRankList();
    }

    /// <summary>
    /// 显示排行榜数据
    /// </summary>
    /// <param name="zoneId">排行榜分区，默认使用"default"</param>
    /// <param name="rankType">排行榜类型，1表示全局榜，2表示好友榜，默认全局榜</param>
    public void ShowRankList(string zoneId = "default", int rankType = 1)
    {
        try
        {
            GuangGao.Instance.CheckSession(true, () =>
            {
                // 构建排行榜请求参数
                JsonData param = new JsonData()
                {
                    ["rankType"] = "all", // 时间范围：day、week、month、all
                    ["dataType"] = 0, // 0：数字类型 、 1：枚举类型
                    ["relationType"] = rankType == 1 ? "all" : "friend", // friend：好友榜，all: 总榜
                    ["pageNum"] = 1, // 页码，从1开始
                    ["pageSize"] = 20, // 每页长度，大于0小于40
                    ["zoneId"] = zoneId // 排行榜分区标识
                };
                TT.GetImRankList(param, (b, s) =>
                {
                    if (b)
                    {
                        Debug.Log("GetImRankList");
                    }
                    else
                    {
                        Debug.Log("GetImRankList 2");
                    }
                });
            });

        }
        catch (Exception e)
        {
            Debug.LogError($"获取排行榜时发生错误：{e.Message}");
        }
    }


    /// <summary>
    /// 分享功能实现
    /// </summary>
    public void ShareGame()
    {
        try
        {
            // 通用分享
            JsonData shareJson = new JsonData();
            shareJson["channel"] = "video";
            shareJson["title"] = "Some Title";
            shareJson["extra"] = new JsonData();
            shareJson["extra"]["videoPath"] = "/xxx/xxx.mp4";//录屏分享的话，路径是 OnRecordComplete 拿到的路径
            JsonData videoTopics = new JsonData();
            videoTopics.SetJsonType(JsonType.Array);
            videoTopics.Add("Some Topic1");
            videoTopics.Add("Some Topic2");
            shareJson["extra"]["videoTopics"] = videoTopics;
            shareJson["extra"]["hashtag_list"] = videoTopics;

            Debug.Log($"ShareAppMessageBtnClicked jsonData: {shareJson.ToJson()}");

            // 调用抖音分享API
            TT.ShareAppMessage(shareJson);
        }
        catch (Exception e)
        {
            Debug.LogError("分享游戏时发生错误：" + e.Message);
        }
    }



}
