using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;
//using Tone.Assets;
//using Tone.Games;

public static class Utils
{

    #region 编码
    /// <summary>
    /// Base64编码
    /// </summary>
    public static string Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return System.Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    public static string Decode(string message)
    {
        byte[] bytes = System.Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }
    /// <summary>
    /// HashToMD5Hex
    /// </summary>
    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
            byte[] result = md5.ComputeHash(Bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));
            return builder.ToString();
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string StringMD5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++) {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string FileMD5(string file)
    {
        try {
            if (File.Exists(file)) {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            return "";
        }
        catch (System.Exception ex) {
            throw new System.Exception("FileMD5 fail, error:" + ex.Message);
        }
    }
    #endregion

    #region PlayerPrefs
    public static string GetKey(string key)
    {
        return Tone.AppConfig.APP_NAME + "_" + key;
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
            return PlayerPrefs.GetString(name);
        else
            return value;
    }

    public static void SetInt(string key, int value)
    {
        string name = GetKey(key);
        //PlayerPrefs.DeleteKey(key);
        PlayerPrefs.SetInt(name, value);
    }

    public static void SetString(string key, string value)
    {
        string name = GetKey(key);
        //PlayerPrefs.DeleteKey(key);
        PlayerPrefs.SetString(name, value);
    }

    public static void RemoveKey(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }
    #endregion

    public static long GetTime()
    {
        System.TimeSpan ts = new System.TimeSpan(System.DateTime.UtcNow.Ticks - new System.DateTime(2000, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }

    public static void Log(string msg)
    {
        Debug.Log(msg);
    }

    public static void Warning(string msg)
    {
#if DEBUG
        Debug.LogWarning(msg + "\n" + System.Environment.StackTrace);
#else
        Debug.LogWarning(msg);
#endif
    }

    public static void Error(string msg)
    {
#if DEBUG
        Debug.LogError(msg + "\n" + System.Environment.StackTrace);
#else
        Debug.LogError(msg);
#endif
    }

    public static void Assert(bool express, string msg)
    {
#if DEBUG
        if (!express) {
            Debug.LogWarning(System.Environment.StackTrace + " : " + msg);
            throw new System.Exception(msg);
        }
#endif
    }

}
