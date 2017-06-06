using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tone.UI {
    public class RoundLayoutGroup : UIBehaviour, ILayoutGroup {

        public float Radius = 40;
        public float StartAngle = 0;
        public uint Capacity = 0;
        public bool Clockwise = true;

        void Start() {
        }

        void Update() {
        }

#if UNITY_EDITOR
		protected void OnValidate() {
			CalcChildrenAxis ();
		}
#endif

        //protected override void OnRectTransformDimensionsChange() {
        //    Debug.Log("OnRectTransformDimensionsChange");
        //}

        //protected virtual void OnTransformChildrenChanged() {
        //    Debug.Log("OnTransformChildrenChanged");
        //    //CalcChildrenAxis();
        //}

        public void CalcChildrenAxis() {
            int count = gameObject.transform.childCount;
            float angle = Mathf.PI * 2 * StartAngle / 360;
            float step = Capacity == 0 ? Mathf.PI * 2 / count : Mathf.PI * 2 / Capacity;
            if (Clockwise) step = -step;

            for (int i = 0; i < count; i++, angle += step)
			{
                gameObject.transform.GetChild(i).localPosition = new Vector3(Radius * Mathf.Cos(angle), Radius * Mathf.Sin(angle), gameObject.transform.localPosition.z);
			}
        }

        public void SetLayoutHorizontal() {
            //Debug.Log("SetLayoutHorizontal");
            CalcChildrenAxis();
        }

        public void SetLayoutVertical() {
            //Debug.Log("SetLayoutVertical");
            //CalcChildrenAxis();
        }
    }
}
