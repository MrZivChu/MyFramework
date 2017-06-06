using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class AutoBind : MonoBehaviour
{

    private static string ExportPath = Application.dataPath + "/../";

    //[MenuItem("Game/Auto Bind Components/Scene", false, 31)]
    //public static void AutoBindInScene() {
    //
    //}

    [MenuItem("Game/Auto Bind Components/Login", false, 51)]
    public static void AutoBindLogin()
    {
        ExportFolder("AB/login");
    }

    [MenuItem("Game/Auto Bind Components/Common", false, 52)]
    public static void AutoBindCommon()
    {
        ExportFolder("AB/common");
    }

    [MenuItem("Game/Auto Bind Components/Char", false, 53)]
    public static void AutoBindChar()
    {
        ExportFolder("AB/char");
    }

    [MenuItem("Game/Auto Bind Components/skycity", false, 54)]
    public static void AutoBindSkyCity()
    {
        ExportFolder("AB/skycity");
    }

    [MenuItem("Game/Auto Bind Components/All", false, 81)]
    public static void AutoBindAll()
    {
        ExportFolder("AB");
    }

    [MenuItem("Assets/Auto Bind This Floder Components")]
    private static void ToThisFloder()
    {
        foreach (Object obj in Selection.objects)
        {
            if (AssetDatabase.Contains(obj))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                Debug.Log(string.Format("{0} ({1})", path, obj.GetType()));
                ExportFolder(path);
                print(ExportPath);
                System.Diagnostics.Process.Start(ExportPath + "/AB");
            }
            else
            {
                Debug.LogWarning(string.Format("{0} is not a source asset.", obj));
            }
        }
    }

    static bool ExportFolder(string folder)
    {
        IEnumerable<string> paths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab") && path.Contains(folder));

        int index = 0;
        int count = paths.Count();
        Debug.Log(string.Format("{0} prefab(s) will be bound.", count));
        foreach (string path in paths)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            EditorUtility.DisplayProgressBar("Collect Prefab Text", "Process prefab " + obj.name, 1.0f * index / count);
            GameObject go = Instantiate(obj) as GameObject;
            ExportScript(path.Substring(6), go);
            DestroyImmediate(go);
            index++;
        }
        Debug.Log("AutoBind is completed.");
        EditorUtility.ClearProgressBar();
        return true;
    }

    //导出GameObject中所有可以绑定的组件
    static bool ExportScript(string path, GameObject go)
    {
        try
        {
            List<Component> comps = FindComponents(go);
            ExportPoda(path, go, comps);
            ExportLua(path, comps);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(path + ":" + ex.Message);
            return false;
        }
    }

    static string Lua_Template = @"
local {0} = class('{0}', M2.Page2)

-- 允许重新加载
{0}.allowReload = true
-- 配置文件
{0}.config = 'AB/{1}/{0}'

-------------------     本地函数    --------------------------------

local llv = 5

-------------------     重载函数    --------------------------------

-- function {0}:onCreate()
-- 
-- end

-- 进入场景时被调用，会执行多次，每次从对象池中取回并加入场景时会被执行
function {0}:onEnterBegin(  )
    -- 进入场景时，关闭自动更新
	-- self.enterUpdate = false

{4}
end

-- function {0}:onMessage( sender, e, data )
--     if sender == 'Unity' then
--         return true
-- 
--     elseif sender == 'Net' then
--         return true
-- 
--     else
--         return false
--     end
-- end

-------------------     刷新函数    --------------------------------

-- 更新所有文字和图片
function {0}:onUpdateTextImage(  )
{5}
end
{2}

-------------------     事件函数    --------------------------------
{3}

-------------------     自定义函数  --------------------------------

-- function {0}:setItem( index, item )

-- end

return {0}
";
    static string Text_Template = "\t-- self:setText('{1}', '')\t\t-- 更新文本{1}";
    static string Image_Template = "\t-- self:setImage('{1}', '', 'gui/comm.tex')\t\t-- 更新图片{1}";
    static string Grid_Template = @"
-- 更新表格{1}
function {0}:onUpdate_{1}( index )
    local items = {{}}
    for i,v in ipairs(items) do
        local child = self:addChild(index, 'prefab')
        child:setItem(i, v)
        -- UITools.SetToggleGroup(self._id + index, child._id)
    end
end";
    static string Loading_Template = @"
-- 更新进度{1}
function {0}:onUpdate_{1}( index )
    UITools.SetFrameBar(self._id + index, 0, 100)
end";
    static string Button_Template = @"
-- 响应按钮{1}
function {0}:onEvent_{1}( index )
    
end";
    static string ToggleGroup_Template = @"
-- 响应切换按钮组{1}
function {0}:onToggle_{1}( index, isOn )
    if not isOn then return end

    -- self.{1} = index - self.c.firstItem.index
end";
    static string Toggle1_Template = @"
-- 响应切换按钮{1}
function {0}:onEvent_{1}( index, isOn )
    if not isOn then return end

    self:bubbling(self, '{0}.selected', {{index=self._index}})
end";
    static string Toggle2_Template = @"    self:bindEvent('{0}', self.onToggle_{1})";

    static bool ExportLua(string filename, List<Component> comps)
    {
        string fullpath = ExportPath + filename.Replace(".prefab", ".lua");
        FileInfo info = new FileInfo(fullpath);
        if (info.Directory.Exists == false)
        {
            info.Directory.Create();
        }
        string[] tokens = filename.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
        string classname = tokens[2].Substring(0, tokens[2].Length - 7);

        List<string> update = new List<string>();
        List<string> events = new List<string>();
        List<string> binds = new List<string>();
        List<string> updateAll = new List<string>();

        foreach (var item in comps)
        {
            if (item is Button)
            {
                events.Add(string.Format(Button_Template, classname, item.name));
            }
            else if (item is Toggle)
            {
                ToggleGroup group = ((Toggle)item).group;
                if (group != null)
                {
                    string name = group.name[0] == '_' ? group.name.Substring(1) : group.name;
                    binds.Add(string.Format(Toggle2_Template, item.name, name));
                }
                else {
                    events.Add(string.Format(Toggle1_Template, classname, item.transform.parent == null ? "self" : item.name));
                }
            }
            else if (item is ToggleGroup)
            {
                events.Add(string.Format(ToggleGroup_Template, classname, item.name));
            }
            else if (item is GridLayoutGroup)
            {
                update.Add(string.Format(Grid_Template, classname, item.name));
            }
            else if (item is HorizontalLayoutGroup)
            {
                update.Add(string.Format(Grid_Template, classname, item.name));
            }
            else if (item is VerticalLayoutGroup)
            {
                update.Add(string.Format(Grid_Template, classname, item.name));
            }
            else if (item is UIUnitFrame_Bar)
            {
                update.Add(string.Format(Loading_Template, classname, item.name));
            }
            //else if (item is Text)
            //{
            //    if (item.name.StartsWith("_"))
            //    {
            //        string itemname = item.name.Substring(1);
            //        updateAll.Add(string.Format(Text_Template, tokens[1], itemname));
            //    }
            //}
            //else if (item is Image)
            //{
            //    if (item.name.StartsWith("_"))
            //    {
            //        string itemname = item.name.Substring(1);
            //        updateAll.Add(string.Format(Image_Template, tokens[1], itemname));
            //    }
            //}
        }

        string contents = string.Format(
            Lua_Template,
            classname,
            tokens[1],
            string.Join("\n", update.ToArray()),
            string.Join("\n", events.ToArray()),
            string.Join("\n", binds.ToArray()),
            string.Join("\n", updateAll.ToArray()));
        System.IO.File.WriteAllText(fullpath, contents);
        return true;
    }

    static bool ExportPoda(string filename, GameObject go, List<Component> comps)
    {
        string fullpath = ExportPath + filename.Replace(".prefab", ".pda");
        FileInfo info = new FileInfo(fullpath);
        if (info.Directory.Exists == false)
        {
            info.Directory.Create();
        }
        string[] tokens = filename.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

        List<string> contents = new List<string>();
        contents.Add("ab\t\t= gui/" + (tokens[1] == "common" ? "comm" : tokens[1]) + ".ab");
        contents.Add("prefab\t= " + tokens[2].Substring(0, tokens[2].Length - 7));
        contents.Add("controls= [");

        string pattern = "\t{0}\t= {1}, {2}";
        foreach (var item in comps)
        {
            string path = GetPath(go.transform, item.transform);
            if (path == go.name) path = ".";

            if (item is Button)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "Button", path));
            }
            else if (item is Toggle)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "Toggle", path));
            }
            else if (item is ToggleGroup)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "ToggleGroup", path));
            }
            else if (item is GridLayoutGroup)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "Object", path));
            }
            else if (item is HorizontalLayoutGroup)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "Object", path));
            }
            else if (item is VerticalLayoutGroup)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "Object", path));
            }
            else if (item is UIUnitFrame_Bar)
            {
                contents.Add(string.Format(pattern, path == "." ? "self" : item.name, "UIUnitFrame_Bar", path));
            }
            //else if (item is Text)
            //{
            //    if (item.name.StartsWith("_"))
            //    {
            //        string itemname = item.name.Substring(1);
            //        contents.Add(string.Format(pattern, path == "." ? "self" : itemname, "Text", path));
            //    }
            //}
            //else if (item is Image)
            //{
            //    if (item.name.StartsWith("_"))
            //    {
            //        string itemname = item.name.Substring(1);
            //        contents.Add(string.Format(pattern, path == "." ? "self" : itemname, "Image", path));
            //    }
            //}
        }

        //RectTransform[] rtfs = go.GetComponentsInChildren<RectTransform>(true);
        //foreach (RectTransform item in rtfs)
        //{
        //    if (item.name.StartsWith("_"))
        //    {
        //        string path = GetPath(go.transform, item.transform);
        //        if (path == go.name) path = ".";
        //        string itemname = item.name.Substring(1);

        //        string itemType = "Object";
        //        if (item.GetComponent<Text>() != null)
        //        {
        //            itemType = "Text";
        //        }
        //        else if (item.GetComponent<Image>() != null)
        //        {
        //            if (item.GetComponent<InputField>() == null)
        //            {
        //                itemType = "Image";
        //            }
        //        }
        //        contents.Add(string.Format(pattern, path == "." ? "self" : itemname, itemType, path));
        //    }
        //}


        Transform[] tfs = go.GetComponentsInChildren<Transform>(true);
        foreach (Transform item in tfs)
        {
            if (item.name.StartsWith("_"))
            {
                string path = GetPath(go.transform, item.transform);
                if (path == go.name) path = ".";
                string itemname = item.name.Substring(1);

                string itemType = "Object";
                if (item.GetComponent<Text>() != null)
                {
                    itemType = "Text";
                }
                else if (item.GetComponent<Image>() != null)
                {
                    if (item.GetComponent<InputField>() == null)
                    {
                        itemType = "Image";
                    }
                }
                contents.Add(string.Format(pattern, path == "." ? "self" : itemname, itemType, path));
            }
        }

        contents.Add("]");
        System.IO.File.WriteAllLines(fullpath, contents.ToArray());
        return true;
    }

    //获取所有可以绑定的组件
    static List<Component> FindComponents(GameObject go)
    {
        List<Component> comps = new List<Component>();

        string name;
        Button[] btns = go.GetComponentsInChildren<Button>(true);
        foreach (var item in btns)
        {
            comps.Add(item);
        }

        Toggle[] tgls = go.GetComponentsInChildren<Toggle>(true);
        foreach (var item in tgls)
        {
            comps.Add(item);
        }

        ToggleGroup[] tgs = go.GetComponentsInChildren<ToggleGroup>(true);
        foreach (var item in tgs)
        {
            comps.Add(item);
        }

        GridLayoutGroup[] ggs = go.GetComponentsInChildren<GridLayoutGroup>(true);
        foreach (var item in ggs)
        {
            comps.Add(item);
        }

        HorizontalLayoutGroup[] hls = go.GetComponentsInChildren<HorizontalLayoutGroup>(true);
        foreach (var item in hls)
        {
            comps.Add(item);
        }

        VerticalLayoutGroup[] vls = go.GetComponentsInChildren<VerticalLayoutGroup>(true);
        foreach (var item in vls)
        {
            comps.Add(item);
        }

        UIUnitFrame_Bar[] bars = go.GetComponentsInChildren<UIUnitFrame_Bar>(true);
        foreach (var item in bars)
        {
            comps.Add(item);
        }

        Text[] texts = go.GetComponentsInChildren<Text>(true);
        foreach (var item in texts)
        {
            comps.Add(item);
        }

        Image[] imgs = go.GetComponentsInChildren<Image>(true);
        foreach (var item in imgs)
        {
            comps.Add(item);
        }

        return comps;
    }

    //获取组件的路径
    static string GetPath(Transform root, Transform node)
    {
        string path = node.name;
        Transform parent = node.parent;
        while (parent != null && parent.parent != null && parent != root)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }
}