using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    List<HotFile> updateFiles1 = new List<HotFile>() {
        new HotFile() { size = 103322224, url = "http://www.hotupdate.com/ab/1.zip" },
        new HotFile() { size = 19071619, url = "http://www.hotupdate.com/ab/2.zip"},
        new HotFile() { size = 20777879, url = "http://www.hotupdate.com/ab/3.zip" },
        new HotFile() { size = 23822, url = "http://www.hotupdate.com/ab/4.zip"},
        new HotFile() { size = 84323, url = "http://www.hotupdate.com/ab/5.zip"},
        new HotFile() { size = 110628, url = "http://www.hotupdate.com/ab/6.zip"},
        new HotFile() { size = 1486058, url = "http://www.hotupdate.com/ab/7.zip"},
        new HotFile() { size = 294906, url =  "http://www.hotupdate.com/ab/8.zip"},
        new HotFile() { size = 126255, url =  "http://www.hotupdate.com/ab/9.zip"},
        new HotFile() { size = 49140, url = "http://www.hotupdate.com/ab/10.zip"},
        new HotFile() { size = 8381791, url = "http://www.hotupdate.com/ab/11.zip"},
        new HotFile() { size = 44287, url = "http://www.hotupdate.com/ab/12.zip"},
        new HotFile() { size = 339696, url = "http://www.hotupdate.com/ab/13.zip"},
        new HotFile() { size = 339696, url = "http://www.hotupdate.com/ab/14.zip"},
    };
    double allSize = 154452324;
    DownloadFiles downloadFiles;
    void Start() {
        //可以实例化带有脚本DownloadFiles的对象，来达到多线程处理多个下载任务
        print("开始时间" + DateTime.Now.ToLocalTime());
        downloadFiles = GetComponent<DownloadFiles>();
        downloadFiles.Download(updateFiles1);
    }

    double currentSize = 0;
    void OnGUI() {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充  
        fontStyle.normal.textColor = new Color(1, 0, 0);   //设置字体颜色  
        fontStyle.fontSize = 40;       //字体大小  

        currentSize = downloadFiles.currentDownloadSize;
        GUI.Label(new Rect(0, 0, 200, 200), (currentSize + " = " + allSize).ToString(), fontStyle);
        GUI.Label(new Rect(0, 50, 200, 200), (currentSize / allSize).ToString("0.##"), fontStyle);
        string num = ((currentSize / allSize) * 100).ToString("0");
        GUI.Label(new Rect(0, 100, 200, 200), (num == "100" ? (currentSize == allSize ? "100" : "99") : num) + "%", fontStyle);
        if (currentSize == allSize) {
            print("结束时间" + DateTime.Now.ToLocalTime());
        }
    }
}
