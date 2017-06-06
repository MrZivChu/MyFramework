using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using uTools;

namespace Tone.UI {
    //[AddComponentMenu("Tone/UI/Button Dark(Tone)")]
    public class ButtonTaps : MonoBehaviour, uTools.uIPointHandler {

        public float delay = .3f;
        public float duration = .5f;

        float needTime = -1f;
        int mCounter;

        void Start() {
            mCounter = 0;
        }

        void Update() {
            if (needTime > 0f) {
                needTime -= Time.deltaTime;
                if (needTime <= 0f) {
                    mCounter++;
                    // Tone.Games.AppGlobal.LuaScriptMgr.CallLuaFunction("M2.Events.emit", gameObject, "Taps");
                    Debug.Log("Taps" + mCounter);
                }
                while(needTime <= 0f) {
                    needTime += duration;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (gameObject.GetComponent<Button>().interactable) {
                mCounter = 0;
                needTime = delay;
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
        }

        public void OnPointerUp(PointerEventData eventData) {
            needTime = -1f;
        }

        public void OnPointerExit(PointerEventData eventData) {
            needTime = -1f;
        }
    }
}