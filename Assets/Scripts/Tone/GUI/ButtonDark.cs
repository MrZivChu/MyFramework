using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using uTools;

namespace Tone.UI {
    //[AddComponentMenu("Tone/UI/Button Dark(Tone)")]
    public class ButtonDark : MonoBehaviour, uTools.uIPointHandler {

        public Image tweenTarget;
        public Color enter = new Color(.7f, .7f, .7f, 1f);
        public Color down = Color.grey;
        public float duration = .2f;

        Color mColor;

        public GameObject inLight;
        public float inDuration = 0.3f;
        public GameObject outLight;
        public float outDuration = 0.25f;
        public float outDelay = 0.2f;
        public Vector3 from = Vector3.one;
        public Vector3 to = new Vector3(1.3f, 1.3f, 1f);

        void Start() {
            if (tweenTarget == null) {
                tweenTarget = GetComponent<Image>();
            }
            mColor = tweenTarget.color;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Change(enter);
        }

        public void OnPointerDown(PointerEventData eventData) {
            Change(down);
            Scale(true);
        }

        public void OnPointerClick(PointerEventData eventData) {
        }

        public void OnPointerUp(PointerEventData eventData) {
            Change(mColor);
            //Scale(false);
        }

        public void OnPointerExit(PointerEventData eventData) {
            Change(mColor);
            //Scale(false);
        }

        void Change(Color to) {
            uTweenColor.Begin(tweenTarget.gameObject, duration, 0f, tweenTarget.color, to);
        }

        void Scale(bool scale) {
            if (scale) {
                inLight.SetActive(true);
                uTweenAlpha tw1 = uTweenAlpha.Begin<uTweenAlpha>(inLight, inDuration);
                tw1.from = 1f;
                tw1.to = 0f;
                tw1.duration = inDuration;

                outLight.SetActive(true);
                uTweenAlpha tw2 = uTweenAlpha.Begin<uTweenAlpha>(outLight, outDuration);
                tw2.from = 1f;
                tw2.to = 0f;
                tw2.duration = outDuration;
                tw2.delay = outDelay;

                uTweenScale tw3 = uTweenScale.Begin(outLight, from, to, outDuration, outDelay);
            }
        }
    }
}