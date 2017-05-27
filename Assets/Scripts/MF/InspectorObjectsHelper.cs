using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class InspectorObj {
    public int ID;
    public GameObject obj;
}

public class InspectorObjectsHelper : MonoBehaviour {

    public List<InspectorObj> allInspectorObjects = new List<InspectorObj>();

}
