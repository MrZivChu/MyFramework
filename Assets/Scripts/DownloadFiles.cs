using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DownloadFiles : MonoBehaviour {

    List<Coroutine> listCoroutine = new List<Coroutine>();
    int completeTimes = 0;
    int tempcompleteTimes = 0;
    int coroutineNums = 5;//最多多少协程用于下载文件
    public void Download(List<string> updateFiles) {
        if (updateFiles != null && updateFiles.Count > 0) {
            int allCount = updateFiles.Count;//要下载的文件数量
            if (allCount <= coroutineNums) {
                completeTimes = 1;
                Coroutine coroutine = StartCoroutine("StartDownload", updateFiles);
                listCoroutine.Add(coroutine);
            } else {
                completeTimes = coroutineNums;
                int count = 0;
                List<string> temp = null;
                Coroutine coroutine = null;
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

    void Update() {
        if (completeTimes > 0 && tempcompleteTimes == completeTimes) {
            completeTimes = 0;
            tempcompleteTimes = 0;
            if (listCoroutine != null && listCoroutine.Count > 0) {
                for (int i = 0; i < listCoroutine.Count; i++) {
                    StopCoroutine(listCoroutine[i]);
                    listCoroutine[i] = null;
                }
            }
            listCoroutine = new List<Coroutine>();
            print("全部下载完成!");
        }
    }

    IEnumerator StartDownload(List<string> file) {
        if (file != null && file.Count > 0) {
            string singlefile = string.Empty;
            WWW Task = null;
            for (int i = 0; i < file.Count; i++) {
                singlefile = file[i];
                if (!string.IsNullOrEmpty(singlefile)) {
                    Task = new WWW(singlefile);
                    yield return Task;
                    if (Task != null) {
                        if (!string.IsNullOrEmpty(Task.error)) {
                            print("下载失败");
                            Task.Dispose();
                            Task = null;
                        } else if (Task.isDone) {
                            string[] names = singlefile.Split(new string[] { "/" }, System.StringSplitOptions.None);
                            string filename = names[names.Length - 1];
                            SaveFileToLocal(Task, filename);
                            Task.Dispose();
                            Task = null;
                            print("下载成功" + filename);
                        }
                    }
                }
            }
        }
        tempcompleteTimes++;
        yield return null;
    }

    public void SaveFileToLocal(WWW task, string fileName) {
        if (task != null) {
            byte[] bytes = task.bytes;
            using (var ms = new MemoryStream(bytes)) {
                string file = "d:/" + fileName;
                using (Stream fs = new FileStream(file, FileMode.Create)) {
                    byte[] b = new byte[1024 * 1024 * 4];
                    int getv = 0;
                    while ((getv = ms.Read(b, 0, b.Length)) > 0) {
                        fs.Write(b, 0, getv);
                    }
                }
            }
        }
    }
}
