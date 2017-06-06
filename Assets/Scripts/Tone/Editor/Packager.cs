using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
//using System.Diagnostics;
using SharpCompress;
using SharpCompress.Common;
using SharpCompress.Archive.Zip;

public static class Packager {
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    /// <summary>
    /// 载入素材
    /// </summary>
    static UnityEngine.Object LoadAsset(string file) {
        if (file.EndsWith(".lua")) file += ".txt";
        return AssetDatabase.LoadMainAssetAtPath("Assets/Builds/" + file);
    }

    [MenuItem("Game/Build iPhone Resource", false, 11)]
    public static void BuildiPhoneResource() { 
        BuildTarget target;
        target = BuildTarget.iOS;
        BuildAssetResource(target, false);
    }

    [MenuItem("Game/Build Android Resource _F3", false, 12)]
    public static void BuildAndroidResource() {
        BuildAssetResource(BuildTarget.Android, true);
    }

    [MenuItem("Game/Build Windows Resource", false, 13)]
    public static void BuildWindowsResource() {
        BuildAssetResource(BuildTarget.StandaloneWindows, true);
    }

    /// <summary>
    /// 生成绑定素材
    /// </summary>
	public static void BuildAssetResource(BuildTarget target, bool isWin) {
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];

        string tempPath = "";
        //if (projectName == "M2AssetBundles") {
        if (Application.dataPath.Contains("M2AssetBundles/")) {
            tempPath = Application.dataPath + "/../M2Gui/";
        } else {
            tempPath = Application.dataPath + "/../" + projectName + "/";
        }
        string uploadPath = Application.dataPath + "/../Upload/";
        List<string> list = new List<string>();
        FileStream fs;
        int subPos;

        Debug.Log("Start to BuildAssetResources to " + uploadPath);
        //清除旧的资源
        if (Directory.Exists(tempPath)) {
            //Directory.Delete(tempPath, true);
        } else {
            Directory.CreateDirectory(tempPath);
        }

        if (Directory.Exists (uploadPath)) {
			//Directory.Delete (uploadPath, true);
		} else {
			Directory.CreateDirectory (uploadPath);
		}

//        if (Directory.Exists (Application.streamingAssetsPath)) {
//			//Directory.Delete (Application.streamingAssetsPath, true);
//		} else {
//			Directory.CreateDirectory (Application.streamingAssetsPath);
//		}

		//记录时间，晚于这个时间的需要压缩
		System.DateTime dt = System.DateTime.Now;

        //打包AssetBundles
        BuildPipeline.BuildAssetBundles(tempPath, BuildAssetBundleOptions.UncompressedAssetBundle, target);

        paths.Clear(); files.Clear();
        Recursive(tempPath);
        subPos = tempPath.Length;

        //为热更压缩AssetBundles
        foreach (var item in files) {
			if (item.EndsWith("manifest") || item.EndsWith("M2Gui") || item.StartsWith(".")) continue;

            FileInfo fi = new FileInfo(item);
			bool timeout = fi.Exists && fi.LastWriteTime >= dt;

			string zipFile = uploadPath + item.Substring(subPos);
			string path = Path.GetDirectoryName(uploadPath + item.Substring(subPos));
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			fi = new FileInfo(zipFile);
			if (timeout || !fi.Exists) {
                fi.Delete();
				string md5 = Utils.FileMD5(item);
				long size = compressFile(zipFile, item, item.Substring(subPos));
				list.Add(item.Substring(subPos) + "|" + md5 + "|" + size);
			} else {
				string md5 = Utils.FileMD5(item);
				long size = fi.Length;
				list.Add(item.Substring(subPos) + "|" + md5 + "|" + size);
			}
        }

        //生成文件列表
        string filesPath = Path.Combine(uploadPath, "files.list");
        if (File.Exists(filesPath)) File.Delete(filesPath);

        using (fs = new FileStream(filesPath, FileMode.CreateNew)) {
            using (StreamWriter sw = new StreamWriter(fs)) {
                sw.Write(string.Join("\n", list.ToArray()));
                sw.Close();
            }
            fs.Close();
        }

        AssetDatabase.Refresh();
		Debug.Log ("It's done.");
		System.Diagnostics.Process.Start(@"F:\m2_asset\06 UnityProjects\M2AssetBundles\copyAB.bat");
    }

    /// <summary>
    /// 数据目录
    /// </summary>
    static string AppDataPath {
        get { return Application.dataPath.ToLower(); }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path) {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names) {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta") || ext.Equals(".DS_Store")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs) {
            if (dir.EndsWith(".svn")) continue;
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    static long compressFile(string zipFile, string sourcefile, string entry) {
        long size = 0;
        using (Stream s = File.OpenWrite(zipFile)) {
            using (var ws = SharpCompress.Writer.WriterFactory.Open(s, SharpCompress.Common.ArchiveType.Zip, SharpCompress.Common.CompressionType.Deflate)) {
                ws.Write(entry, File.OpenRead(sourcefile), null);
            }
            size = s.Length;
            s.Flush();
            s.Close();
        }
        return size;
    }

    static long compressFiles(string zipFile, List<string> files, int startPos, string profix) {
        long size = 0;
        using (Stream s = File.OpenWrite(zipFile)) {
            using (var ws = SharpCompress.Writer.WriterFactory.Open(s, SharpCompress.Common.ArchiveType.Zip, SharpCompress.Common.CompressionType.Deflate)) {
                foreach (var item in files) {
                    ws.Write(profix + item.Substring(startPos), File.OpenRead(item), null);
                }
            }
            size = s.Length;
            s.Flush();
            s.Close();
        }
        return size;
    }

    static void UpdateProgress(int progress, int progressMax, string desc) {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
}