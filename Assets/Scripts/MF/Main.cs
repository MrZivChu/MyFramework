using LitJson;
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

public enum CheckStatus {
    Default = 0,
    CheckVersioning = 1,//检查版本更新
    CheckVersionOver = 1,//检查版本更新完毕
    CheckAssetsing = 2,//检查资源更新
    CheckAssetsOver = 3,//检查资源更新结束完毕
    DownloadAssetsing = 4,//下载资源
    StartGame = 5,//开始游戏
}

public class Main : MonoBehaviour {

    string hotUpdateUrl = string.Empty; //热更地址
    HotUpdateHelper hotUpdateHelper = null;
    CheckStatus currentCheckStatus = CheckStatus.Default;
    CheckStatus preCheckStatus = CheckStatus.Default;

    public Text tipText;
    public Text downloadTipText;
    public Text progress;
    public Slider slider;
    public Button startBtn;

    void Start() {
        downloadTipText.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        tipText.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(false);
        hotUpdateHelper = GameObject.Find("HotUpdateHelper").GetComponent<HotUpdateHelper>();

        EventTriggerListener.Get(startBtn.gameObject).onClick = StartGame;
        RequestNet();
    }

    void StartGame(GameObject obj, object param) {
        print("startgame");
        LuaState lua = new LuaState();
        LuaBinder.Bind(lua);
        lua.Start();
        string fullPath = Application.dataPath.Replace("Assets", "Lua");
        lua.AddSearchPath(fullPath);
        lua.DoFile("Others/main.lua");
        //lua.Collect();
        //lua.CheckTop();
        //lua.Dispose();
        //lua = null;
    }

    /// <summary>
    /// 第一次请求服务器
    /// </summary>
    void RequestNet() {
        if (Utils.NetIsAvailable == false) {
            MessageBox.Instance.PopYesNo(StaticText.STR_NO_NET, () => {
                Application.Quit();
            }, () => {
                RequestNet();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        } else {
            currentCheckStatus = CheckStatus.CheckVersioning;
            string fields = "name&Tom&age&18";
            Utils.PostHttp(AppConfig.ServerURL + "Login.ashx", fields, onRequestSuccess, onRequestFailed);
        }
    }

    //游戏的完整版本号
    string appVersion = string.Empty;
    void onRequestSuccess(string message) {
        currentCheckStatus = CheckStatus.CheckVersionOver;
        try {
            JsonData res = JsonMapper.ToObject(message);
            string result = (string)res["result"];
            if (result == "success") {
                JsonData note = res["data"];
                int encrypted = Convert.ToInt16(res["encrypted"].ToString());
                if (encrypted > 0) {//是加密数据 
                    note = JsonMapper.ToObject(Utils.Decrypt((string)note, AppConfig.APP_SALT));
                }
                string version = (string)note["version"];
                if (version == "needReplace") { //需要强更换包
                    string replaceAppUrl = (string)note["replaceAppUrl"];
                    MessageBox.Instance.PopOK(StaticText.ChangeApp, () => {
                        Application.OpenURL(replaceAppUrl);
                        Application.Quit();
                    }, StaticText.GoDownloadApp);
                } else {
                    appVersion = "";//(string)note["Version"];
                    hotUpdateUrl = ((string)note["hotUpdateUrl"]).Replace("com", "xyz");
                    CallHotUpdateHelper();
                }
            } else {
                //这里是由服务器返回请求失败的原因，例如服务器正在维护，在某某时间段才开服
                MessageBox.Instance.PopOK((string)res["error"], () => {
                    Application.Quit();
                }, StaticText.QuitGame);
            }
        } catch (Exception ex) {
            MessageBox.Instance.PopOK(StaticText.Data_Error + ex.Message, () => {
                Application.Quit();
            }, StaticText.QuitGame);
        }
    }

    void CallHotUpdateHelper() {
        hotUpdateHelper.DownloadFileListError += () => {
            MessageBox.Instance.PopYesNo(StaticText.CheckAssetsUpdateError, () => {
                Application.Quit();
            }, () => {
                hotUpdateHelper.Retry();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        };
        hotUpdateHelper.DownloadAssetsError += () => {
            MessageBox.Instance.PopYesNo(StaticText.DownloadAssetsUpdateError, () => {
                Application.Quit();
            }, () => {
                hotUpdateHelper.Retry();
            }, StaticText.QuitGame, StaticText.STR_RETRY);
        };
        hotUpdateHelper.ConfirmDownloadAssets += () => {
            currentCheckStatus = CheckStatus.CheckAssetsOver;
            string tip = Utils.WifiIsAvailable ? StaticText.ConfirmDownloadAssetsHasWifi : StaticText.ConfirmDownloadAssetsNoWifi;
            MessageBox.Instance.PopYesNo(string.Format(tip, GetShortSize(hotUpdateHelper.NeedUpdateSize)), () => {
                Application.Quit();
            }, () => {
                currentCheckStatus = CheckStatus.DownloadAssetsing;
                hotUpdateHelper.StartDownloadAssets();
                downloadTipText.gameObject.SetActive(true);
                progress.gameObject.SetActive(true);
            }, StaticText.QuitGame, StaticText.StartDownloadAssets);
        };
        hotUpdateHelper.StartGame += () => {
            currentCheckStatus = CheckStatus.StartGame;

        };
        currentCheckStatus = CheckStatus.CheckAssetsing;
        hotUpdateHelper.StartUpdate(hotUpdateUrl);
    }


    void onRequestFailed(string message) {
        currentCheckStatus = CheckStatus.CheckVersionOver;
        //这里的错误消息主要是因为网络原因造成，是由自己根据网络错误类型定义的
        MessageBox.Instance.PopOK(message, () => {
            RequestNet();
        }, StaticText.STR_RETRY);
    }


    void Update() {
        if (preCheckStatus != currentCheckStatus) {
            if (currentCheckStatus == CheckStatus.CheckVersioning) {
                tipText.text = StaticText.CheckVersioning;
            } else if (currentCheckStatus == CheckStatus.CheckVersionOver) {
                tipText.text = StaticText.CheckVersionOver;
            } else if (currentCheckStatus == CheckStatus.CheckAssetsing) {
                tipText.text = StaticText.CheckAssetsing;
            } else if (currentCheckStatus == CheckStatus.CheckAssetsOver) {
                tipText.text = StaticText.CheckAssetsOver;
            } else if (currentCheckStatus == CheckStatus.DownloadAssetsing) {
                downloadTipText.gameObject.SetActive(true);
                progress.gameObject.SetActive(true);
                slider.gameObject.SetActive(true);
                tipText.text = StaticText.DownloadAssetsing;
            } else if (currentCheckStatus == CheckStatus.StartGame) {
                downloadTipText.gameObject.SetActive(false);
                tipText.gameObject.SetActive(false);
                progress.gameObject.SetActive(false);
                slider.gameObject.SetActive(false);
                startBtn.gameObject.SetActive(true);
            }
            preCheckStatus = currentCheckStatus;
        }
        if (currentCheckStatus == CheckStatus.DownloadAssetsing) {
            if (hotUpdateHelper.NeedUpdateSize > 0) {
                downloadTipText.text = string.Format(StaticText.DownloadShowText, GetShortSize(hotUpdateHelper.NeedUpdateSize), GetShortSize((int)hotUpdateHelper.DownloadSizePerSecond), appVersion);
                float value = ((float)hotUpdateHelper.HasDownloadSize / hotUpdateHelper.NeedUpdateSize);
                progress.text = string.Format("{0:#.##}%", value * 100);
                slider.value = value;
                if (value == 1) {
                    currentCheckStatus = CheckStatus.StartGame;
                }
            }
        }
    }

    string GetShortSize(int size) {
        if (size < 1024) {
            return string.Format("{0:#.##}B", size);
        } else if (size < 1048576) {
            return string.Format("{0:#.##}KB", size / 1024f);
        } else {
            return string.Format("{0:#.##}MB", size / 1048576f);
        }
    }
}
