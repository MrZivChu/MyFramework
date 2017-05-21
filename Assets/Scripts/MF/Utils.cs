﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Utils {
    /// <summary>
    /// 获取网络是否可用
    /// </summary>
    public static bool NetIsAvailable { get { return Application.internetReachability != NetworkReachability.NotReachable; } }

    /// <summary>
    /// post方式网络请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="fields"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailed"></param>
    public static void PostHttp(string url, string fields, System.Action<string> onSuccess, System.Action<string> onFailed) {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(fields)) {
            string[] str = fields.Split('&');
            for (int i = 0; i + 1 < str.Length; i += 2) {
                dict[str[i]] = str[i + 1];
            }
        }
        GameObject go = GameObject.Find("CoroutineHelper");
        go.GetComponent<CoroutineHelper>().StartCoroutine(HttpPost(url, dict, onSuccess, onFailed));
    }

    private static IEnumerator HttpPost(string url, Dictionary<string, string> fields, System.Action<string> onSuccess, System.Action<string> onFailed) {
        WWWForm form = new WWWForm(); 
        form.AddField("cpId", AppConfig.CP_ID.ToString());
        form.AddField("appId", AppConfig.APP_ID.ToString());
        form.AddField("channelId", AppConfig.CHANNEL_ID.ToString());
        form.AddField("clientVersion", AppConfig.APP_VERSION);
        if (fields != null) {
            foreach (var item in fields) {
                form.AddField(item.Key, item.Value);
            }
        }
        WWW www = new WWW(url, form);
        yield return www;

        if (www.isDone && string.IsNullOrEmpty(www.error)) {
            onSuccess(www.text);
        } else {
            if (www.error.Contains("Timed out")) {
                onFailed(StaticText.STR_TIMEOUT);
            } else if (www.error.Contains("Host unreachable")) {
                onFailed(StaticText.STR_UNREACHABLE);
            } else if (www.error.StartsWith("Could not resolve host")) {
                onFailed(StaticText.STR_NOT_RESOLVE);
            } else {
                onFailed(StaticText.STR_SERVER_FAILED);
            }
        }
        www.Dispose();
        www = null;
    }

    //解密AES
    public static string Decrypt(string toDecrypt) {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(AppConfig.APP_SECRET);
        SHA256 sha256 = new SHA256Managed();
        keyArray = sha256.ComputeHash(keyArray);

        byte[] ivArray = UTF8Encoding.UTF8.GetBytes("MrZivChu FrameWork");
        byte[] toEncryptArray = System.Convert.FromBase64String(toDecrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.IV = ivArray;
        rDel.Mode = CipherMode.CBC;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    #region PlayerPrefs
    static string GetKey(string key) {
        return AppConfig.APP_NAME + "_" + key;
    }

    public static bool HasKey(string key) {
        string name = GetKey(key);
        return PlayerPrefs.HasKey(name);
    }

    public static int GetInt(string key, int value) {
        string name = GetKey(key);
        if (PlayerPrefs.HasKey(name))
            return PlayerPrefs.GetInt(name);
        else
            return value;
    }

    public static string GetString(string key, string value) {
        string name = GetKey(key);
        if (PlayerPrefs.HasKey(name)) {
            string str = PlayerPrefs.GetString(name);
            str = WWW.UnEscapeURL(str);
            return str;
        } else {
            return value;
        }
    }

    public static void SetInt(string key, int value) {
        string name = GetKey(key);
        PlayerPrefs.SetInt(name, value);
    }

    public static void SetString(string key, string value) {
        string name = GetKey(key);
        value = WWW.EscapeURL(value);//用url编码,否则无法识别中文
        PlayerPrefs.SetString(name, value);
    }

    public static void RemoveKey(string key) {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }
    #endregion

    public static bool UncompressMemory(byte[] bytes) {
        using (var ms = new MemoryStream(bytes)) {
            using (var ar = SharpCompress.Archive.ArchiveFactory.Open(ms)) {
                foreach (var item in ar.Entries) {
                    if (!item.IsDirectory) {
                        string file = AppConfig.HotAssetsPath + item.FilePath;
                        string path = Path.GetDirectoryName(file);
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        using (FileStream fs = new FileStream(file, FileMode.Create)) {
                            item.WriteTo(fs);
                        }
                    }
                }
            }
        }
        return true;
    }
}