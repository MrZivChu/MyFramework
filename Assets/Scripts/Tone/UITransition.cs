using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
//using DG.Tweening;

namespace Tone.UI {
    public class UITransition : MonoBehaviour {

        [SerializeField] public CanvasGroup fadeTarget;
        [SerializeField] public RectTransform scaleTarget;

        /// <summary>
        /// 进入场景时的过渡动画类型
        /// 0-无，1-Duang，2-上部进出，3-下部进出，4-左边进出，5-右边进出
        /// </summary>
        [SerializeField]
        public int enterType = 1;

        /// <summary>
        /// 退出场景时的过渡动画类型
        /// </summary>
        [SerializeField]
        public int exitType = 3;

        [SerializeField]
        public int easeType = 24;
        [SerializeField]
        public float start = .5f;
        [SerializeField]
        public float duration = .8f;

        private Vector3 mScale;

     //   void Start() {
     //   }

     //   public void EnterBegin() {
     //       //Debug.Log("EnterBegin");
     //       //Utils.BlockRaycasts(gameObject, false);

     //       if (scaleTarget == null) {
     //           scaleTarget = GetComponent<RectTransform>();
     //       }
     //       if (fadeTarget == null) {
     //           fadeTarget = GetComponent<CanvasGroup>();
     //       }

     //       Tweener tw = null;
     //       if (scaleTarget != null) {
     //           Vector3 startPos = scaleTarget.localPosition;
     //           switch (enterType) {
     //               case 1: {
     //                   Vector3 end = scaleTarget.localScale;
     //                   scaleTarget.localScale *= start;
     //                   tw = scaleTarget.DOScale(end, duration);
     //                   }
     //                   break;
     //               case 2: {
     //                   float end = startPos.y;
					//startPos.y = 640;
     //                   scaleTarget.localPosition = startPos;
     //                   tw = scaleTarget.DOLocalMoveY(end, duration);
     //                   }
     //                   break;
     //               case 3: {
     //                   float end = startPos.y;
     //                   startPos.y = -640;
     //                   scaleTarget.localPosition = startPos;
     //                   tw = scaleTarget.DOLocalMoveY(end, duration);
     //                   }
     //                   break;
     //               case 4: {
     //                   float end = startPos.x;
     //                   startPos.x = -960;
     //                   scaleTarget.localPosition = startPos;
     //                   tw = scaleTarget.DOLocalMoveX(end, duration);
     //                   }
     //                   break;
     //               case 5: {
     //                   float end = startPos.x;
     //                   startPos.x = 960;
     //                   scaleTarget.localPosition = startPos;
     //                   tw = scaleTarget.DOLocalMoveX(end, duration);
     //                   }
     //                   break;
     //               default:
     //                   break;
     //           }
     //           if (tw != null) {
     //               tw.SetEase((Ease)easeType);
     //           }
     //       }

     //       if (fadeTarget != null && enterType > 0) {
     //           fadeTarget.alpha = 0f;
     //           tw = fadeTarget.DOFade(1f, duration * 0.7f).SetEase(Ease.Linear);
     //       }

     //       if (tw != null) {
     //           tw.OnComplete(EnterOver);
     //       } else {
     //           EnterOver();
     //       }
     //   }

     //   void EnterOver() {
     //       //Utils.BlockRaycasts(gameObject, true);
     //       //Tone.Games.AppGlobal.LuaScriptMgr.CallLuaFunction("M2.Route.notify", gameObject, "EnterTransitionOver");
     //   }

     //   public void ExitBegin() {
     //       //Debug.Log("ExitBegin");
     //       //Utils.BlockRaycasts(gameObject, false);
            
     //       if (scaleTarget == null) {
     //           scaleTarget = GetComponent<RectTransform>();
     //       }
     //       if (fadeTarget == null) {
     //           fadeTarget = GetComponent<CanvasGroup>();
     //       }

     //       Tweener tw = null;
     //       if (scaleTarget != null) {
     //           switch (exitType) {
     //               case 1:
     //                   tw = scaleTarget.DOScale(0.1f, duration / 2);
     //                   break;
     //               case 2:
     //                   tw = scaleTarget.DOLocalMoveY(640, duration / 2);
     //                   break;
     //               case 3:
     //                   tw = scaleTarget.DOLocalMoveY(-640, duration / 2);
     //                   break;
     //               case 4:
     //                   tw = scaleTarget.DOLocalMoveX(-960, duration / 2);
     //                   break;
     //               case 5:
     //                   tw = scaleTarget.DOLocalMoveX(960, duration / 2);
     //                   break;
     //               default:
     //                   break;
     //           }
     //           if (tw != null) {
     //               tw.SetEase(Ease.Linear);
     //           }
     //       }

     //       if (fadeTarget != null && exitType > 0) {
     //           tw = fadeTarget.DOFade(0f, duration / 2).SetEase(Ease.Linear);
     //       }

     //       if (tw != null) {
     //           tw.OnComplete(ExitOver);
     //       } else {
     //           ExitOver();
     //       }

     //   }

     //   void ExitOver() {
     //       //Debug.Log("ExitOver");
     //       //Tone.Games.AppGlobal.LuaScriptMgr.CallLuaFunction("M2.Events.emit", gameObject, "ExitTransitionOver");
     //   }
    }
}
