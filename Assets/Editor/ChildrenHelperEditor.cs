using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChildrenHelper))]
public class ChildrenHelperEditor : Editor
{
    public List<Children> list = null;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("更新孩子节点"))
        {
            list = new List<Children>();
            ChildrenHelper childrenHelper = (ChildrenHelper)target;
            GetChildren(childrenHelper.gameObject);
            childrenHelper.list = list;
        }
    }


    public void GetChildren(GameObject go)
    {
        foreach (Transform item in go.transform)
        {
            Children children = new Children();
            temPath = string.Empty;
            children.path = GetObjPath(item.gameObject);
            children.go = item.gameObject;
            list.Add(children);
            GetChildren(item.gameObject);
        }
    }

    string temPath = string.Empty;
    public string GetObjPath(GameObject go)
    {
        temPath = go.name + "/" + temPath;
        if (go.transform.parent != null && go.transform.parent.name != "Canvas")
        {
            GetObjPath(go.transform.parent.gameObject);
        }
        return temPath.TrimEnd('/');
    }
}
