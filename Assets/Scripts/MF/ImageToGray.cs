using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageToGray : MonoBehaviour
{
    Component[] imagelist;
    Component[] textlist;
    Color[] colorlist;
    Material material;
    void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        if (material == null)
        {
            material = Resources.Load("Shaders/ImageToGray") as Material;
            imagelist = gameObject.transform.GetComponentsInChildren(typeof(Image), true);
            textlist = gameObject.transform.GetComponentsInChildren(typeof(Text), true);
            colorlist = new Color[textlist.Length];
        }
    }

    public void ChangeGray()
    {
        InitData();
        if (material != null)
        {
            material.SetColor("_MyColor", new Color(191f / 255, 191f / 255, 187f / 255, 1));
        }
        if (imagelist != null)
        {
            for (int i = 0; i < imagelist.Length; i++)
            {
                imagelist[i].GetComponent<Image>().material = material;
            }
        }
        if (textlist != null)
        {
            for (int i = 0; i < textlist.Length; i++)
            {
                Text t = textlist[i].GetComponent<Text>();
                colorlist[i] = t.color;
                t.color = new Color(0.78f, 0.78f, 0.78f);
            }
        }
    }

    public void SetColor(Color c)
    {
        material.SetColor("_MyColor", c);
    }

    public void Recovery()
    {
        if (imagelist != null)
        {
            for (int i = 0; i < imagelist.Length; i++)
            {
                imagelist[i].GetComponent<Image>().material = null;
            }
        }
        if (textlist != null)
        {
            for (int i = 0; i < textlist.Length; i++)
            {
                textlist[i].GetComponent<Text>().color = colorlist[i];
            }
        }
    }
}
