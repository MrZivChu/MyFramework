using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PackagerEditor : EditorWindow
{
    [MenuItem("Window/打包")]
    static void PackagerWindow()
    {
        PackagerEditor packagerEditor = (PackagerEditor)EditorWindow.GetWindow(typeof(PackagerEditor), false, "打包", true);
        packagerEditor.minSize = new Vector2(600, 220);
        packagerEditor.maxSize = new Vector2(600, 220);
        packagerEditor.Show();
    }

    string uploadPathPlayerPrefs = "uploadPath-Editor";
    string innerLuaPathPlayerPrefs = "innerLuaPath-Editor";
    string gameLuaPathPlayerPrefs = "gameLuaPath-Editor";
    string uiBuildTargetPlayerPrefs = "uiBuildTarget-Editor";
    void Awake()
    {
        uploadPath = PlayerPrefs.GetString(uploadPathPlayerPrefs);
        innerLuaPath = PlayerPrefs.GetString(innerLuaPathPlayerPrefs);
        gameLuaPath = PlayerPrefs.GetString(gameLuaPathPlayerPrefs);
        string tempBuildTarget = PlayerPrefs.GetString(uiBuildTargetPlayerPrefs);
        if (!string.IsNullOrEmpty(tempBuildTarget))
            uiBuildTarget = (BuildTarget)(System.Convert.ToInt32(tempBuildTarget));
    }

    string uploadPath = string.Empty;
    string innerLuaPath = string.Empty;
    string gameLuaPath = string.Empty;
    BuildTarget uiBuildTarget = BuildTarget.Android;
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 30), "压缩的UI和Lua资源保存路径：");
        uploadPath = GUI.TextField(new Rect(175, 10, 300, 20), uploadPath);
        if (GUI.Button(new Rect(490, 10, 80, 20), "设置"))
        {
            uploadPath = EditorUtility.OpenFolderPanel("压缩的UI和Lua资源保存路径", "", "");
        }

        GUI.Label(new Rect(10, 45, 150, 30), "内部lua资源路径：");
        innerLuaPath = GUI.TextField(new Rect(175, 45, 300, 20), innerLuaPath);
        if (GUI.Button(new Rect(490, 45, 80, 20), "设置"))
        {
            innerLuaPath = EditorUtility.OpenFolderPanel("内部lua资源路径", "", "");
        }

        GUI.Label(new Rect(10, 75, 150, 30), "游戏lua资源路径：");
        gameLuaPath = GUI.TextField(new Rect(175, 75, 300, 20), gameLuaPath);
        if (GUI.Button(new Rect(490, 75, 80, 20), "设置"))
        {
            gameLuaPath = EditorUtility.OpenFolderPanel("游戏lua资源路径", "", "");
        }

        GUI.Label(new Rect(10, 110, 120, 30), "Build目标平台：");
        uiBuildTarget = (BuildTarget)EditorGUI.EnumPopup(new Rect(175, 110, 80, 20), uiBuildTarget);

        if (GUI.Button(new Rect(230, 140, 80, 20), "打包"))
        {
            if (!Directory.Exists(uploadPath))
            {
                ShowNotification(new GUIContent("压缩的UI和Lua资源保存路径不存在"));
                return;
            }
            if (!Directory.Exists(innerLuaPath))
            {
                ShowNotification(new GUIContent("lua原始资源路径不存在"));
                return;
            }
            if (!Directory.Exists(gameLuaPath))
            {
                ShowNotification(new GUIContent("游戏lua资源路径不存在"));
                return;
            }
            PlayerPrefs.SetString(uploadPathPlayerPrefs, uploadPath);
            PlayerPrefs.SetString(innerLuaPathPlayerPrefs, innerLuaPath);
            PlayerPrefs.SetString(gameLuaPathPlayerPrefs, gameLuaPath);
            PlayerPrefs.SetString(uiBuildTargetPlayerPrefs, ((int)uiBuildTarget).ToString());
            BuildAssetResource(uiBuildTarget, uploadPath, innerLuaPath, gameLuaPath);
        }
    }

    string manifestName = "UI";
    public void BuildAssetResource(BuildTarget tTarget, string tUploadPath, string tInnerLuaPath, string tGameLuaPath)
    {
        string uiABSpawnPath = Application.dataPath.Replace("/Assets", "/" + manifestName);
        Debug.Log("生成UI-ab包的临时目录：" + uiABSpawnPath);
        Debug.Log("lua原始资源路径：" + tInnerLuaPath);
        Debug.Log("游戏lua资源路径：" + tGameLuaPath);
        Debug.Log("压缩的UI和Lua资源保存路径：" + tUploadPath);
        //清除旧的资源
        {
            if (Directory.Exists(uiABSpawnPath))
                Directory.Delete(uiABSpawnPath, true);
            if (Directory.Exists(tUploadPath))
                Directory.Delete(tUploadPath, true);
        }
        //不加{}，无法创建目录
        {
            Directory.CreateDirectory(uiABSpawnPath);
            Directory.CreateDirectory(tUploadPath);
        }

        //生成UI的AB包
        BuildPipeline.BuildAssetBundles(uiABSpawnPath, BuildAssetBundleOptions.UncompressedAssetBundle, tTarget);

        List<string> md5List = StartCompressFile(uiABSpawnPath, tUploadPath);
        List<string> md5List1 = StartCompressFile(tInnerLuaPath, tUploadPath);
        List<string> md5List2 = StartCompressFile(tGameLuaPath, tUploadPath);
        md5List.AddRange(md5List1);
        md5List.AddRange(md5List2);

        ////生成内部lua的压缩包
        //List<string> allInnerLuaFiles = new List<string>();
        //Recursive(tInnerLuaPath, allInnerLuaFiles);
        //allInnerLuaFiles = allInnerLuaFiles.FindAll(it => { return !it.EndsWith(".meta"); });
        //string rootName = tInnerLuaPath.Substring(tInnerLuaPath.LastIndexOf("/"));
        //string zipName = tUploadPath + rootName + ".zip";
        //long size = compressFiles(zipName, allInnerLuaFiles, tInnerLuaPath.Length, rootName);
        //md5List.Add(Path.GetFileName(zipName) + "|" + Utils.FileMD5(zipName) + "|" + size);

        ////生成游戏Lua的压缩包
        //List<string> allGameLuaFiles = new List<string>();
        //Recursive(tGameLuaPath, allGameLuaFiles);
        //allGameLuaFiles = allGameLuaFiles.FindAll(it => { return !it.EndsWith(".meta"); });
        //rootName = tGameLuaPath.Substring(tGameLuaPath.LastIndexOf("/"));
        //zipName = tUploadPath + rootName + ".zip";
        //size = compressFiles(tUploadPath + rootName + ".zip", allGameLuaFiles, tGameLuaPath.Length, rootName);
        //md5List.Add(Path.GetFileName(zipName) + "|" + Utils.FileMD5(zipName) + "|" + size);

        //生成files.list文件
        SpawnMD5File(md5List, tUploadPath);

        AssetDatabase.Refresh();
        System.Diagnostics.Process.Start(tUploadPath);
    }

    List<string> StartCompressFile(string orignalPath, string targetPath)
    {
        int stringLengthOfOrignalPath = orignalPath.Length;
        string rootName = orignalPath.Substring(orignalPath.LastIndexOf("/"));
        targetPath += rootName;
        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        List<string> allOriganlFiles = new List<string>();
        Recursive(orignalPath, allOriganlFiles);

        List<string> md5List = new List<string>();
        foreach (var item in allOriganlFiles)
        {
            if (item.EndsWith(".meta") || item.EndsWith(".manifest") || item.EndsWith(manifestName)) continue;
            string fileFloderStruct = item.Substring(stringLengthOfOrignalPath);
            string zipFile = targetPath + fileFloderStruct;
            string itemDir = Path.GetDirectoryName(zipFile);
            if (!Directory.Exists(itemDir)) Directory.CreateDirectory(itemDir);

            string fileName = rootName + "/" + fileFloderStruct;
            string md5 = CSharpUtils.FileMD5(item);
            long size = compressFile(zipFile, item, fileName);
            md5List.Add(fileName + "|" + md5 + "|" + size);
        }
        return md5List;
    }

    void SpawnMD5File(List<string> md5List, string targetPath)
    {
        string filesPath = Path.Combine(targetPath, "files.list");
        if (File.Exists(filesPath)) File.Delete(filesPath);

        using (FileStream fs = new FileStream(filesPath, FileMode.CreateNew))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(string.Join("\n", md5List.ToArray()));
            }
        }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path, List<string> result)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            result.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            Recursive(dir, result);
        }
    }

    static long compressFile(string zipFile, string sourcefile, string fileName)
    {
        long size = 0;
        using (Stream s = File.OpenWrite(zipFile))
        {
            using (var ws = SharpCompress.Writer.WriterFactory.Open(s, SharpCompress.Common.ArchiveType.Zip, SharpCompress.Common.CompressionType.Deflate))
            {
                ws.Write(fileName, File.OpenRead(sourcefile), null);
            }
            size = s.Length;
        }
        return size;
    }

    static long compressFiles(string zipFile, List<string> files, int startPos, string profix)
    {
        long size = 0;
        using (Stream s = File.OpenWrite(zipFile))
        {
            using (var ws = SharpCompress.Writer.WriterFactory.Open(s, SharpCompress.Common.ArchiveType.Zip, SharpCompress.Common.CompressionType.Deflate))
            {
                foreach (var item in files)
                {
                    ws.Write(profix + item.Substring(startPos), File.OpenRead(item), null);
                }
            }
            size = s.Length;
        }
        return size;
    }
}