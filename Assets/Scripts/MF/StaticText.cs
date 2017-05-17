using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticText {
    public static readonly string STR_UNKNOWN = "未知错误";
    public static readonly string STR_NO_NET = "网络不可用，请打开网络！";
    public static readonly string STR_TIMEOUT = "登录服务器超时，请检查网络！";
    public static readonly string STR_UNREACHABLE = "无法连接服务器，请检查网络！";
    public static readonly string STR_NOT_RESOLVE = "无法找到服务器，请检查网络配置！";
    public static readonly string STR_SERVER_FAILED = "连接服务器失败，请检查网络！";

    public static readonly string STR_FIRSTRUN = "新版本第一次运行，正在解压资源，请耐心等待！";
    public static readonly string STR_DOWNLOADING = "正在下载资源，";
    public static readonly string STR_CHECKING = "正在检查资源，请稍候。。。";
    public static readonly string STR_WIFI_ASK = "需要下载资源{0}，您现在处于WIFI环境中，可以放心下载。现在就下载吗？";
    public static readonly string STR_3G_ASK = "需要下载资源{0}，您现在处于非WIFI环境中，请小心流量。现在可以下载吗？";
    public static readonly string STR_DONE = "已经完成资源下载，{0}/{0}";
    public static readonly string STR_READY = "本地资源已经是最新的！";
    public static readonly string STR_BAD_NET = "网络不稳定，请再试一次。如果依然失败，请检查网络！(GN2)";
    public static readonly string STR_RETRY_ASK = "部分资源下载失败，需要再试一次吗？";
    public static readonly string STR_RETRY = "重试";
    public static readonly string STR_LOW_DEVICE = "为了更好的体验,单核2G以内的安卓设备暂未开放游戏!";

    public static readonly string Data_Error = "处理数据出错";
    public static readonly string QuitGame = "退出游戏";
    public static readonly string ChangeApp = "需要去应用商店下载最新包";
    public static readonly string GoDownloadApp = "去下载";

    public static readonly string DownloadFileListError = "检查热更失败";
    public static readonly string DownloadAssetsError = "下载资源失败";
    public static readonly string ConfirmDownloadAssets = "本次热更新大小为{0}，是否确认下载";
    public static readonly string StartDownloadAssets = "开始下载";
    public static readonly string DownloadShowText = "本次下载{0}，速度为{1}/s ({2})";


    public static readonly string Ok = "确定";
    public static readonly string StartGame = "开始游戏";
    public static readonly string Welcome = "欢迎来到我的游戏";
}
