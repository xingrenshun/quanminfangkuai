using BBG;
using System;
using System.Collections;
using System.Collections.Generic;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;
using UnityEngine;
using static TTSDK.TTAndroidCallJsInterface;


public class GuangGao : Singleton<GuangGao>
{
    private static string XPId = "7k59bjl1hg9kf1e08f";
    private static string BannerId = "3ijaj0cb780o32x3np";
    private static string rewardId = "20gi8fgaghdbi3o74m";

    TTInterstitialAd m_InterAdIns = null;
    TTBannerAd m_bannerAdIns = null;
    TTRewardedVideoAd m_rewardAdIns = null;
    Action rewardAction;
    private bool rewardLoading;        // 防止 Load 后还没 Show 又被点
    private float gameStartTime; // 游戏启动时间
    private float showXPTime;//插屏广告显示时间
    private float showRewardTime;//激励广告显示时间
    private void Awake()
    {
        base.Awake();
        gameStartTime = Time.time; // 记录游戏启动时间
        TT.InitSDK();
        CreateXP();
        CreatBanner();
        CreateReward();
    }

    /// <summary>
    /// 检查游戏启动是否已经超过30秒
    /// </summary>
    /// <returns>如果游戏启动时间超过30秒返回true，否则返回false</returns>
    public bool IsGameStartedOver30Seconds()
    {
        return Time.time - gameStartTime >= 30f;
    }
    public bool IsShowXPStartedOver60Seconds()
    {
        if(showXPTime == 0) return true;
        return Time.time - showXPTime >= 60f;
    }
    public bool IsShowRewardStartedOver60Seconds()
    {
        if (showRewardTime == 0) return true;
        return Time.time - showRewardTime >= 60;
    }
    public void CreateXP()
    {
        var param = new CreateInterstitialAdParam { InterstitialAdId = XPId };
        m_InterAdIns = TT.CreateInterstitialAd(param);
        m_InterAdIns.OnClose += () => Debug.Log("插屏广告关闭");
        m_InterAdIns.OnLoad += () => Debug.Log("插屏广告加载");
        m_InterAdIns.OnError += (code, message) => Debug.Log($"插屏 错 {code}  {message}");
    }

    public void ShowXP()
    {
        if (IsGameStartedOver30Seconds())
        {
            if (IsShowRewardStartedOver60Seconds())
            {
                if (IsShowXPStartedOver60Seconds())
                {
                    m_InterAdIns.Show();
                    showXPTime = Time.time;
                }
                else
                {
                    Debug.Log("离打开插屏广告没到60s");
                }
            }
            else
            {
                Debug.Log("离打开激励广告没到60s");
            }
        }
        else
        {
            Debug.Log("30s没到");
        }
    }

    public void CreatBanner()
    {
        var param = new CreateBannerAdParam { BannerAdId = BannerId };
        m_bannerAdIns = TT.CreateBannerAd(param);
    }
    public void ShowBanner()
    {
        m_bannerAdIns.Show();
    }
    public void CloseBanner()
    {
        m_bannerAdIns.Hide();
    }
    public void CreateReward()
    {
        var param = new CreateRewardedVideoAdParam { AdUnitId = rewardId };
        m_rewardAdIns = TT.CreateRewardedVideoAd(param);

        // 加载完成：拉取到广告素材后才能 Show
        m_rewardAdIns.OnLoad += () =>
        {
            Debug.Log("[RewardAd] loaded");
            // 等待中的 Show 请求在此处真正执行
            if (rewardLoading)
            {
                rewardLoading = false;
                m_rewardAdIns.Show();
            }
        };

        // 广告关闭：isEnded=true 表示完整观看了视频，可以发放奖励
        m_rewardAdIns.OnClose += (isEnded, count) =>
        {
            Debug.Log($"[RewardAd] close isEnded={isEnded} count={count}");
            if (isEnded)
            {
                rewardAction?.Invoke();
            }
            rewardAction = null;
        };

        // 加载/展示过程中出错：仅记录日志，绝不要在这里再 Show，否则会反复触发
        m_rewardAdIns.OnError += (code, msg) =>
        {
            Debug.LogError($"[RewardAd] error code={code} msg={msg}");
            rewardLoading = false;
            rewardAction = null;
        };
    }

    /// <summary>
    /// 异步展示激励视频：先 Load，OnLoad 回调里再 Show
    /// </summary>
    public void ShowReward(Action action)
    {
        if (m_rewardAdIns == null)
        {
            Debug.LogError("[RewardAd] not created");
            action?.Invoke(); // 兜底：让流程不至于卡死
            return;
        }

        if (rewardLoading)
        {
            Debug.Log("[RewardAd] already loading, skip click");
            return;
        }

        rewardAction = action;
        showRewardTime = Time.time;
        rewardLoading = true;

        m_rewardAdIns.Load();
    }

    public void WatchAddIcons()
    {
        ShowReward(() =>
        {
            CurrencyManager.Instance.Give("coins", 50);
        });
    }
    /// <summary>
    /// 上传分数
    /// </summary>
    /// <param name="score"></param>
    /// <param name="zoneId"></param>
    public void UploadScore(int score, string zoneId = "default")
    {
        try
        {
            JsonData data = new JsonData()
            {
                ["dataType"] = 0,
                ["value"] = score.ToString(),
                ["priority"] = 0,
                ["zoneId"] = zoneId
            };
            CheckSession(true, () =>
            {
                TT.SetImRankData(data, (b, a) =>
                {
                    if (b)
                    {
                        Debug.Log("上传数据");
                    }
                    else
                    {
                        Debug.Log("上传数据");
                    }
                });
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"上传分数时发生错误：{e.Message}");
        }
    }
    /// <summary>
    /// 登录
    /// </summary>
    public void CheckSession(Boolean callShow = false, Action callback = null)
    {
        TT.CheckSession(() =>
        {
            if (callShow)
            {
                callback?.Invoke();
            }
        }, (err) =>
        {
            TT.Login((code, anonymousCode, isLogin) =>
            {
                if (callShow)
                {
                    callback?.Invoke();
                }
            },
        (msg) => { Debug.Log($"TestLogin: {msg}"); }, true);
        });
    }
}