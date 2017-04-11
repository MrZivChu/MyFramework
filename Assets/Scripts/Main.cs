using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    List<string> updateFiles1 = new List<string>() {
        "http://www.hotupdate.com/ab/1.zip",
        "http://www.hotupdate.com/ab/2.zip",
        "http://www.hotupdate.com/ab/3.zip",
        "http://www.hotupdate.com/ab/4.zip",
        "http://www.hotupdate.com/ab/5.zip",
        "http://www.hotupdate.com/ab/6.zip",
        "http://www.hotupdate.com/ab/7.zip",
    };
    List<string> updateFiles2 = new List<string>() {
        "http://www.hotupdate.com/ab/8.zip",
        "http://www.hotupdate.com/ab/9.zip",
        "http://www.hotupdate.com/ab/10.zip",
        "http://www.hotupdate.com/ab/11.zip",
        "http://www.hotupdate.com/ab/12.zip",
        "http://www.hotupdate.com/ab/13.zip",
    };
    void Start() {
        GetComponent<DownloadFiles>().Download(updateFiles1);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            GetComponent<DownloadFiles>().Download(updateFiles2);
        }
    }


}
