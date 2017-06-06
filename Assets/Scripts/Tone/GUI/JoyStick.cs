using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tone.UI {
    public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        Vector3 mOriginPos;
        public float Radius = 40;

        void Awake() {
        }

        // Use this for initialization
        void Start() {
            mOriginPos = transform.localPosition;
            //M2.InputManager.Instance.OnHandleSwing += onGamePad;
        }

        // Update is called once per frame
        void Update() {

        }

        void OnDestory() {
            //M2.InputManager.Instance.OnHandleSwing -= onGamePad;
        }

//        void onGamePad(InputEnum.HandleAxis axis, Vector2 vec){
//            if (axis == InputEnum.HandleAxis.Left) {
//                transform.localPosition = mOriginPos + new Vector3(vec.x, vec.y, 0);
//            }
//        }

        public void OnBeginDrag(PointerEventData data) {
            
        }

        public void OnDrag(PointerEventData data) {

            Vector2 delta = data.position - data.pressPosition;
            transform.localPosition = mOriginPos + new Vector3(delta.x, delta.y, 0);

            if (Vector3.Distance(transform.localPosition, mOriginPos) > Radius) {
                Vector3 dir = (transform.localPosition - mOriginPos).normalized;
                transform.localPosition = mOriginPos + dir * Radius;
            }
        }

        public void OnEndDrag(PointerEventData data) {
            transform.localPosition = mOriginPos;
        }
    }
}