using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class HandlePage {
    int index = 0;

    public void AddRootPage(Transform parent, List<InspectorObj> list) {
        InspectorObj inspectorObj = new InspectorObj();
        inspectorObj.ID = index;
        inspectorObj.obj = parent.gameObject;
        list.Add(inspectorObj);
        GetPageChildsKeyValue(parent, list);
    }

    public void GetPageChildsKeyValue(Transform parent, List<InspectorObj> list) {
        InspectorObj inspectorObj = null;
        foreach (Transform item in parent) {
            index++;
            inspectorObj = new InspectorObj();
            inspectorObj.ID = index;
            inspectorObj.obj = item.gameObject;
            list.Add(inspectorObj);
            if (item.childCount > 0) {
                GetPageChildsKeyValue(item, list);
            }
        }
    }
}

[CustomEditor(typeof(InspectorObjectsHelper))]
public class DecoratorEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Inspector Objects")) {
            List<InspectorObj> allObjectsDic = new List<InspectorObj>();
            InspectorObjectsHelper bindAllObjects = (InspectorObjectsHelper)target;
            HandlePage handlePage = new HandlePage();
            handlePage.AddRootPage(bindAllObjects.gameObject.transform, allObjectsDic);
            bindAllObjects.allInspectorObjects = allObjectsDic;
        }
    }
}
