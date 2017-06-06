#region Copyright
/*
 * ----------------------------------------------------------------------
 *  Copyright (C) 2015-2017 图观网络科技
 *  http://www.tonegames.com
 *  All rights reserved
 * ----------------------------------------------------------------------
 * 
 *     filename: DynamicHide
 *       author: shenjianjiang
 *         time: 6/1/2016 5:25:39 PM
 *  description: 
 */
#endregion

using UnityEngine;
using System;
using System.Collections.Generic;

public class DynamicHide : MonoBehaviour {
    public List<GameObject> hideList = new List<GameObject>();

    /// <summary>
    /// 更新显示
    /// </summary>
    /// <param name="display"></param>
    public void UpdateDisplay(bool display) {
        int count = hideList.Count;
        for (int i = 0; i < count; i++) {
            if (hideList[i]) {
                hideList[i].SetActive(display);
            }
        }
    }
}
