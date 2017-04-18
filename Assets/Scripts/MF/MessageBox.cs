using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour {

    public Button btn1;
    public Button btn2;
    public Text tipText;

    public void PopOK(string tip, Action callback, string btnText) {
        btn2.gameObject.SetActive(false);
        btn1.gameObject.transform.Find("Text").GetComponent<Text>().text = btnText;
        tipText.text = tip;
        EventTriggerListener.Get(btn1.gameObject).onClick = (go) => {
            if (callback != null) {
                callback();
            }
            Destroy(gameObject);
        };
    }

    public void PopYesNo(string tip, Action callback1, Action callback2, string btnText1, string btnText2) {
        btn1.gameObject.transform.Find("Text").GetComponent<Text>().text = btnText1;
        btn2.gameObject.transform.Find("Text").GetComponent<Text>().text = btnText2;
        tipText.text = tip;
        EventTriggerListener.Get(btn1.gameObject).onClick = (go) => {
            if (callback1 != null) {
                callback1();
            }
            Destroy(gameObject);
        };
        EventTriggerListener.Get(btn2.gameObject).onClick = (go) => {
            if (callback2 != null) {
                callback2();
            }
            Destroy(gameObject);
        };
    }
}
