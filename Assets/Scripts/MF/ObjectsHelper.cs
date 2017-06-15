using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

public class ObjectsHelper
{

    public static Dictionary<int, List<GameObject>> allObjectsDic = new Dictionary<int, List<GameObject>>();

    public static int SpawnPage(int parentID, int childID, string abName, string assetName)
    {
        GameObject parent = null;
        int pageID = -1;
        if (parentID == 0)
        {
            parent = GameObject.Find("GUI/Canvas/Root");
        }
        else
        {
            parent = GetGameObject(parentID, childID);
        }
        if (parent)
        {
            GameObject page = GameObject.Instantiate(Resources.Load(assetName)) as GameObject;
            if (page)
            {
                pageID = page.GetInstanceID();
                InspectorObjectsHelper inspectorObjectsHelper = page.GetComponent<InspectorObjectsHelper>();
                if (inspectorObjectsHelper)
                {
                    allObjectsDic[pageID] = inspectorObjectsHelper.allInspectorObjects;
                }
                page.transform.parent = parent.transform;
                page.transform.localScale = Vector3.one;
            }
        }
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


    public static GameObject GetGameObject(int parentID, int childID)
    {
        GameObject go = null;
        if (allObjectsDic.ContainsKey(parentID))
        {
            List<GameObject> list = allObjectsDic[parentID];
            go = list[childID];
        }
        return go;
    }

    public static void SetText(int parentID, int childID, string content)
    {
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            Text uiText = obj.GetComponent<Text>();
            if (uiText != null)
            {
                uiText.text = content;
            }
        }
    }

    public static string GetText(int parentID, int childID)
    {
        string content = string.Empty;
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            Text uiText = obj.GetComponent<Text>();
            if (uiText != null)
            {
                content = uiText.text;
            }
        }
        return content;
    }

    public static void SetImage(int parentID, int childID, string spriteName)
    {
        GameObject obj = GetGameObject(parentID, childID);
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

    public static void SetSlider(int parentID, int childID, float value)
    {
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            Slider uiSlider = obj.GetComponent<Slider>();
            if (uiSlider != null)
            {
                uiSlider.value = value;
            }
        }
    }

    public static float GetSlider(int parentID, int childID)
    {
        float value = 0;
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            Slider uiSlider = obj.GetComponent<Slider>();
            if (uiSlider != null)
            {
                value = uiSlider.value;
            }
        }
        return value;
    }

    public static void SetScrollbar(int parentID, int childID, float value)
    {
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            Scrollbar uiScrollbar = obj.GetComponent<Scrollbar>();
            if (uiScrollbar != null)
            {
                uiScrollbar.value = value;
            }
        }
    }

    public static float GetScrollbar(int parentID, int childID)
    {
        float value = 0;
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            Scrollbar uiScrollbar = obj.GetComponent<Scrollbar>();
            if (uiScrollbar != null)
            {
                value = uiScrollbar.value;
            }
        }
        return value;
    }

    public static void SetInputField(int parentID, int childID, string value)
    {
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            InputField uiInputField = obj.GetComponent<InputField>();
            if (uiInputField != null)
            {
                uiInputField.text = value;
            }
        }
    }

    public static string GetInputField(int parentID, int childID)
    {
        string content = string.Empty;
        GameObject obj = GetGameObject(parentID, childID);
        if (obj != null)
        {
            InputField uiInputField = obj.GetComponent<InputField>();
            if (uiInputField != null)
            {
                content = uiInputField.text;
            }
        }
        return content;
    }


    public static void AddButtonClick(int parentID, int childID, LuaFunction func)
    {
        GameObject obj = GetGameObject(parentID, childID);
        Button btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            func.Call();
        });
    }

    public static void AddToggleClick(int parentID, int childID, LuaFunction func)
    {
        GameObject obj = GetGameObject(parentID, childID);
        Toggle tog = obj.GetComponent<Toggle>();
        tog.onValueChanged.AddListener(delegate (bool isOn)
        {
            func.Call(isOn);
        });
    }

    public static void SetToggleGroup(int parentID, int childID, int mparentID, int mchildID)
    {
        GameObject obj = GetGameObject(parentID, childID);
        GameObject child = GetGameObject(mparentID, mchildID);
        if (obj && child)
        {
            Toggle toggle = child.GetComponent<Toggle>();
            ToggleGroup toggleGroup = obj.GetComponent<ToggleGroup>();
            if (toggle && toggleGroup)
            {
                toggle.group = toggleGroup;
            }
        }
    }

    public static void SetObjIsActive(int parentID, int childID, int flag)
    {
        GameObject obj = GetGameObject(parentID, childID);
        obj.SetActive(flag > 0);
    }

    public static void SetSortOrder(int parentID, int childID, int torder)
    {
        GameObject page = GetGameObject(parentID, childID);
        if (page == null)
        {
            return;
        }

        GraphicRaycaster gr = page.GetComponent<GraphicRaycaster>();
        if (gr == null)
        {
            gr = page.gameObject.AddComponent<GraphicRaycaster>();
        }

        int order = 0;
        if (torder > 0)
        {
            order = torder;
        }
        else
        {
            int index = page.transform.GetSiblingIndex();
            for (int i = index - 1; i >= 0; --i)
            {
                var child = page.transform.parent.GetChild(index - 1);
                var canvas = child.GetComponent<Canvas>();
                if (canvas != null)
                {
                    if (canvas.sortingOrder < 300)
                    {
                        order = canvas.sortingOrder + (index - i) * 10;
                        break;
                    }
                }
            }
        }

        Canvas[] children = page.GetComponentsInChildren<Canvas>(true);
        foreach (var item in children)
        {
            item.overrideSorting = true;
            item.sortingOrder += order;
        }

        ParticleSystemRenderer[] renders = page.GetComponentsInChildren<ParticleSystemRenderer>(true);
        foreach (var item in renders)
        {
            if (item.sortingOrder < 10)
            {
                item.sortingOrder += order;
            }
        }
    }

    public static void SetIsReceiveClick(int parentID, int childID, int isCanClick)
    {
        GameObject go = GetGameObject(parentID, childID);
        if (go == null) return;
        if (isCanClick > 0)
        {
            GraphicRaycaster comp = go.GetComponent<GraphicRaycaster>();
            if (comp != null)
            {
                comp.enabled = true;
            }
        }
        else
        {
            GraphicRaycaster comp = go.GetComponent<GraphicRaycaster>();
            if (comp == null)
            {
                comp = go.AddComponent<GraphicRaycaster>();
            }
            comp.enabled = false;
        }
    }
}
