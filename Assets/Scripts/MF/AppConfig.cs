using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppConfig {
    //应用名称
    public const string APP_NAME = "MF";
    //公司哪个应用（ID唯一标识）
    public const int APP_ID = 1;
    //此应用是哪个渠道的（ID唯一标识）
    public const int CHANNEL_ID = 1;


    //请求检查资源更新的服务器地址
    public const string ServerURL = "http://www.hotupdate.xyz/";
    //文件列表的文件名称
    public const string LIST_FILENAME = "files.list";
    //强更版本号
    public const string APP_FoceVERSION = "1.3";
    //我们的热更新规则是根据下载的文件列表来确定更新内容的，而不是根据版本号
    //因为本地文件有可能会被清除，所以用下载的文件列表对本地文件进行检测是最保险的方式
    //所以检查更新，只要检查是否有强更就行了，没有强更就下载文件列表进行资源更新检测
    //APP_VERSION仅仅用来显示app的版本号而已，此值是从服务器获取的，然后保存到本地的PlayerPrefs
    public const string APP_VERSION = "";

    //网络数据加密盐
    public const string APP_SALT = "AQMSQEchcrYkbN5A";


    //最大重试次数
    public const int MAX_TRY_TIMES = 3;
    //最大同时下载数量
    public const int MAX_DOWNLOAD_TASKS = 3;

    // 热更新资源所在路径
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
