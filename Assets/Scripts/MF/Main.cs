﻿using LitJson;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CheckStatus
{
    Default = 0,
    CheckVersioning = 1,//检查版本更新
    CheckVersionOver = 1,//检查版本更新完毕
    CheckAssetsing = 2,//检查资源更新
    CheckAssetsOver = 3,//检查资源更新结束完毕
    DownloadAssetsing = 4,//下载资源
    StartGame = 5,//开始游戏
}

public class Main : MonoBehaviour
{
    string hotUpdateUrl = string.Empty; //热更地址
    HotUpdateHelper hotUpdateHelper = null;
    CheckStatus currentCheckStatus = CheckStatus.Default;

    public Text tipText;
    public Text downloadTipText;
    public Text progress;
    public Slider slider;
    public Button startBtn;

    void Start()
    {
        downloadTipText.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        tipText.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(false);
        hotUpdateHelper = GameObject.Find("HotUpdateHelper").GetComponent<HotUpdateHelper>();

        EventTriggerListener.Get(startBtn.gameObject).onClick = StartGame;
        RequestNet();
    }

    public static ABManager abManager;
    LuaState luaState;
    void StartGame(GameObject obj, object param)
    {
        abManager = new ABManager();
        abManager.LoadAB(AppConfig.SearchUIABPath + "comm.tex");
        luaState = new LuaState();
        luaState.AddSearchPath(AppConfig.SearchGameLuaPath);
        LuaBinder.Bind(luaState);
        luaState.Start();
        luaState.DoFile("Others/main.lua");
        luaState.CheckTop();
        //luaState.Dispose();
        luaState = null;
    }

    /// <summary>
    /// 第一次请求服务器
    /// </summary>
    void RequestNet()
    {
        if (GameUtils.NetIsAvailable == false)
        {
            MessageBox.Instance.PopYesNo(StaticText.STR_NO_NET, () =>
            {
                Application.Quit();
            }, () =>
            {
                RequestNet();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        }
        else
        {
            currentCheckStatus = CheckStatus.CheckVersioning;
            UpdateUIStatus();
            GameUtils.PostHttp(AppConfig.ServerURL + "Login.ashx", null, onRequestSuccess, onRequestFailed);
        }
    }

    void onRequestSuccess(string message)
    {
        currentCheckStatus = CheckStatus.CheckVersionOver;
        UpdateUIStatus();
        try
        {
            JsonData res = JsonMapper.ToObject(message);
            bool success = (bool)res["success"];
            if (success)
            {
                JsonData note = res["data"];
                bool encrypted = (bool)res["encrypted"];
                if (encrypted)
                {
                    //是加密数据 
                    note = JsonMapper.ToObject(CSharpUtils.Decrypt((string)note, AppConfig.APP_SALT));
                }
                int serverForceVersion = (int)note["forceVersion"];
                int localForceVersion = AppConfig.APP_ForceVERSION;
                if (localForceVersion < serverForceVersion)
                {
                    //需要强更换包
                    string replaceAppUrl = (string)note["replaceAppUrl"];
                    MessageBox.Instance.PopOK(StaticText.ChangeApp, () =>
                    {
                        Application.OpenURL(replaceAppUrl);
                        Application.Quit();
                    }, StaticText.GoDownloadApp);
                }
                else
                {
                    string serverWeakVersion = (string)note["weakVersion"];
                    AppConfig.APP_WeakVERSION = serverWeakVersion; 
                    hotUpdateUrl = ((string)note["hotUpdateUrl"]);
                    CallHotUpdateHelper();
                }
            }
            else
            {
                //这里是由服务器返回请求失败的原因，例如服务器正在维护，在某某时间段才开服
                MessageBox.Instance.PopOK((string)res["error"], () =>
                {
                    Application.Quit();
                }, StaticText.QuitGame);
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
            MessageBox.Instance.PopYesNo(StaticText.CheckAssetsUpdateError, () =>
            {
                Application.Quit();
            }, () =>
            {
                hotUpdateHelper.Retry();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        };
        hotUpdateHelper.DownloadAssetsError += () =>
        {
            MessageBox.Instance.PopYesNo(StaticText.DownloadAssetsUpdateError, () =>
            {
                Application.Quit();
            }, () =>
            {
                hotUpdateHelper.Retry();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        };
        hotUpdateHelper.ConfirmDownloadAssets += () =>
        {
            currentCheckStatus = CheckStatus.CheckAssetsOver;
            UpdateUIStatus();
            string tip = GameUtils.WifiIsAvailable ? StaticText.ConfirmDownloadAssetsHasWifi : StaticText.ConfirmDownloadAssetsNoWifi;
            MessageBox.Instance.PopYesNo(string.Format(tip, GetShortSize(hotUpdateHelper.NeedUpdateSize)), () =>
            {
                Application.Quit();
            }, () =>
            {
                currentCheckStatus = CheckStatus.DownloadAssetsing;
                UpdateUIStatus();
                hotUpdateHelper.StartDownloadAssets();
                downloadTipText.gameObject.SetActive(true);
                progress.gameObject.SetActive(true);
            }, StaticText.QuitGame, StaticText.StartDownloadAssets);
        };
        hotUpdateHelper.StartGame += () =>
        {
            currentCheckStatus = CheckStatus.StartGame;
            UpdateUIStatus();
            StartGame(null,null);
        };
        currentCheckStatus = CheckStatus.CheckAssetsing;
        UpdateUIStatus();
        hotUpdateHelper.StartUpdate(hotUpdateUrl);
    }


    void onRequestFailed(string message)
    {
        currentCheckStatus = CheckStatus.CheckVersionOver;
        //这里的错误消息主要是因为网络原因造成，是由自己根据网络错误类型定义的
        MessageBox.Instance.PopOK(message, () =>
        {
            RequestNet();
        }, StaticText.STR_RETRY);
    }


    void Update()
    {
        if (currentCheckStatus == CheckStatus.DownloadAssetsing)
        {
            if (hotUpdateHelper.NeedUpdateSize > 0)
            {
                downloadTipText.text = string.Format(StaticText.DownloadShowText, GetShortSize(hotUpdateHelper.NeedUpdateSize), GetShortSize((int)hotUpdateHelper.DownloadSizePerSecond), AppConfig.APP_ForceVERSION);
                float value = ((float)hotUpdateHelper.HasDownloadSize / hotUpdateHelper.NeedUpdateSize);
                progress.text = string.Format("{0:#.##}%", value * 100);
                slider.value = value;
            }
        }
    }
    
    void UpdateUIStatus()
    {
        if (currentCheckStatus == CheckStatus.CheckVersioning)
        {
            tipText.text = StaticText.CheckVersioning;
        }
        else if (currentCheckStatus == CheckStatus.CheckVersionOver)
        {
            tipText.text = StaticText.CheckVersionOver;
        }
        else if (currentCheckStatus == CheckStatus.CheckAssetsing)
        {
            tipText.text = StaticText.CheckAssetsing;
        }
        else if (currentCheckStatus == CheckStatus.CheckAssetsOver)
        {
            tipText.text = StaticText.CheckAssetsOver;
        }
        else if (currentCheckStatus == CheckStatus.DownloadAssetsing)
        {
            downloadTipText.gameObject.SetActive(true);
            progress.gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
            tipText.text = StaticText.DownloadAssetsing;
        }
        else if (currentCheckStatus == CheckStatus.StartGame)
        {
            downloadTipText.gameObject.SetActive(false);
            tipText.gameObject.SetActive(false);
            progress.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(true);
            //StartGame(null, null);
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

    ///// <summary>
    ///// 释放资源
    ///// </summary>
    //public void CheckExtractResource()
    //{
    //    bool isExists = Directory.Exists(AppConfig.HotAssetsPath + "InnerLua/files.txt");
    //    if (isExists || AppConst.DebugMode)
    //    {
    //        StartCoroutine(OnUpdateResource());
    //        return;   //文件已经解压过了，自己可添加检查文件列表逻辑
    //    }
    //    StartCoroutine(OnExtractResource());    //启动释放协成 
    //}

    //IEnumerator OnExtractResource()
    //{
    //    string dataPath = Util.DataPath;  //数据目录
    //    string resPath = Util.AppContentPath(); //游戏包资源目录

    //    if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
    //    Directory.CreateDirectory(dataPath);

    //    string infile = resPath + "files.txt";
    //    string outfile = dataPath + "files.txt";
    //    if (File.Exists(outfile)) File.Delete(outfile);

    //    string message = "正在解包文件:>files.txt";
    //    Debug.Log(infile);
    //    Debug.Log(outfile);
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        WWW www = new WWW(infile);
    //        yield return www;

    //        if (www.isDone)
    //        {
    //            File.WriteAllBytes(outfile, www.bytes);
    //        }
    //        yield return 0;
    //    }
    //    else File.Copy(infile, outfile, true);
    //    yield return new WaitForEndOfFrame();

    //    //释放所有文件到数据目录
    //    string[] files = File.ReadAllLines(outfile);
    //    foreach (var file in files)
    //    {
    //        string[] fs = file.Split('|');
    //        infile = resPath + fs[0];  //
    //        outfile = dataPath + fs[0];

    //        message = "正在解包文件:>" + fs[0];
    //        Debug.Log("正在解包文件:>" + infile);
    //        facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

    //        string dir = Path.GetDirectoryName(outfile);
    //        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

    //        if (Application.platform == RuntimePlatform.Android)
    //        {
    //            WWW www = new WWW(infile);
    //            yield return www;

    //            if (www.isDone)
    //            {
    //                File.WriteAllBytes(outfile, www.bytes);
    //            }
    //            yield return 0;
    //        }
    //        else
    //        {
    //            if (File.Exists(outfile))
    //            {
    //                File.Delete(outfile);
    //            }
    //            File.Copy(infile, outfile, true);
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //    message = "解包完成!!!";
    //    facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
    //    yield return new WaitForSeconds(0.1f);

    //    message = string.Empty;
    //    //释放完成，开始启动更新资源
    //    StartCoroutine(OnUpdateResource());
    //}
}
