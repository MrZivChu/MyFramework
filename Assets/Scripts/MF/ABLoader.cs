using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ABLoader是AssetBundle的加载器，加载完成后会卸载内存镜像，但是不会释放Assets对象
// 释放Assets由ResourceDepot通过调用Resouces.UnloadUnusedAsset()完成
public class ABLoader
{
    Dictionary<string, AssetBundle> mLoaded = new Dictionary<string, AssetBundle>();
    Dictionary<string, string> mDepend = new Dictionary<string, string>();

    private void getDependencies(string abName, List<string> depList)
    {
        if (abName.EndsWith(".ab"))
        {
            string dep = abName.Replace(".ab", ".tex");
            depList.Add(dep);
        }
        depList.Add(abName);
    }

    private void getAssetBundles(List<string> depList, List<AssetBundle> abList)
    {
        for (int i = 0; i < depList.Count; i++)
        {
            if (string.IsNullOrEmpty(depList[i]) || mLoaded.ContainsKey(depList[i])) continue;
            string path = "";// AppGlobal.VersionDepot.GetResourcePath(deps[i]);
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
            abList.Add(AssetBundle.LoadFromFile(path));
#else
            abList.Add(AssetBundle.CreateFromFile(path));
#endif

        }
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

        if (mDepend.ContainsKey(abName))
        {
            string dep = mDepend[abName];
            mDepend.Remove(abName);

            if (mLoaded.ContainsKey(dep))
            {
                mLoaded[dep].Unload(true);
                mLoaded.Remove(dep);
            }
        }
    }

    public void ClearAB(bool clearAll)
    {
        if (clearAll)
        {
            foreach (AssetBundle item in mLoaded.Values)
            {
                item.Unload(true);
            }
            mLoaded.Clear();
            mDepend.Clear();
        }
        else
        {
            string str = "";
            foreach (AssetBundle item in mLoaded.Values)
            {
                str += item.name + ", ";
            }
        }
    }

    public void LoadAssetBundle(string abName)
    {
        if (!mLoaded.ContainsKey(abName))
        {
            List<string> depList = new List<string>();
            getDependencies(abName, depList);
            if (depList.Count > 2)
            {
                Debug.Log(abName + "存在两个以上的依赖！！！");
                return;
            }

            List<AssetBundle> abList = new List<AssetBundle>();
            getAssetBundles(depList, abList);
            for (int i = 0; i < abList.Count; i++)
            {
                if (abList[i] != null)
                {
                    mLoaded[depList[i]] = abList[i];
                }
            }
            mDepend[abName] = depList[0];
        }
    }

    public Object[] LoadAllAssets(string abName, System.Type type)
    {
        if (!mLoaded.ContainsKey(abName))
        {
            List<string> depList = new List<string>();
            getDependencies(abName, depList);
            if (depList.Count > 1)
            {
                return null;
            }

            List<AssetBundle> abList = new List<AssetBundle>();
            getAssetBundles(depList, abList);

            Object[] objs = null;
            if (abList[0] != null)
            {
                if (type == null)
                    objs = abList[0].LoadAllAssets();
                else
                    objs = abList[0].LoadAllAssets(type);
                mLoaded[abName] = abList[0];
            }
            return objs;
        }
        return null;
    }

    /// <summary>
    /// 加载指定的预设
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public Object LoadAsset(string abName, string resName, System.Type type = null)
    {
        if (string.IsNullOrEmpty(abName)) return null;

        //如果AB已经加载，则直接获取Asset
        if (mLoaded.ContainsKey(abName))
        {
            if (type == typeof(Transform))
            {
                GameObject o = mLoaded[abName].LoadAsset(resName) as GameObject;
                if (o != null)
                {
                    return o.transform;
                }
            }
            else if (type == null)
            {
                return mLoaded[abName].LoadAsset(resName);
            }
            else
            {
                return mLoaded[abName].LoadAsset(resName, type);
            }
        }

        List<string> deps = new List<string>();
        //得到所有依赖，应该没有需要加载的依赖
        getDependencies(abName, deps);
        if (deps.Count > 1)
        {
            return null;
        }

        //载入所有AB
        List<AssetBundle> abs = new List<AssetBundle>();
        getAssetBundles(deps, abs);
        if (abs[0] == null) return null;

        //获取GameObject
        Object obj = null;
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
        return obj;
    }

    public IEnumerator LoadAssetBundle(string abName, System.Action onDone, System.Action onFailed)
    {
        List<string> deps = new List<string>();

        if (string.IsNullOrEmpty(abName))
        {
            onFailed();
            yield break;
        }

        if (mLoaded.ContainsKey(abName))
        {
            onDone();
            yield break;
        }

        //得到所有依赖，应该没有需要加载的依赖
        getDependencies(abName, deps);
        if (deps.Count > 2)
        {
            yield break;
        }

        for (int i = 0; i < deps.Count; i++)
        {
            //if (AppGlobal.VersionDepot.Contains(deps[i]))
            {
                string path = "";// AppGlobal.VersionDepot.GetResourcePath(deps[i]);
                //WWW会被网络请求卡住，因此改用LoadFromFileAsync
                AssetBundleCreateRequest cr = AssetBundle.LoadFromFileAsync(path);
                yield return cr;

                if (cr.isDone && cr.assetBundle != null)
                {
                    mLoaded.Add(deps[i], cr.assetBundle);
                    //Utils.Log(deps[i] + " was loaded as mirror.");

                    if (i < deps.Count - 1)
                    {
                        mDepend[abName] = deps[i];
                    }
                }
                else
                {
                    onFailed();
                }
            }
            //else
            //{
            //    onFailed();
            //}
        }

        onDone();
    }

    public IEnumerator LoadAllAssets(string abName, System.Type type, System.Action<string, Object[]> onDone, System.Action onFailed)
    {
        List<string> deps = new List<string>();

        if (string.IsNullOrEmpty(abName))
        {
            onFailed();
            yield break;
        }

        if (mLoaded.ContainsKey(abName))
        {
            onFailed();
            yield break;
        }

        //仅能依赖公共AB
        getDependencies(abName, deps);
        if (deps.Count > 1)
        {
            yield break;
        }

        for (int i = 0; i < deps.Count; i++)
        {
            //if (AppGlobal.VersionDepot.Contains(deps[i]))
            {
                string path = "";// AppGlobal.VersionDepot.GetResourcePath(deps[i]);
                //WWW会被网络请求卡住，因此改用LoadFromFileAsync
                AssetBundleCreateRequest cr = AssetBundle.LoadFromFileAsync(path);
                yield return cr;

                if (cr.isDone && cr.assetBundle != null)
                {
                    mLoaded[cr.assetBundle.name] = cr.assetBundle;
                    //由于改用TP，不能直接通过LoadAsset来获取TP合成的碎图，只能保存LoadAll了
                    AssetBundleRequest abr = cr.assetBundle.LoadAllAssetsAsync(i == deps.Count - 1 ? type : typeof(Sprite));
                    yield return abr;

                    if (i < deps.Count - 1)
                    {
                        mDepend[abName] = deps[i];
                    }
                    //Utils.Log("LoadAllAssets : " + deps[i] + " was loaded all.");
                    onDone(deps[i], abr.allAssets);
                }
                else
                {
                    onFailed();
                    break;
                }
            }
            //else
            //{
            //    onFailed();
            //    break;
            //}
        }
    }
}
