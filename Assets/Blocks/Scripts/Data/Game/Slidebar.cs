using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;
public class Slidebar : MonoBehaviour
{
    public GameObject CebianPanel;//侧边栏界面
    public  Button CloseBtn;//关闭按钮
    public Button CebianBtn;//侧边栏按钮

    private void Start()
    {
        TT.InitSDK((code, env) =>
        {
            Debug.Log("Unity message init sdk callback");
            Debug.Log("Unity message code: " + code);
            Debug.Log("Unity message HostEnum: " + env.m_HostEnum);
        });
        // 绑定按钮点击事件
            CebianBtn.onClick.AddListener(ToSidebar); 
            CloseBtn.onClick.AddListener(CloseCebianPanel);
       
    }
  
    public void CloseCebianPanel()
    {
        CebianPanel.SetActive(false);
    }

    /// <summary>
    /// 侧边栏跳转
    /// </summary>
    public void ToSidebar()
    {

        NavigateToDouyinSidebar();
    }

    /// <summary>
    /// 通用抖音侧边栏按钮跳转功能
    /// </summary>
    /// <param name="activityId">活动ID（可选）</param>
    /// <param name="extraParams">额外参数（可选）</param>
    public void NavigateToDouyinSidebar(string activityId = "", Dictionary<string, string> extraParams = null)
    {
        var data = new JsonData
        {
            ["scene"] = "sidebar"
        };

        // 如果提供了activityId，则添加到参数中
        if (!string.IsNullOrEmpty(activityId))
        {
            data["activityId"] = activityId;
        }

        // 如果提供了额外参数，则添加到参数中
        if (extraParams != null)
        {
            foreach (var param in extraParams)
            {
                data[param.Key] = param.Value;
            }
        }

        TT.NavigateToScene(data, () =>
        {
            Debug.Log("抖音侧边栏跳转请求成功");
        }, () =>
        {
            Debug.Log("抖音侧边栏跳转完成");
        }, (errCode, errMsg) =>
        {
            Debug.LogError($"抖音侧边栏跳转失败，错误码：{errCode}，错误信息：{errMsg}");
        });
    }
}
