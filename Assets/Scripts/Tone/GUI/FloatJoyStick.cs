using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tone.UI {
    public class FloatJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

        Vector3 mOriginPos;
        static Vector2 mDir = Vector2.zero;
        bool stopGamePad = false;
        
		public GameObject JoyPanel;
		public GameObject JoyPoint;
		public bool Left = true;
		public float Radius = 40;
        public float Alpha = 0.6f;
        public bool Reset = true;

        void Awake() {
        }

        // Use this for initialization
        void Start() {
            if (JoyPanel != null) {
                mOriginPos = JoyPanel.transform.localPosition;
            }
            if (JoyPoint != null) {
                Image comp = JoyPoint.GetComponent<Image>();
                if (comp != null) {
                    comp.color = new Color(1f ,1f, 1f, Alpha);
                }
            }
        }

        // Update is called once per frame
        void Update() {

        }

#if UNITY_EDITOR
        protected void OnValidate() {
            if (JoyPoint != null) {
                Image comp = JoyPoint.GetComponent<Image>();
                if (comp != null) {
                    comp.color = new Color(1f ,1f, 1f, Alpha);
                }
            }
        }
#endif
        void OnDestory() {
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (JoyPanel != null) {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.pressPosition, eventData.pressEventCamera, out pos);
                JoyPanel.transform.localPosition = new Vector3(pos.x, pos.y, mOriginPos.z);
            }
            if (JoyPoint != null) {
                Image comp = JoyPoint.GetComponent<Image>();
                if (comp != null) {
                    comp.color = new Color(1f ,1f, 1f, 1f);
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (JoyPanel != null && Reset) {
                JoyPanel.transform.localPosition = mOriginPos;
            }
            if (JoyPoint != null) {
                JoyPoint.transform.localPosition = Vector3.zero;
                Image comp = JoyPoint.GetComponent<Image>();
                if (comp != null) {
                    comp.color = new Color(1f ,1f, 1f, Alpha);
                }
            }
        }

        public void OnBeginDrag(PointerEventData data) {
            stopGamePad = true;

            //Vector3 pos = new Vector3(data.pressPosition.x - mOffset.x, data.pressPosition.y - mOffset.y, mOriginPos.z);
            //JoyPanel.transform.localPosition = pos;
        }

        public void OnDrag(PointerEventData data) {

            mDir = data.position - data.pressPosition;
            if (mDir.magnitude > Radius) {
                mDir = mDir.normalized * Radius;
            }
            if (JoyPoint != null) {
                JoyPoint.transform.localPosition = new Vector3(mDir.x, mDir.y, 0);
            }
            mDir = mDir.normalized;
        }

        public void OnEndDrag(PointerEventData data) {
            mDir = Vector2.zero;
            stopGamePad = false;
        }

        public static Vector2 GetLeftAxis() {
            return mDir;
        }
    }
}