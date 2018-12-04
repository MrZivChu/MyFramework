using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Children
{
    public string path;
    public GameObject go;
}

public class ChildrenHelper : MonoBehaviour
{
    public List<Children> list = new List<Children>();
    public Dictionary<string, GameObject> dic = null;
    private void Awake()
    {
        dic = new Dictionary<string, GameObject>();
        if (list != null && list.Count > 0)
        {
            Children children = null;
            for (int i = 0; i < list.Count; i++)
            {
                children = list[i];
                if (!dic.ContainsKey(children.path))
                {
                    dic.Add(children.path, children.go);
                }
            }
        }
    }

    public GameObject GetHieChild(string path)
    {
        if (dic != null && dic.Count > 0)
        {
            if (dic.ContainsKey(path))
                return dic[path];
        }
        return null;
    }
}
