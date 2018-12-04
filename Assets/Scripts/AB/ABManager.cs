using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManager
{
    Dictionary<string, AssetBundle> hasLoadedABList = new Dictionary<string, AssetBundle>();
    Dictionary<AssetBundle, int> abCounter = new Dictionary<AssetBundle, int>();

    public AssetBundle GetAB(string abName)
    {
        AssetBundle result = null;
        List<string> dependenciesList = GetGetDependencies(abName);
        if (dependenciesList != null && dependenciesList.Count > 0)
        {
            AssetBundle itemAB = null;
            for (int i = 0; i < dependenciesList.Count; i++)
            {
                itemAB = LoadAB(dependenciesList[i]);
                if (i == 0)
                    result = itemAB;
            }
        }
        return result;
    }

    public void UnLoadAB(string abName, bool unloadAllLoadedObjects)
    {
        if (hasLoadedABList.ContainsKey(abName))
        {
            hasLoadedABList[abName].Unload(unloadAllLoadedObjects);
            abCounter[hasLoadedABList[abName]]--;
            ABRecovery();
        }
    }

    AssetBundle LoadAB(string abName)
    {
        if (!hasLoadedABList.ContainsKey(abName))
        {
            AssetBundle result = AssetBundle.LoadFromFile(abName);
            hasLoadedABList[abName] = result;
        }
        AssetBundle ab = hasLoadedABList[abName];
        if (!abCounter.ContainsKey(ab))
            abCounter.Add(ab, 0);
        abCounter[ab]++;
        return hasLoadedABList[abName];
    }

    void ABRecovery()
    {
        if (abCounter != null && abCounter.Count > 0)
        {
            foreach (var item in abCounter)
            {
                int count = item.Value;
                if (count <= 0)
                {
                    item.Key.Unload(true);
                    abCounter.Remove(item.Key);
                }
            }
        }
    }

    List<string> GetGetDependencies(string abName)
    {
        List<string> list = new List<string>() { abName };
        if (abName.EndsWith(".ab"))
            list.Add(abName.Replace(".ab", ".tex"));
        return list;
    }

}
