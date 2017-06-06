#region Copyright
/*
 * ----------------------------------------------------------------------
 *  Copyright (C) 2015-2017 图观网络科技
 *  http://www.tonegames.com
 *  All rights reserved
 * ----------------------------------------------------------------------
 * 
 *     filename: UIRawImage
 *       author: shenjianjiang
 *         time: 7/4/2016 12:00:10 PM
 *  description: 
 */
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIRawImage : MonoBehaviour {
    //修改rawimage的yv
    //[SerializeField]
    public float XSpeed = 0;
    //[SerializeField]
    public float YSpeed = 0;

    private RawImage image;

    void Start() {
        image = gameObject.GetComponent<RawImage>();
    }

    void FixedUpdate() {
        if (XSpeed != 0 || YSpeed != 0) {
            Rect rect = image.uvRect;
            rect.x += XSpeed * Time.fixedDeltaTime;
            rect.y += YSpeed * Time.fixedDeltaTime;

            if (rect.x > 1) rect.x = rect.x - 1;
            if (rect.y > 1) rect.y = rect.y - 1;

            image.uvRect = rect;
        }
    }

    public void UpdateTexture(Texture2D tex) {
        gameObject.GetComponent<RawImage>().texture = tex;
    }

    public void ChangeSpeed(float x, float y) {
        XSpeed = x;
        YSpeed = y;
    }
}
