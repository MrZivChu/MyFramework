using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class FileData {
    public string name;
    public string md5;
    public int size;
    public bool needUpdate;
}

public class HotUpdateHelper : MonoBehaviour {

    string HotUpdateFileSavePath = string.Empty;//热更文件保存地址

    public FileDownloadHelper fileDownloadHelper = null;
    public event Func<int, bool> ConfirmDownload;


    void Awake() {
        fileDownloadHelper = GetComponent<FileDownloadHelper>();
    }

    public void StartUpdate(string hotUpdateUrl) {
        hotUpdateUrl = HandleUpdateUrl(hotUpdateUrl);
        HotUpdateUrl = hotUpdateUrl;
        ToDownloadFileList(hotUpdateUrl);
    }

    string HotUpdateUrl = string.Empty;
    /// <summary>
    /// 根据不同的平台来确定不同的热更地址
    /// </summary>
    /// <param name="hotUpdateUrl"></param>
    string HandleUpdateUrl(string hotUpdateUrl) {
        if (!string.IsNullOrEmpty(hotUpdateUrl)) {
            if (!hotUpdateUrl.EndsWith("/")) {
                hotUpdateUrl += "/";
            }
            hotUpdateUrl += AppConfig.APP_FoceVERSION;
#if UNITY_ANDROID
            hotUpdateUrl += "/Android/";
#elif UNITY_IOS
			HotUpdateUrl += "/iOS/";
#else
			HotUpdateUrl += "/Windows/";
#endif
            if (!Directory.Exists(AppConfig.HotAssetsPath)) {
                Directory.CreateDirectory(AppConfig.HotAssetsPath);
            }
        }
        return hotUpdateUrl;
    }

    void ToDownloadFileList(string hotUpdateUrl) {
        fileDownloadHelper.Init();
        fileDownloadHelper.WorkDone += onDownloadFileListWorkDone;
        fileDownloadHelper.PushTask(hotUpdateUrl + AppConfig.LIST_FILENAME, 0, null, onDownLoadFileListSuccess, onDownLoadFileListFailed);
        fileDownloadHelper.StartDownload();
    }

    private void onDownloadFileListWorkDone(object sender, System.EventArgs e) {
        if (fileDownloadHelper.IsCompleted) {
            print("文件列表下载成功");
            CheckFileMD5();
        } else {
            //重试
            fileDownloadHelper.Retry();
        }
    }

    string[] downloadFileStringList = null;
    List<FileData> mServerFileList = null;
    private void onDownLoadFileListSuccess(WWW www, object o) {
        print("onDownLoadFileListSuccess");
        downloadFileStringList = www.text.Split('\n');
        mServerFileList = new List<FileData>();
        foreach (var item in downloadFileStringList) {
            if (string.IsNullOrEmpty(item.Trim())) continue;
            string[] tokens = item.Split('|');
            if (tokens.Length >= 3) {
                FileData data = new FileData();
                data.needUpdate = true;
                data.name = tokens[0].Trim();
                data.md5 = tokens[1].Trim();
                data.size = 0;
                int.TryParse(tokens[2], out data.size);
                if (data.size > 0) mServerFileList.Add(data);
            }
        }
    }

    private void onDownLoadFileListFailed(string err, object item) {
        print(err);
    }

    private string GetLocalFileListPath() {
        string localFile = AppConfig.HotAssetsPath + AppConfig.LIST_FILENAME;
        if (!File.Exists(localFile)) {
            using (File.Create(localFile)) { }
        }
        return localFile;
    }

    private void CheckFileMD5() {
        //得到本地的文件列表
        string localFile = GetLocalFileListPath();
        string[] oldLines = File.ReadAllLines(localFile);

        //去除不需要更新的文件
        foreach (var item in oldLines) {
            if (string.IsNullOrEmpty(item.Trim())) continue;

            string[] tokens = item.Split('|');
            if (tokens.Length < 2) continue;

            string name = tokens[0].Trim();
            string md5 = tokens[1].Trim();

            if (!File.Exists(AppConfig.HotAssetsPath + name)) {
                continue;
            }

            for (int i = 0; i < mServerFileList.Count; i++) {
                if (mServerFileList[i].name == name && mServerFileList[i].md5 == md5) {
                    mServerFileList[i].needUpdate = false;
                    break;
                }
            }
        }
        CalcUpdateSize();
        if (ConfirmDownload != null) {
            if (ConfirmDownload(NeedUpdateSize)) {
                StartDownloadAssets();
            }
        } else {
            StartDownloadAssets();
        }
    }

    public int NeedUpdateSize = 0;
    void CalcUpdateSize() {
        foreach (var item in mServerFileList) {
            if (item.needUpdate) { NeedUpdateSize += item.size; }
        }
    }

    public void StartDownloadAssets() {
        if (NeedUpdateSize > 0) {
            print("StartDownloadAssets");
            fileDownloadHelper.Init();
            fileDownloadHelper.WorkDone += onDownloadAssetsWorkDone;
            foreach (var item in mServerFileList) {
                if (item.needUpdate)
                    fileDownloadHelper.PushTask(HotUpdateUrl + item.name, item.size, item, onDownloadAssetSuccess, onDownloadAssetFailed);
            }
            fileDownloadHelper.StartDownload();
        }
    }



    private void onDownloadAssetsWorkDone(object sender, System.EventArgs e) {
        if (fileDownloadHelper.IsCompleted) {
            print("所有ab资源下载成功");
            string temp = GetLocalFileListPath();
            File.WriteAllLines(temp, downloadFileStringList);
        } else {
            //重试
            //fileDownloadHelper.Retry();
        }
    }


    private void onDownloadAssetSuccess(WWW www, object item) {
        print("单个ab下载成功");
        if (www.isDone && string.IsNullOrEmpty(www.error)) {
            if (Utils.UncompressMemory(www.bytes)) {
                FileData data = item as FileData;
                data.needUpdate = false;
            }
        }
    }

    private void onDownloadAssetFailed(string err, object item) {
        print("单个ab下载失败" + err);
    }
}
