using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppConfig {
    //应用名称
    public const string APP_NAME = "MF";
    //合作商
    public const int CP_ID = 1;
    //公司哪个应用
    public const int APP_ID = 1;
    //此应用是哪个渠道的
    public const int CHANNEL_ID = 1;


    //服务器地址
    public const string ServerURL = "http://www.hotupdate.xyz/";
    //强更版本号
    public const string APP_FoceVERSION = "1.3";
    //版本号
    public const string APP_VERSION = "1.3.0";

    //数据加密盐
    public const string APP_SECRET = "AQMSQEchcrYkbN5A";


    //最大重试次数
    public const int MAX_TRY_TIMES = 3;
    //最大同时下载数量
    public const int MAX_DOWNLOAD_TASKS = 3;


    //文件列表的文件名称
    public const string LIST_FILENAME = "files.list";


    /// <summary>
    /// 热更新资源所在路径
    /// </summary>
    public static string HotAssetsPath {
        get {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Application.persistentDataPath + "/assets/";
#elif UNITY_ANDROID
            return Application.persistentDataPath + "/assets/";
#else
            return Application.temporaryCachePath + "/assets/";
#endif
        }
    }
}
