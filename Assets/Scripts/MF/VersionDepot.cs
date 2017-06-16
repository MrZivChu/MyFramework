using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class VersionDepot
{
    public enum ResourceLocation
    {
        None,
        Resources,
        StreamingAssets,
        persistentDataPath,
    }

    struct AssetBundleVersion
    {
        public int Version;
        public long Size;
        public ResourceLocation Location;
    }

    Dictionary<string, AssetBundleVersion> mVersion = new Dictionary<string, AssetBundleVersion>();

    public void Clear()
    {
        mVersion.Clear();
    }

    public void Load()
    {
        mVersion.Clear();
        DirectoryInfo di = new DirectoryInfo(AppConfig.HotAssetsPath);
        if (di.Exists)
        {
            LoadFolder(di, di.FullName.Length, ResourceLocation.persistentDataPath);
        }
        di = new DirectoryInfo(Application.streamingAssetsPath);
        if (di.Exists)
        {
            LoadFolder(di, di.FullName.Length, ResourceLocation.StreamingAssets);
        }
    }

    private void LoadFolder(DirectoryInfo folder, int length, ResourceLocation loc)
    {
        FileInfo[] files = folder.GetFiles();
        foreach (var item in files)
        {
            if (item.Name.EndsWith(".tex") || item.Name.EndsWith(".ab"))
            {
                string name = (folder.FullName.Length <= length ? item.Name : folder.FullName.Substring(length) + "/" + item.Name).ToLower();
                if (mVersion.ContainsKey(name) == false)
                {
                    AssetBundleVersion abv = new AssetBundleVersion();
                    abv.Version = 1;
                    abv.Size = item.Length;
                    abv.Location = loc;
                    name = name.Replace("\\", "/");
                    mVersion.Add(name, abv);
                }
            }
        }
        DirectoryInfo[] dirs = folder.GetDirectories();
        foreach (var item in dirs)
        {
            LoadFolder(item, length, loc);
        }
    }

    public bool Contains(string resName)
    {
        return mVersion.ContainsKey(resName.ToLower());
    }

    public ResourceLocation QueryResouceLocation(string resName)
    {
        string low = resName.ToLower();
        if (!Contains(low))
            return ResourceLocation.None;
        else
            return mVersion[low].Location;
    }

    public string GetResourcePath(string resName)
    {
        ResourceLocation loc = QueryResouceLocation(resName);
        switch (loc)
        {
            case ResourceLocation.persistentDataPath:
                return AppConfig.HotAssetsPath + resName;
            case ResourceLocation.StreamingAssets:
                return Application.streamingAssetsPath + "/" + resName;
            default:
                return resName;
        }
    }
}

