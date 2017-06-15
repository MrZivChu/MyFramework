using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LuaUtils 
{
    public static void LoadLevel(int index)
    {
        Loading.index = index;
        SceneManager.LoadScene(1);
    }
}
