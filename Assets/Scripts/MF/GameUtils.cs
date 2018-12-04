using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;

public static class GameUtils
{
    /// <summary>
    /// 获取网络是否可用
    /// </summary>
    public static bool NetIsAvailable { get { return Application.internetReachability != NetworkReachability.NotReachable; } }

    /// <summary>
    /// wifi是否可用
    /// </summary>
    public static bool WifiIsAvailable { get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; } }

    /// <summary>
    /// Get方式网络请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailed"></param>
    public static void GetHttp(string url, System.Action<string> onSuccess, System.Action<string> onFailed)
    {
        UnityEngine.EventSystems.EventSystem es = UnityEngine.EventSystems.EventSystem.current;
        es.StartCoroutine(HttpGet(url, onSuccess, onFailed));
    }

    private static System.Collections.IEnumerator HttpGet(string url, System.Action<string> onSuccess, System.Action<string> onFailed)
    {
        if (url.IndexOf('?') > 0)
        {
            if (!url.EndsWith("&")) url += "&";
        }
        else
        {
            url += "?";
        }
        url += "&appId=" + AppConfig.APP_ID;
        url += "&channelId=" + AppConfig.CHANNEL_ID;
        url += "&clientFoceVersion=" + AppConfig.APP_FoceVERSION;

        WWW www = new WWW(url);
        yield return www;

        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            onSuccess(www.text);
        }
        else
        {
            onFailed(www.error);
        }
    }


    /// <summary>
    /// post方式网络请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="fields"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailed"></param>
    public static void PostHttp(string url, string fields, System.Action<string> onSuccess, System.Action<string> onFailed)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(fields))
        {
            string[] str = fields.Split('&');
            for (int i = 0; i + 1 < str.Length; i += 2)
            {
                dict[str[i]] = str[i + 1];
            }
        }
        UnityEngine.EventSystems.EventSystem es = UnityEngine.EventSystems.EventSystem.current;
        es.StartCoroutine(HttpPost(url, dict, onSuccess, onFailed));
    }

    private static IEnumerator HttpPost(string url, Dictionary<string, string> fields, System.Action<string> onSuccess, System.Action<string> onFailed)
    {
        WWWForm form = new WWWForm();
        form.AddField("appId", AppConfig.APP_ID.ToString());
        form.AddField("channelId", AppConfig.CHANNEL_ID.ToString());
        form.AddField("clientFoceVersion", AppConfig.APP_FoceVERSION);
        if (fields != null)
        {
            foreach (var item in fields)
            {
                form.AddField(item.Key, item.Value);
            }
        }
        WWW www = new WWW(url, form);
        yield return www;

        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            onSuccess(www.text);
        }
        else
        {
            if (www.error.Contains("Timed out"))
            {
                onFailed(StaticText.STR_TIMEOUT);
            }
            else if (www.error.Contains("Host unreachable"))
            {
                onFailed(StaticText.STR_UNREACHABLE);
            }
            else if (www.error.StartsWith("Could not resolve host"))
            {
                onFailed(StaticText.STR_NOT_RESOLVE);
            }
            else
            {
                onFailed(StaticText.STR_SERVER_FAILED);
            }
        }
        www.Dispose();
        www = null;
    }

    #region PlayerPrefs
    static string GetKey(string key)
    {
        return AppConfig.APP_NAME + "_" + key;
    }

    public static bool HasKey(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.HasKey(name);
    }

    public static int GetInt(string key, int value)
    {
        string name = GetKey(key);
        if (PlayerPrefs.HasKey(name))
            return PlayerPrefs.GetInt(name);
        else
            return value;
    }

    public static string GetString(string key, string value)
    {
        string name = GetKey(key);
        if (PlayerPrefs.HasKey(name))
        {
            string str = PlayerPrefs.GetString(name);
            str = WWW.UnEscapeURL(str);
            return str;
        }
        else
        {
            return value;
        }
    }

    public static void SetInt(string key, int value)
    {
        string name = GetKey(key);
        PlayerPrefs.SetInt(name, value);
    }

    public static void SetString(string key, string value)
    {
        string name = GetKey(key);
        value = WWW.EscapeURL(value);//用url编码,否则无法识别中文
        PlayerPrefs.SetString(name, value);
    }

    public static void RemoveKey(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }
    #endregion

}
