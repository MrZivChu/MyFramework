using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tone.UI {
    public class FullScreenSize : MonoBehaviour {

        public int DesignWidth = 960;
        public int DesignHeight = 640;

        void Start() {
            Vector2 size = new Vector2(960, 640);

            Object canvas = GameObject.Find("GUI/Canvas");
            if (canvas != null) {
                RectTransform rt0 = transform.parent as RectTransform;
                size.x = rt0.rect.width;
                size.y = rt0.rect.height;
            }
            RectTransform rt = transform as RectTransform;
            if (rt != null) {
                rt.offsetMin = new Vector2(-size.x / 2, -size.y / 2);
                rt.offsetMax = new Vector2(size.x / 2, size.y / 2);
            }
        }
    }
}
