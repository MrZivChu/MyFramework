using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    //List<HotFile> updateFiles1 = new List<HotFile>() {
    //    new HotFile() { size = 103322224, url = "http://www.hotupdate.com/ab/1.zip" },
    //    new HotFile() { size = 19071619, url = "http://www.hotupdate.com/ab/2.zip"},
    //    new HotFile() { size = 20777879, url = "http://www.hotupdate.com/ab/3.zip" },
    //    new HotFile() { size = 23822, url = "http://www.hotupdate.com/ab/4.zip"},
    //    new HotFile() { size = 84323, url = "http://www.hotupdate.com/ab/5.zip"},
    //    new HotFile() { size = 110628, url = "http://www.hotupdate.com/ab/6.zip"},
    //    new HotFile() { size = 1486058, url = "http://www.hotupdate.com/ab/7.zip"},
    //    new HotFile() { size = 294906, url =  "http://www.hotupdate.com/ab/8.zip"},
    //    new HotFile() { size = 126255, url =  "http://www.hotupdate.com/ab/9.zip"},
    //    new HotFile() { size = 49140, url = "http://www.hotupdate.com/ab/10.zip"},
    //    new HotFile() { size = 8381791, url = "http://www.hotupdate.com/ab/11.zip"},
    //    new HotFile() { size = 44287, url = "http://www.hotupdate.com/ab/12.zip"},
    //    new HotFile() { size = 339696, url = "http://www.hotupdate.com/ab/13.zip"},
    //    new HotFile() { size = 339696, url = "http://www.hotupdate.com/ab/14.zip"},
    //};
    //double allSize = 154452324;
    //DownloadFiles downloadFiles;
    //void Start() {
    //    //可以实例化带有脚本DownloadFiles的对象，来达到多线程处理多个下载任务
    //    print("开始时间" + DateTime.Now.ToLocalTime());
    //    downloadFiles = GetComponent<DownloadFiles>();
    //    downloadFiles.Download(updateFiles1);
    //}

    //double currentSize = 0;
    //void OnGUI() {
    //    GUIStyle fontStyle = new GUIStyle();
    //    fontStyle.normal.background = null;    //设置背景填充  
    //    fontStyle.normal.textColor = new Color(1, 0, 0);   //设置字体颜色  
    //    fontStyle.fontSize = 40;       //字体大小  

    //    currentSize = downloadFiles.currentDownloadSize;
    //    GUI.Label(new Rect(0, 0, 200, 200), (currentSize + " = " + allSize).ToString(), fontStyle);
    //    GUI.Label(new Rect(0, 50, 200, 200), (currentSize / allSize).ToString("0.##"), fontStyle);
    //    string num = ((currentSize / allSize) * 100).ToString("0");
    //    GUI.Label(new Rect(0, 100, 200, 200), (num == "100" ? (currentSize == allSize ? "100" : "99") : num) + "%", fontStyle);
    //    if (currentSize == allSize) {
    //        print("结束时间" + DateTime.Now.ToLocalTime());
    //    }
    //}


    string hotUpdateUrl = string.Empty; //热更地址

    HotUpdateHelper hotUpdateHelper = null;


    void Start()
    {
        downloadTipText.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        hotUpdateHelper = GameObject.Find("HotUpdateHelper").GetComponent<HotUpdateHelper>();
        RequestNet();
    }

    /// <summary>
    /// 第一次请求服务器
    /// </summary>
    void RequestNet()
    {
        if (Utils.NetIsAvailable == false)
        {
            MessageBox.Instance.PopOK(StaticText.STR_NO_NET, () =>
            {
                RequestNet();
            }, StaticText.STR_RETRY);
        }
        else
        {
            string fields = "name&Tom&age&18";
            Utils.PostHttp(AppConfig.ServerURL + "Login.ashx", fields, onRequestSuccess, onRequestFailed);
        }
    }

    void onRequestSuccess(string message)
    {
        try
        {
            JsonData res = JsonMapper.ToObject(message);
            string result = (string)res["result"];
            if (result == "success")
            {
                JsonData note = res["data"];
                int encrypted = Convert.ToInt16(res["encrypted"].ToString());
                if (encrypted > 0)
                {//是加密数据 
                    note = JsonMapper.ToObject(Utils.Decrypt((string)note));
                }
                string version = (string)note["version"];
                if (version == "needReplace")
                { //需要强更换包
                    string replaceAppUrl = (string)note["replaceAppUrl"];
                    MessageBox.Instance.PopOK(StaticText.ChangeApp, () =>
                    {
                        Application.OpenURL(replaceAppUrl);
                    }, StaticText.GoDownloadApp);
                }
                else
                {
                    hotUpdateUrl = ((string)note["hotUpdateUrl"]).Replace("com", "xyz");
                    CallHotUpdateHelper();
                }
            }
            else
            {
                //这里是由服务器返回请求失败的原因，例如服务器正在维护，在某某时间段才开服
                MessageBox.Instance.PopOK(res["error"].ToString(), null, StaticText.Ok);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Instance.PopOK(StaticText.Data_Error + ex.Message, () =>
            {
                Application.Quit();
            }, StaticText.QuitGame);
        }
    }

    void CallHotUpdateHelper()
    {
        hotUpdateHelper.DownloadFileListError += () =>
        {
            MessageBox.Instance.PopYesNo(StaticText.DownloadFileListError, () =>
            {
                Application.Quit();
            }, () =>
            {
                hotUpdateHelper.Retry();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        };
        hotUpdateHelper.DownloadAssetsError += () =>
        {
            MessageBox.Instance.PopYesNo(StaticText.DownloadAssetsError, () =>
            {
                Application.Quit();
            }, () =>
            {
                hotUpdateHelper.Retry();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        };
        hotUpdateHelper.ConfirmDownloadAssets += () =>
        {
            MessageBox.Instance.PopYesNo(string.Format(StaticText.ConfirmDownloadAssets, GetShortSize(hotUpdateHelper.NeedUpdateSize)), () =>
            {
                Application.Quit();
            }, () =>
            {
                hotUpdateHelper.StartDownloadAssets();
                downloadTipText.gameObject.SetActive(true);
                progress.gameObject.SetActive(true);
            }, StaticText.QuitGame, StaticText.StartDownloadAssets);
        };
        hotUpdateHelper.AllDownloadSuccess += () =>
        {
            MessageBox.Instance.PopOK(StaticText.Welcome, null, StaticText.StartGame);
        };
        hotUpdateHelper.StartUpdate(hotUpdateUrl);
    }


    void onRequestFailed(string message)
    {
        //这里的错误消息主要是因为网络原因造成，是由自己根据网络错误类型定义的
        MessageBox.Instance.PopOK(StaticText.STR_SERVER_FAILED + message, () =>
        {
            RequestNet();
        }, StaticText.STR_RETRY);
    }

    public Text tipText;
    public Text downloadTipText;
    public Text progress;
    public Slider slider;
    void Update()
    {
        if (hotUpdateHelper.NeedUpdateSize > 0)
        {
            downloadTipText.text = string.Format(StaticText.DownloadShowText, GetShortSize(hotUpdateHelper.NeedUpdateSize), GetShortSize((int)hotUpdateHelper.DownloadSizePerSecond), AppConfig.APP_VERSION);
            float value = ((float)hotUpdateHelper.HasDownloadSize / hotUpdateHelper.NeedUpdateSize);
            progress.text = string.Format("{0:#.##}%", value * 100);
            slider.value = value;
        }
    }

    string GetShortSize(int size)
    {
        if (size < 1024)
        {
            return string.Format("{0:#.##}B", size);
        }
        else if (size < 1048576)
        {
            return string.Format("{0:#.##}KB", size / 1024f);
        }
        else
        {
            return string.Format("{0:#.##}MB", size / 1048576f);
        }
    }

}
