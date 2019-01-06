using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UrlData {
    public string Url { get; private set; }
    public int Size { get; private set; }
    public object Data { get; private set; }
    public Action<WWW, object> onSuccess { get; private set; }
    public Action<string, object> onFailed { get; private set; }

    public WWW www { get; private set; }
    public int TryTimes { get; set; }
    public bool IsSuccess { get; private set; }
    public bool IsFailed { get; private set; }

    private float mLastProgress;

    public UrlData(string url, int size, object data, Action<WWW, object> callback, Action<string, object> failed) {
        Url = url;
        Size = size;
        Data = data;
        www = null;
        onSuccess = callback;
        onFailed = failed;
    }

    /// <summary>
    /// 获取每桢下载的数据为多少
    /// </summary>
    /// <returns></returns>
    public int GetFrameSize() {
        if (www != null && www.progress > mLastProgress) {
            int size = (int)((www.progress - mLastProgress) * Size);
            mLastProgress = www.progress;
            return size;
        }
        return 0;
    }

    /// <summary>
    /// 获取现在总共下载了多少数据
    /// </summary>
    /// <returns></returns>
    public int GetDownloadSize() {
        if (mLastProgress < 1.0) {
            return (int)(Size * mLastProgress);
        }
        return Size;
    }

    public void Start() {
        www = new WWW(Url);
        TryTimes++;
        IsSuccess = false;
        IsFailed = false;

        mLastProgress = 0.0f;
    }

    public void Update() {
        if (www != null) {
            if (!string.IsNullOrEmpty(www.error)) {
                DownloadError();
            } else if (www.isDone) {
                if(www.bytes.Length == 0)
                    DownloadError();
                else
                    DownloadSuccess();
            }
        }
    }
    
    void DownloadError()
    {
        if (TryTimes == AppConfig.MAX_TRY_TIMES) {
            if (onFailed != null) {
                onFailed(Url + ":" + www.error, Data);
            }
        }
        www.Dispose();
        www = null;
        IsFailed = true;
    }
    
    void DownloadSuccess()
    {
        if (onSuccess != null) {
            onSuccess(www, Data);
        }
        www.Dispose();
        www = null;
        IsSuccess = true;
    }
};


public class FileDownloadHelper : MonoBehaviour {

    #region Properties
    /// <summary>
    /// 正在执行和正在等待的任务数量总和
    /// </summary>
    public int WaitingAndRunningCount { get { return mWaiting.Count + mRunning.Count; } }
    /// <summary>
    /// 最多可同时执行多少个任务
    /// </summary>
    public int MaxTaskCountInSameTime { get; private set; }
    /// <summary>
    /// 当前正在执行的任务数量
    /// </summary>
    public int RunningCount { get { return mRunning.Count; } }
    /// <summary>
    /// 已经完成的任务数量
    /// </summary>
    public int FinishedCount { get { return mFinished.Count; } }
    /// <summary>
    /// 任务是否成功完成，并且不存在失败的任务
    /// </summary>
    public bool IsCompleted { get { return WaitingAndRunningCount == 0; } }
    /// <summary>
    /// 是否正在下载
    /// </summary>
    public bool IsDownloading { get; private set; }
    /// <summary>
    /// 每秒下载的数据大小
    /// </summary>
    public float DownloadSizePerSecond { get; private set; }
    /// <summary>
    /// 已经总共下载的数据大小
    /// </summary>
    public int DownloadSize {
        get {
            int size = 0;
            foreach (var item in mRunning) {
                size += item.GetDownloadSize();
            }
            return mTotalSize + size;
        }
    }
    /// <summary>
    /// 任务执行完时的回调
    /// </summary>
    public event EventHandler WorkDone;

    #endregion

    #region Fields
    List<UrlData> mWaiting = new List<UrlData>();
    List<UrlData> mRunning = new List<UrlData>();
    List<UrlData> mFinished = new List<UrlData>();

    int mTotalSize;
    int mSecondSize;
    float mPerSecond;
    #endregion

    void Update() {
        if (!IsDownloading) {
            return;
        }
        if (mRunning.Count > 0) {
            for (int i = mRunning.Count - 1; i >= 0; i--) {
                mSecondSize += mRunning[i].GetFrameSize();
                mRunning[i].Update();
                if (mRunning[i].IsSuccess) {
                    mTotalSize += mRunning[i].Size;
                    mFinished.Add(mRunning[i]);
                    mRunning.RemoveAt(i);
                } else if (mRunning[i].IsFailed) {
                    mWaiting.Add(mRunning[i]);
                    mRunning.RemoveAt(i);
                }
            }
            mPerSecond += Time.deltaTime;
            if (mPerSecond >= 1.0f) {
                DownloadSizePerSecond = mSecondSize / mPerSecond;
                mSecondSize = 0;
                mPerSecond = 0.0f;
            }
        }

        if (mWaiting.Count > 0) {
            if (mRunning.Count < MaxTaskCountInSameTime) {
                UrlData urlData = FindEnabledTask();
                if (urlData != null) {
                    urlData.Start();
                    mRunning.Add(urlData);
                }
            }
        }

        if (mRunning.Count == 0) {
            IsDownloading = false;
            if (WorkDone != null) {
                WorkDone(this, null);
            }
        }
    }

    private UrlData FindEnabledTask() {
        UrlData urlData = null;
        foreach (var item in mWaiting) {
            if (item.TryTimes < AppConfig.MAX_TRY_TIMES) {
                urlData = item;
                mWaiting.Remove(item);
                break;
            }
        }
        return urlData;
    }

    public void Init() {
        mWaiting.Clear();
        mRunning.Clear();
        mFinished.Clear();

        mTotalSize = 0;
        DownloadSizePerSecond = 0.0f;
        mSecondSize = 0;
        mPerSecond = 0.0f;
        IsDownloading = false;
        MaxTaskCountInSameTime = AppConfig.MAX_DOWNLOAD_TASKS;
        WorkDone = null;
    }

    public void StartDownload() {
        if (IsDownloading == false) {
            IsDownloading = true;
        }
    }

    public void Retry() {
        if (IsDownloading == false) {
            //重试就要把所有失败的任务重试次数重置为0
            foreach (var item in mWaiting) {
                item.TryTimes = 0;
            }
            IsDownloading = true;
        }
    }

    public void PushTask(string url, int size, object data, Action<WWW, object> onSuccess, Action<string, object> onFailed) {
        mWaiting.Add(new UrlData(url, size, data, onSuccess, onFailed));
    }
}
