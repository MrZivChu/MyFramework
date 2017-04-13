using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HotFile {
    public double size;
    public string url;
}
public class CalcProgress {
    public WWW www;
    public HotFile hotFile;
}

public class DownloadFiles : MonoBehaviour {
    List<Coroutine> listCoroutine = new List<Coroutine>();
    int needCompleteTimes = 0;
    int tempcompleteTimes = 0;
    int coroutineNums = 1;//最多几个协程并行下载文件
    public void Download(List<HotFile> updateFiles) {
        if (updateFiles != null && updateFiles.Count > 0) {
            Coroutine coroutine = null;
            int allCount = updateFiles.Count;//要下载的文件数量
            if (allCount <= coroutineNums) {
                needCompleteTimes = 1;
                coroutine = StartCoroutine("StartDownload", updateFiles);
                listCoroutine.Add(coroutine);
            } else {
                needCompleteTimes = coroutineNums;
                int count = 0;
                List<HotFile> temp = null;
                int spanNums = allCount / coroutineNums;
                int tempNums = spanNums * coroutineNums;
                int lastAddNums = allCount - tempNums;
                for (int i = 0; i < coroutineNums; i++) {
                    count = spanNums;
                    if (i == coroutineNums - 1) {
                        count = spanNums + lastAddNums;
                    }
                    temp = updateFiles.GetRange(0 + i * spanNums, count);
                    coroutine = StartCoroutine("StartDownload", temp);
                    listCoroutine.Add(coroutine);
                }
            }
        }
    }

    List<CalcProgress> calcProgressList = new List<CalcProgress>();

    List<HotFile> errorHotFile = new List<HotFile>();
    IEnumerator StartDownload(List<HotFile> file) {
        if (file != null && file.Count > 0) {
            string singlefile = string.Empty;
            WWW www = null;
            for (int i = 0; i < file.Count; i++) {
                singlefile = file[i].url;
                if (!string.IsNullOrEmpty(singlefile)) {
                    www = new WWW(singlefile);
                    CalcProgress dic = new CalcProgress();
                    dic.www = www;
                    dic.hotFile = file[i];
                    calcProgressList.Add(dic);
                    yield return www;
                    if (www != null) {
                        if (!string.IsNullOrEmpty(www.error)) {
                            print("下载失败:" + www.error);
                            errorHotFile.Add(file[i]);
                            //www.Dispose();
                            //www = null;
                        } else if (www.isDone) {
                            string[] names = singlefile.Split(new string[] { "/" }, System.StringSplitOptions.None);
                            string filename = names[names.Length - 1];
                            SaveFileToLocal(www.bytes, "d:/" + filename);
                            //www.Dispose();
                            //www = null;
                            print("下载成功" + filename);
                        }
                    }
                }
            }
        }
        tempcompleteTimes++;
    }


    void SaveFileToLocal(byte[] bytes, string filePath) {
        if (bytes != null && bytes.Length > 0) {
            using (var ms = new MemoryStream(bytes)) {
                using (Stream fs = new FileStream(filePath, FileMode.Create)) {
                    byte[] b = new byte[1024 * 1024 * 4];
                    int getv = 0;
                    while ((getv = ms.Read(b, 0, b.Length)) > 0) {
                        fs.Write(b, 0, getv);
                    }
                }
            }
        }
    }

    bool isOver = false;
    int Nums = 3;//重试几次
    void Update() {
        if (!isOver) {
            if (needCompleteTimes > 0 && tempcompleteTimes == needCompleteTimes) { //所有协程结束
                needCompleteTimes = 0;
                tempcompleteTimes = 0;
                CalcCurrentSize();
                StopAllCoroutine();
                if (errorHotFile != null && errorHotFile.Count > 0) {
                    if (Nums > 0) {
                        Nums -= 1;
                        print("第" + (3 - Nums) + "次重试");
                        List<HotFile> temp = new List<HotFile>();
                        temp.AddRange(errorHotFile);
                        errorHotFile = new List<HotFile>();
                        Download(temp);
                    }
                } else {
                    isOver = true;
                    DisposeWWW();
                    print("全部下载完成!");
                }
            } else {
                CalcCurrentSize();
            }
        }
    }

    public double currentDownloadSize = 0;//当前已经下载了多少
    void CalcCurrentSize() {
        if (calcProgressList != null && calcProgressList.Count > 0) {
            CalcProgress calcProgress = null;
            currentDownloadSize = 0;
            for (int i = 0; i < calcProgressList.Count; i++) {
                calcProgress = calcProgressList[i];
                if (string.IsNullOrEmpty(calcProgress.www.error)) {
                    currentDownloadSize += calcProgress.www.progress * calcProgress.hotFile.size;
                }
            }
        }
    }

    void StopAllCoroutine() {
        if (listCoroutine != null && listCoroutine.Count > 0) {
            for (int i = 0; i < listCoroutine.Count; i++) {
                StopCoroutine(listCoroutine[i]);
                listCoroutine[i] = null;
            }
            listCoroutine = new List<Coroutine>();
        }
    }

    void DisposeWWW() {
        if (calcProgressList != null && calcProgressList.Count > 0) {
            WWW temp;
            for (int i = 0; i < calcProgressList.Count; i++) {
                temp = calcProgressList[i].www;
                temp.Dispose();
                temp = null;
            }
            calcProgressList = new List<CalcProgress>();
        }
    }

    void OnGUI() {
        if (Nums <= 0 && errorHotFile != null && errorHotFile.Count > 0) {
            if (GUI.Button(new Rect(0, 200, 200, 200), "重试")) {
                List<HotFile> temp = new List<HotFile>();
                temp.AddRange(errorHotFile);
                errorHotFile = new List<HotFile>();
                Download(temp);
            }
        }
    }
}
