using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

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
    };
    double allSize = 154112628;
    DownloadFiles downloadFiles;
    void Start()
    {
        downloadFiles = GetComponent<DownloadFiles>();
        downloadFiles.Download(updateFiles1);
    }

    double currentSize = 0;
    void OnGUI()
    {
        currentSize = 0;
        foreach (var item in downloadFiles.currentAllSize)
        {
            currentSize += item.Value;
        }
        GUILayout.Label((currentSize + " = " + allSize).ToString());
        GUILayout.Label((currentSize / allSize).ToString("0.##"));
    }


}
