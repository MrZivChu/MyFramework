using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour {

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
    enum CurrentBtnSattusEnum {
        none = 0,
        replaceApp = 1,
        startUpdate = 2,
        startPlay = 3,
        startRetry = 4,
    }

    string hotUpdateUrl = string.Empty; //热更地址
    string replaceAppUrl = string.Empty; //下载最新强更包的地址
    CurrentBtnSattusEnum currentBtnSattusEnum = Main.CurrentBtnSattusEnum.none;

    public Button goReplaceAppBtn;
    public Button startUpdateBtn;
    public Button startPlayBtn;
    public Button startRetryBtn;
    public Text msgText;


    void Start() {
        RegisterButtonClick();
        RequestNet();
    }

    void RegisterButtonClick() {
        EventTriggerListener.Get(goReplaceAppBtn.gameObject).onClick = onReplaceApp;
        EventTriggerListener.Get(startUpdateBtn.gameObject).onClick = onStartUpdate;
        EventTriggerListener.Get(startPlayBtn.gameObject).onClick = onStartPlay;
        EventTriggerListener.Get(startRetryBtn.gameObject).onClick = onStartRetry;
    }

    //需要去重新下载最新包（强更）
    void onReplaceApp(GameObject go) {
        if (!string.IsNullOrEmpty(replaceAppUrl)) {
            Application.OpenURL(replaceAppUrl);
        }
    }

    //开始热更
    void onStartUpdate(GameObject go) {
        if (!string.IsNullOrEmpty(hotUpdateUrl)) {
            
        }
    }

    //开始游戏
    void onStartPlay(GameObject go) {
        SceneManager.LoadScene(1);
    }

    //重试
    void onStartRetry(GameObject go) {
        RequestNet();
    }

    void ChangeBtnStatus() {
        goReplaceAppBtn.gameObject.SetActive(currentBtnSattusEnum == CurrentBtnSattusEnum.replaceApp);
        startUpdateBtn.gameObject.SetActive(currentBtnSattusEnum == CurrentBtnSattusEnum.startUpdate);
        startPlayBtn.gameObject.SetActive(currentBtnSattusEnum == CurrentBtnSattusEnum.startPlay);
        startRetryBtn.gameObject.SetActive(currentBtnSattusEnum == CurrentBtnSattusEnum.startRetry);
    }

    /// <summary>
    /// 第一次请求服务器
    /// </summary>
    void RequestNet() {
        if (Utils.NetIsAvailable == false) {
            print("网络不可用");
        } else {
            string fields = "name&" + 123 + "&age&" + 456 + "&hobby&" + 10;
            Utils.PostHttp(AppConfig.LOGIN_URL + "messages", fields, onRequestSuccess, onRequestFailed);
        }
    }

    void onRequestSuccess(string message) {
        try {
            JsonData res = JsonMapper.ToObject(message);
            string result = (string)res["result"];
            if (result == "success") {
                string data = (string)res["data"];
                int encrypted = Convert.ToInt16(res["encrypted"].ToString());
                if (encrypted > 0) {//是加密数据 
                    data = Utils.Decrypt(data);
                }
                JsonData note = JsonMapper.ToObject(data);
                string version = (string)note["version"];
                if (version == "needReplace") { //需要强更换包
                    replaceAppUrl = (string)note["messageURL"];
                    currentBtnSattusEnum = CurrentBtnSattusEnum.replaceApp;
                } else {
                    currentBtnSattusEnum = CurrentBtnSattusEnum.startUpdate;
                    hotUpdateUrl = (string)note["onlineUpdatePackageURL"];
                }
            } else {
                //这里是由服务器返回请求失败的原因，例如服务器正在维护，在某某时间段才开服
                print((string)res["error"]);
            }
        } catch (Exception ex) {
            currentBtnSattusEnum = CurrentBtnSattusEnum.startRetry;
            print(ex.Message);
        }
        ChangeBtnStatus();
    }
    void onRequestFailed(string message) {
        //这里的错误消息主要是因为网络原因造成，是由自己根据网络错误类型定义的
        print("请求服务器失败 = " + message);
        currentBtnSattusEnum = CurrentBtnSattusEnum.startRetry;
        ChangeBtnStatus();
    }

}
