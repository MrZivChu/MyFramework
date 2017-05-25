using UnityEngine;
using System;
using System.Collections.Generic;


public class ObjectsHelper {

    static Dictionary<int, List<InspectorObj>> allObjectsDic = new Dictionary<int, List<InspectorObj>>();

    public static int SpawnPage(string pageName) {
        GameObject page = GameObject.Instantiate(Resources.Load(pageName)) as GameObject;
        int pageID = page.GetInstanceID();
        allObjectsDic[pageID] = page.GetComponent<InspectorObjectsHelper>().allInspectorObjects;
        return pageID;
    }

    public static void SetPageNull(int pageID) {
        if (allObjectsDic.ContainsKey(pageID)) {
            allObjectsDic[pageID] = null;
        }
    }



    //public static void SetUILabel(string parentID, string childPath, string content)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        UILabel uiLabel = obj.GetComponent<UILabel>();
    //        if (uiLabel != null)
    //        {
    //            uiLabel.text = content;
    //        }
    //    }
    //}

    //public static void SetUISprite(string parentID, string childPath, string tSpriteName)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        UISprite uiSprite = obj.GetComponent<UISprite>();
    //        if (uiSprite != null)
    //        {
    //            uiSprite.spriteName = tSpriteName;
    //        }
    //    }
    //}

    //public static void SetUITexture(string parentID, string childPath, string tMainTexture)
    //{
    //    GameObject obj = GetChildObjectByPath(parentID, childPath);
    //    if (obj != null)
    //    {
    //        UITexture uiTexture = obj.GetComponent<UITexture>();
    //        if (uiTexture != null)
    //        {
    //            uiTexture.mainTexture = XYClient.Resource.ResourceManager.LoadTexture(tMainTexture);
    //        }
    //    }
    //}

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
