using UnityEngine;
using UnityEngine.UI;

public class SceneCanvasStart : MonoBehaviour {

    // Use this for initialization
    void Start() {
        
    }

    void OnApplicationPause(bool paused) {
        if (!paused) {
            setWidthScale();
        }
    }

    private int scaleWidth = 0;
    private int scaleHeight = 0;
    
    void setWidthScale() {
        if (scaleWidth == 0) {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            float w = (rt.offsetMax.x - rt.offsetMin.x);
            float h = (rt.offsetMax.y - rt.offsetMin.y);
            float s = w / h;
            scaleWidth = Mathf.FloorToInt(640 * s);
            scaleHeight = 640;
            if (scaleWidth % 2 == 1) scaleWidth--;
        }
        Screen.SetResolution(scaleWidth, scaleHeight, true);
        Debug.Log("Result: Width=" + scaleWidth + ":Height=" + scaleHeight);
    }
}
