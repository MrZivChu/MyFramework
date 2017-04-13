using System.Collections;
using System.Collections.Generic;
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
}
