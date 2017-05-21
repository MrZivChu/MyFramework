using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticText {
    public static readonly string STR_NO_NET = "网络不可用，请打开网络！";
    public static readonly string STR_TIMEOUT = "登录服务器超时，请检查网络！";
    public static readonly string STR_UNREACHABLE = "无法连接服务器，请检查网络！";
    public static readonly string STR_NOT_RESOLVE = "无法找到服务器，请检查网络配置！";
    public static readonly string STR_SERVER_FAILED = "连接服务器失败，请检查网络！";

    public static readonly string STR_RETRY = "重试";

    public static readonly string Data_Error = "处理数据出错";
    public static readonly string QuitGame = "退出游戏";
    public static readonly string ChangeApp = "需要去应用商店下载最新包";
    public static readonly string GoDownloadApp = "去下载";

    public static readonly string CheckAssetsUpdateError = "检查热更资源失败";
    public static readonly string DownloadAssetsUpdateError = "下载热更资源失败";
    public static readonly string ConfirmDownloadAssetsHasWifi = "本次热更新资源大小为{0}\n您现在处于WIFI环境中,可以放心下载\n是否确认下载";
    public static readonly string ConfirmDownloadAssetsNoWifi = "本次热更新资源大小为{0}\n您现在处于非WIFI环境中\n是否确认下载";
    public static readonly string StartDownloadAssets = "开始下载";
    public static readonly string DownloadShowText = "本次下载{0}，速度为{1}/s ({2})";


    public static readonly string CheckVersioning = "正在检查版本更新";
    public static readonly string CheckVersionOver = "版本更新检查完毕";
    public static readonly string CheckAssetsing = "正在检查资源更新";
    public static readonly string CheckAssetsOver = "资源更新检查完毕";

    public static readonly string DownloadAssetsing = "正在为您下载更新资源包";
    public static readonly string Ok = "确定";
    public static readonly string StartGame = "开始游戏";
    public static readonly string Welcome = "欢迎来到我的游戏";
}
