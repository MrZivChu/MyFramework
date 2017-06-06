#region Copyright
/*
 * ----------------------------------------------------------------------
 * Copyright (C) 2015-2017 shenjianjiang
 * bonny@tonegames.com
 * All rights reserved
 * ----------------------------------------------------------------------
 *    filename: GrayImage
 *      author: shenjianjiang
 *        time: 11/28/2016 4:19:01 PM
 * description: 
 */
#endregion

using System;
using UnityEngine;
using UnityEngine.UI;

public class GrayImage : Image {
    [SerializeField]
    private bool isGray = false;

    private Material grayMat = null;
    private Material sourceMat = null;

    //关于修改变灰图片的操作,参考 http://www.cnblogs.com/yaukey/p/unity-ui-mask-override-children-image-material.html
    //unity answer:http://answers.unity3d.com/questions/1130203/ui-mask-override-my-shaders-custom-property.html
    public override Material GetModifiedMaterial(Material baseMaterial) {
        Material modifiedMat = base.GetModifiedMaterial(baseMaterial);
        if (isGray) {
            
        }
        return modifiedMat;
    }
}