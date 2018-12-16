using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppConfig
{
    //应用名称
    public const string APP_NAME = "MF";
    //公司哪个应用（ID唯一标识）
    public const int APP_ID = 1;
    //此应用是哪个渠道的（ID唯一标识）
    public const int CHANNEL_ID = 1;


    //请求检查资源更新的服务器地址
    public const string ServerURL = "http://www.moling.mobi/";
    //文件列表的文件名称
    public const string LIST_FILENAME = "files.list";
    //强更版本号
    //我们的热更新规则是根据下载的文件列表来确定更新内容的，而不是根据版本号
    //因为本地文件有可能会被清除，所以用下载的文件列表对本地文件进行检测是最保险的方式
    //所以检查更新，只要检查是否有强更就行了，没有强更就下载文件列表进行资源更新检测
    public static string APP_FoceVERSION = "1.3";

    //网络数据加密盐
    public const string APP_SALT = "AQMSQEchcrYkbN5A";

    //最大重试次数
    public const int MAX_TRY_TIMES = 3;
    //最大同时下载数量
    public const int MAX_DOWNLOAD_TASKS = 5;

    // 热更新资源所在路径
    public static string HotAssetsPath
    {
        get
        {
            return Application.persistentDataPath + "/";
        }
    }

    public static string LocalMD5FilePath
    {
        get
        {
            return HotAssetsPath + "/" + LIST_FILENAME;
        }
    }

    /// <summary>
    /// 应用程序内容路径，即streamingAssetsPath目录
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/StreamingAssets/";
                break;
        }
        return path;
    }

    public static string InnerLuaHotAssetsPath
    {
        get
        {
            return HotAssetsPath + "/InnerLua/";
        }
    }

    public static string GameLuaHotAssetsPath
    {
        get
        {
            return HotAssetsPath + "/GameLua/";
        }
    }

    public static string UIHotAssetsPath
    {
        get
        {
            return HotAssetsPath + "/UI/";
        }
    }


    public static string SearchGameLuaPath
    {
        get
        {
            return Application.isEditor ? Application.dataPath + "/Lua/GameLua" : GameLuaHotAssetsPath;
        }
    }

    public static string SearchInnerLuaPath
    {
        get
        {
            return Application.isEditor ? Application.dataPath + "/Lua/InnerLua" : InnerLuaHotAssetsPath;
        }
    }

    public static string SearchUIABPath
    {
        get
        {
            return Application.isEditor ? @"F:\MyFrameworkAssets\UI\" : UIHotAssetsPath;
        }
    }
}
