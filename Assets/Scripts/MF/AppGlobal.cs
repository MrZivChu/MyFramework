using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppGlobal
{
    public static VersionDepot VersionDepot { get; private set; }

    public static void Start()
    {
        if (VersionDepot == null)
        {
            VersionDepot = new VersionDepot();
            VersionDepot.Load();
        }
    }
}
