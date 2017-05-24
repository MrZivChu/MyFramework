using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    private MessageBox()
    { }

    private static GameObject messageBoxObj;
    private static GameObject canvas;
    public static MessageBox Instance
    {
        get
        {
            if (canvas == null)
            {
                canvas = GameObject.Find("Canvas");
            }
            if (messageBoxObj == null)
            {
                UnityEngine.Object obj = Resources.Load("MessageBox");
                messageBoxObj = Instantiate(obj) as GameObject;
                messageBoxObj.transform.parent = canvas.transform;
                messageBoxObj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                messageBoxObj.transform.localScale = Vector3.one;
            }
            return messageBoxObj.GetComponent<MessageBox>();
        }
    }

    public Button leftBtn;
    public Button rightBtn;
    public Text tipText;

    public Text leftBtnText;
    public Text rightBtnText;

    public void PopOK(string tip, Action callback, string btnText)
    {
        gameObject.SetActive(true);
        leftBtn.gameObject.SetActive(false);
        rightBtn.gameObject.SetActive(true);
        rightBtnText.text = btnText;
        tipText.text = tip;
        EventTriggerListener.Get(rightBtn.gameObject).onClick = (go, data) =>
        {
            if (callback != null)
            {
                callback();
            }
            gameObject.SetActive(false);
        };
    }

    public void PopYesNo(string tip, Action leftCallback, Action rightCallback, string tleftBtnText, string trightBtnText)
    {
        gameObject.SetActive(true);
        leftBtnText.text = tleftBtnText;
        rightBtnText.text = trightBtnText;
        tipText.text = tip;
        leftBtn.gameObject.SetActive(true);
        rightBtn.gameObject.SetActive(true);
        EventTriggerListener.Get(leftBtn.gameObject).onClick = (go, data) =>
        {
            if (leftCallback != null)
            {
                leftCallback();
            }
            gameObject.SetActive(false);
        };
        EventTriggerListener.Get(rightBtn.gameObject).onClick = (go, data) =>
        {
            if (rightCallback != null)
            {
                rightCallback();
            }
            gameObject.SetActive(false);
        };
    }
}
