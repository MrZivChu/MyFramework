using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class FileData
{
    public string name;
    public string md5;
    public int size;
    public bool needUpdate;
}

public class HotUpdateHelper : MonoBehaviour
{

    //文件下载器
    FileDownloadHelper fileDownloadHelper = null;
    //服务器下发的热更地址
    string HotUpdateUrl = string.Empty;
    //总共需要下载的资源大小
    [HideInInspector]
    public int NeedUpdateSize = 0;
    //已经下载的资源大小
    public int HasDownloadSize
    {
        get
        {
            return fileDownloadHelper.DownloadSize;
        }
    }
    //每秒下载的数据大小
    public float DownloadSizePerSecond
    {
        get
        {
            return fileDownloadHelper.DownloadSizePerSecond;
        }
    }

    //下载资源失败
    public event Action DownloadAssetsError;
    //下载文件列表失败
    public event Action DownloadFileListError;
    //是否确认下载
    public event Action ConfirmDownloadAssets;
    //没有资源需要更新，开始游戏
    public event Action StartGame;

    void Awake()
    {
        fileDownloadHelper = GetComponent<FileDownloadHelper>();
    }

    /// <summary>
    /// 热更新开始
    /// </summary>
    /// <param name="hotUpdateUrl"></param>
    public void StartUpdate(string hotUpdateUrl)
    {
        HotUpdateUrl = hotUpdateUrl;
        HandleHotUpdateUrl();
        ToDownloadFileList();
    }
    /// <summary>
    /// 重试
    /// </summary>
    public void Retry()
    {
        fileDownloadHelper.Retry();
    }

    /// <summary>
    /// 根据不同的平台来确定不同的热更地址
    /// </summary>
    /// <param name="hotUpdateUrl"></param>
    void HandleHotUpdateUrl()
    {
        if (!string.IsNullOrEmpty(HotUpdateUrl))
        {
            if (!HotUpdateUrl.EndsWith("/"))
            {
                HotUpdateUrl += "/";
            }
            HotUpdateUrl += AppConfig.APP_FoceVERSION + "/" + Application.platform.ToString() + "/";
        }
        print("服务器热更地址 = " + HotUpdateUrl);
    }

    void ToDownloadFileList()
    {
        print("热更文件存储位置 = " + AppConfig.HotAssetsPath);
        fileDownloadHelper.Init();
        fileDownloadHelper.WorkDone += onDownloadFileListWorkDone;
        fileDownloadHelper.PushTask( HotUpdateUrl + AppConfig.LIST_FILENAME, 0, null, onDownLoadFileListSuccess, onDownLoadFileListFailed);
        fileDownloadHelper.StartDownload();
    }

    void onDownloadFileListWorkDone(object sender, System.EventArgs e)
    {
        if (fileDownloadHelper.IsCompleted)
        {
            print("文件列表下载成功");
            CheckFileMD5();
        }
        else
        {
            print("文件列表下载失败");
            if (DownloadFileListError != null)
            {
                DownloadFileListError();
            }
        }
    }

    string[] downloadFileStringList = null;
    List<FileData> mServerFileList = null;
    void onDownLoadFileListSuccess(WWW www, object o)
    {
        downloadFileStringList = www.text.Split('\n');
        mServerFileList = new List<FileData>();
        foreach (var item in downloadFileStringList)
        {
            if (string.IsNullOrEmpty(item.Trim()))
            {
                continue;
            }
            string[] tokens = item.Split('|');
            if (tokens.Length >= 3)
            {
                FileData data = new FileData();
                data.needUpdate = true;
                data.name = tokens[0].Trim();
                data.md5 = tokens[1].Trim();
                data.size = 0;
                int.TryParse(tokens[2], out data.size);
                if (data.size > 0)
                {
                    mServerFileList.Add(data);
                }
            }
        }
    }

    void onDownLoadFileListFailed(string err, object item)
    {
        print(err);
    }

    void CheckFileMD5()
    {
        //得到本地的文件列表
        string[] oldLines = null;
        if (File.Exists(AppConfig.LocalMD5FilePath))
        {
            oldLines = File.ReadAllLines(AppConfig.LocalMD5FilePath);
        }
        if (oldLines != null && oldLines.Length > 0)
        {
            //去除不需要更新的文件
            foreach (var item in oldLines)
            {
                if (string.IsNullOrEmpty(item.Trim())) continue;
                string[] tokens = item.Split('|');
                if (tokens.Length < 2) continue;

                string name = tokens[0].Trim();
                string md5 = tokens[1].Trim();
                //最好实时获取本地文件的MD5值，与服务器端的MD5值进行比较，避免本地文件损坏，造成程序运行出现问题，主要出于性能考虑所以暂且不采用此方法
                if (!File.Exists(AppConfig.HotAssetsPath + name))
                {
                    continue;
                }

                for (int i = 0; i < mServerFileList.Count; i++)
                {
                    if (mServerFileList[i].name == name && mServerFileList[i].md5 == md5)
                    {
                        mServerFileList[i].needUpdate = false;
                        break;
                    }
                }
            }
        }
        CalcUpdateSize();
        if (NeedUpdateSize > 0)
        {
            if (ConfirmDownloadAssets != null)
            {
                print("确认是否下载");
                ConfirmDownloadAssets();
            }
            else
            {
                StartDownloadAssets();
            }
        }
        else
        {
            print("没有最新资源可更新，直接进入游戏");
            if (StartGame != null)
            {
                StartGame();
            }
        }
    }

    void CalcUpdateSize()
    {
        foreach (var item in mServerFileList)
        {
            if (item.needUpdate)
            {
                NeedUpdateSize += item.size;
            }
        }
    }

    public void StartDownloadAssets()
    {
        if (NeedUpdateSize > 0)
        {
            print("StartDownloadAssets");
            fileDownloadHelper.Init();
            fileDownloadHelper.WorkDone += onDownloadAssetsWorkDone;
            foreach (var item in mServerFileList)
            {
                if (item.needUpdate)
                {
                    fileDownloadHelper.PushTask(HotUpdateUrl + item.name, item.size, item, onDownloadAssetSuccess, onDownloadAssetFailed);
                }
            }
            fileDownloadHelper.StartDownload();
        }
    }

    void onDownloadAssetsWorkDone(object sender, System.EventArgs e)
    {
        if (fileDownloadHelper.IsCompleted)
        {
            print("所有ab资源下载成功");
            File.WriteAllLines(AppConfig.LocalMD5FilePath, downloadFileStringList);
            if (StartGame != null)
            {
                StartGame();
            }
        }
        else
        {
            print("ab资源下载失败");
            UpdateFileList();
            if (DownloadAssetsError != null)
            {
                DownloadAssetsError();
            }
        }
    }


    void onDownloadAssetSuccess(WWW www, object item)
    {
        print("单个ab下载成功");
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            if (CSharpUtils.UncompressMemory(AppConfig.HotAssetsPath, www.bytes))
            {
                FileData data = item as FileData;
                data.needUpdate = false;
            }
        }
    }

    void onDownloadAssetFailed(string err, object item)
    {
        print("单个ab下载失败" + err);
    }

    void OnApplicationQuit()
    {
        print("程序退出");
        UpdateFileList();
    }
    void UpdateFileList()
    {
        if (mServerFileList != null && mServerFileList.Count > 0)
        {
            string reslut = string.Empty;
            FileData fileData = null;
            for (int i = 0; i < mServerFileList.Count; i++)
            {
                fileData = mServerFileList[i];
                if (fileData.needUpdate == false)
                {
                    reslut += fileData.name + "|" + fileData.md5 + "|" + fileData.size + "\n";
                }
            }
            reslut.TrimEnd('\n');
            File.WriteAllText(AppConfig.LocalMD5FilePath, reslut);
            print("保存已经下载的文件的MD5值");
        }
    }
}
