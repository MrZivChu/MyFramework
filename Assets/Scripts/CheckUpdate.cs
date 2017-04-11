using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CheckUpdate : MonoBehaviour
{
    float coroutineNums = 4;//最多的协程数量
    List<string> updateFiles = new List<string>() {
        "http://www.hotupdate.com/ab/BackPack.unity3d",
        "http://www.hotupdate.com/ab/Battle.unity3d",
        "http://www.hotupdate.com/ab/Chapter.unity3d",
        "http://www.hotupdate.com/ab/Chat.unity3d",
        "http://www.hotupdate.com/ab/DailyCopy.unity3d",
        "http://www.hotupdate.com/ab/DaYanTa.unity3d",
        "http://www.hotupdate.com/ab/Email.unity3d",
        "http://www.hotupdate.com/ab/Equipment.unity3d",
        "http://www.hotupdate.com/ab/Fabao.unity3d",
        "http://www.hotupdate.com/ab/FabaoLoot.unity3d",
        "http://www.hotupdate.com/ab/Fairyland.unity3d",
        "http://www.hotupdate.com/ab/Friend.unity3d",
        "http://www.hotupdate.com/ab/Gang.unity3d",
    };

    void Start()
    {
        int allCount = updateFiles.Count;//要下载的文件数量
        int spanNums = (int)Mathf.Ceil(allCount / coroutineNums);
        int count = 0;
        List<string> temp = null;
        for (int i = 0; i < coroutineNums; i++)
        {
            count = allCount >= spanNums ? spanNums : allCount;
            temp = updateFiles.GetRange(0 + i * spanNums, count);
            StartCoroutine("StartDownload", temp);
            allCount = allCount - spanNums;
        }
    }

    IEnumerator StartDownload(List<string> file)
    {
        if (file != null && file.Count > 0)
        {
            string singlefile = string.Empty;
            WWW Task = null;
            for (int i = 0; i < file.Count; i++)
            {
                singlefile = file[i];
                if (!string.IsNullOrEmpty(singlefile))
                {
                    Task = new WWW(singlefile);
                    yield return Task;
                    if (Task != null)
                    {
                        if (!string.IsNullOrEmpty(Task.error))
                        {
                            print("下载失败");
                            Task.Dispose();
                            Task = null;
                        }
                        else if (Task.isDone)
                        {
                            string[] names = singlefile.Split(new string[] { "/" }, System.StringSplitOptions.None);
                            string filename = names[names.Length - 1];
                            Download(Task, filename);
                            Task.Dispose();
                            Task = null;
                            print("下载成功" + filename);
                        }
                    }
                }
            }
        }
        yield return null;
    }

    public void Download(WWW task, string fileName)
    {
        byte[] bytes = task.bytes;
        using (var ms = new MemoryStream(bytes))
        {
            string file = "d:/" + fileName;
            using (Stream fs = new FileStream(file, FileMode.Create))
            {
                byte[] b = new byte[1024 * 1024 * 4];
                int getv = 0;
                while ((getv = ms.Read(b, 0, b.Length)) > 0)
                {
                    fs.Write(b, 0, getv);
                }
            }
        }
    }
}
