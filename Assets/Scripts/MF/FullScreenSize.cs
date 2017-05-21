using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tone.UI {
    public class FullScreenSize : MonoBehaviour {
        public bool coroutine = false;//是否异步
        public int DesignWidth = 960;
        public int DesignHeight = 640;
        public string CanvasRoot = "GUI/Canvas";

        void Awake() {
            if (coroutine) {
                StartCoroutine(FixScreenSize());//在另外一个线程刷新
            } else {
                FixSize();
            }
        }

        void Start() { }

        IEnumerator FixScreenSize() {
			yield return null;
            FixSize();
        }

        void FixSize() {
            Vector2 size = new Vector2(DesignWidth, DesignHeight);

            GameObject canvas = GameObject.Find(CanvasRoot);
            if (canvas != null) {
                RectTransform rt0 = canvas.transform as RectTransform;
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
