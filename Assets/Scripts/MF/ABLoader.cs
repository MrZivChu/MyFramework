using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ABLoader是AssetBundle的加载器，加载完成后会卸载内存镜像，但是不会释放Assets对象
// 释放Assets由ResourceDepot通过调用Resouces.UnloadUnusedAsset()完成
public class ABLoader
{
    Dictionary<string, AssetBundle> mLoaded = new Dictionary<string, AssetBundle>();

    private List<string> GetDependencies(string abName)
    {
        List<string> list = new List<string>();
        if (abName.EndsWith(".ab"))
        {
            list.Add(abName.Replace(".ab", ".tex"));
        }
        list.Add(abName);
        return list;
    }

    private List<AssetBundle> GetAssetBundles(List<string> depList)
    {
        List<AssetBundle> list = null;
        if (depList != null && depList.Count > 0)
        {
            list = new List<AssetBundle>();
            string abName = string.Empty;
            for (int i = 0; i < depList.Count; i++)
            {
                abName = depList[i];
                string path = AppGlobal.VersionDepot.GetResourcePath(abName);
                AssetBundle ab = null;
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
                ab = AssetBundle.LoadFromFile(path);
#else
                ab = AssetBundle.LoadFromFile(path);
#endif
                list.Add(ab);
                mLoaded[abName] = ab;
            }
        }
        return list;
    }

    public bool isLoaded(string abName)
    {
        return mLoaded.ContainsKey(abName);
    }

    public void UnloadAB(string abName)
    {
        if (mLoaded.ContainsKey(abName))
        {
            mLoaded[abName].Unload(true);
            mLoaded.Remove(abName);
        }
    }

    public void ClearAB()
    {
        foreach (AssetBundle item in mLoaded.Values)
        {
            item.Unload(true);
        }
        mLoaded.Clear();
    }

    public List<AssetBundle> LoadAssetBundle(string abName)
    {
        if (!mLoaded.ContainsKey(abName))
        {
            List<string> depList = GetDependencies(abName);
            if (depList.Count > 2)
            {
                Debug.Log(abName + "普通界面ab不能存在两个以上的依赖！！！");
                return null;
            }
            return GetAssetBundles(depList);
        }
        return null;
    }

    public Object[] LoadAllAssets(string abName, System.Type type = null)
    {
        if (!mLoaded.ContainsKey(abName))
        {
            List<string> depList = GetDependencies(abName);
            if (depList.Count > 1)
            {
                Debug.Log(abName + "公共图集的ab不能存在一个以上的依赖！！！");
                return null;
            }
            List<AssetBundle> abList = GetAssetBundles(depList);

            Object[] objs = null;
            if (abList[0] != null)
            {
                if (type == null)
                {
                    objs = abList[0].LoadAllAssets();
                }
                else
                {
                    objs = abList[0].LoadAllAssets(type);
                }
            }
            return objs;
        }
        return null;
    }

    public Object LoadAsset(string abName, string resName, System.Type type = null)
    {
        if (mLoaded.ContainsKey(abName))
        {
            if (type == null)
            {
                return mLoaded[abName].LoadAsset(resName);
            }
            else
            {
                return mLoaded[abName].LoadAsset(resName, type);
            }
        }

        List<string> deps = new List<string>() { abName };
        List<AssetBundle> abs = GetAssetBundles(deps);

        Object obj = null;
        if (abs != null && abs.Count > 0)
        {
            if (abs[0].Contains(resName))
            {
                if (type == null)
                {
                    obj = abs[0].LoadAsset(resName);
                }
                else
                {
                    obj = abs[0].LoadAsset(resName, type);
                }
            }
            abs[0].Unload(false);
        }
        return obj;
    }
}
