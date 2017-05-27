using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectsHelper
{

    static Dictionary<int, List<GameObject>> allObjectsDic = new Dictionary<int, List<GameObject>>();

    public static int SpawnPage(string pageName)
    {
        GameObject page = GameObject.Instantiate(Resources.Load(pageName)) as GameObject;
        int pageID = page.GetInstanceID();
        allObjectsDic[pageID] = page.GetComponent<InspectorObjectsHelper>().allInspectorObjects;
        return pageID;
    }

    public static void SetPageNull(int pageID)
    {
        if (allObjectsDic.ContainsKey(pageID))
        {
            allObjectsDic[pageID] = null;
            allObjectsDic.Remove(pageID);
        }
    }



    public static void SetText(int parentID, int childID, string content)
    {
        GameObject obj = allObjectsDic[parentID][childID];
        if (obj != null)
        {
            Text uiText = obj.GetComponent<Text>();
            if (uiText != null)
            {
                uiText.text = content;
            }
        }
    }

    public static void SetImage(int parentID, int childID, string tSpriteName)
    {
        GameObject obj = allObjectsDic[parentID][childID];
        Sprite sprite = null;
        if (obj != null)
        {
            Image uiImage = obj.GetComponent<Image>();
            if (uiImage != null)
            {
                uiImage.sprite = sprite;
            }
        }
    }

    //public static void SetUISlider(string parentID, string childPath, float value)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        UISlider uiSlider = obj.GetComponent<UISlider>();
    //        if (uiSlider != null)
    //        {
    //            uiSlider.value = value;
    //        }
    //    }
    //}

    //public static void SetGameObjectActive(string parentID, string childPath, bool isActive)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        obj.SetActive(isActive);
    //    }
    //}

    //public static void RegisterClick(string parentID, string childPath, object luaFunc)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        LuaHelper.AddClick(obj, luaFunc);
    //    }
    //}
    //public static void RegisterClickParam(string parentID, string childPath, object luaFunc, object luaParam)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        LuaHelper.AddClick(obj, luaFunc, luaParam);
    //    }
    //}
}
