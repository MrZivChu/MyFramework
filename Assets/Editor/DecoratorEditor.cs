using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class HandlePrefabEditorHelper
{
    public string podaFileSavePath = "d:/{0}_pda.lua";

    Transform parent;
    public HandlePrefabEditorHelper(Transform tparent)
    {
        parent = tparent;
    }

    public void CreatePdaFile()
    {
        string content = "local tab = { \r\t";
        Dictionary<string, GameObject> dic = GetFlagChilds(parent);
        int index = 0;
        string temp = string.Empty;
        foreach (var item in dic)
        {
            temp = ",\r\t";
            if (index == dic.Count - 1)
            {
                temp = "\r";
            }
            content += "[\"" + item.Key + "\"] = " + index + temp;
            index++;
        }
        content += "}\rreturn tab";
        string fileName = string.Format(podaFileSavePath, parent.name);
        File.WriteAllText(fileName, content);
        System.Diagnostics.Process.Start(Path.GetDirectoryName(fileName));
    }

    public void BindScriptForPrefab()
    {
        Dictionary<string, GameObject> dic = GetFlagChilds(parent);
        InspectorObjectsHelper helper = parent.GetComponent<InspectorObjectsHelper>();
        helper.allInspectorObjects = new List<GameObject>();
        foreach (var item in dic)
        {
            helper.allInspectorObjects.Add(item.Value);
        }
    }

    Dictionary<string, GameObject> GetFlagChilds(Transform parent)
    {
        Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();
        dic[parent.name] = parent.gameObject;
        GetPageChildsKeyValue(parent, dic);
        return dic;
    }

    void GetPageChildsKeyValue(Transform parent, Dictionary<string, GameObject> dic)
    {
        foreach (Transform item in parent)
        {
            if (item.name.StartsWith("_"))
            {
                dic[item.name.Substring(1, item.name.Length - 1)] = item.gameObject;
            }
            if (item.childCount > 0)
            {
                GetPageChildsKeyValue(item, dic);
            }
        }
    }
}

[CustomEditor(typeof(InspectorObjectsHelper))]
public class DecoratorEditor : Editor
{
    //在预设中需要用到的节点需要在前面加_
    //生成一个poda文件，里面存一个集合    Name -> ID
    //预设上绑一个脚本，脚本里存一个集合  ID -> Obj
    //备注：ID为集合的索引，Name必须是唯一的(一个好处就是强制命名是有意义的)
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Child Objects"))
        {
            InspectorObjectsHelper bindAllObjects = (InspectorObjectsHelper)target;
            HandlePrefabEditorHelper handlePage = new HandlePrefabEditorHelper(bindAllObjects.transform);
            handlePage.CreatePdaFile();
            handlePage.BindScriptForPrefab();
        }
    }
}
