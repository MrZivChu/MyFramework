using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tone.UI {
    public class ModelRenderTexture : MonoBehaviour {

        GameObject UI3D;

		public string RootName = "UI 3D";
        public string CameraName = "3D Camera";

        void Start() {
            UI3D = GameObject.Find("GUI/UI 3D");
            if (UI3D != null) {
                UI3D.SetActive(true);
            } else {
                Debug.LogError("Cannot find UI3D.");
                return;
            }

            Camera cam = UI3D.transform.FindChild(CameraName).GetComponent<Camera>();
            if (cam == null) {
				Debug.LogError("Cannot find 3DCamera.");
                return;
            }

            RawImage ri = gameObject.GetComponent<RawImage>();
            if (ri != null) {
                ri.texture = cam.targetTexture;
            } else {
				Debug.LogError("GameObject isn't RawImage.");
            }
        }

        void OnDestory() {
            if (UI3D != null) {
                UI3D.SetActive(false);
            }
        }
    }
}
