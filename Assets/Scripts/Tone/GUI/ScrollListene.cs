using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tone.UI
{
    public class ScrollListene : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        ScrollRect rect;

        public GameObject Points;

        //是否拖拽结束
        bool isDrag = false;

        // Use this for initialization
        void Start()
        {
            rect = transform.GetComponent<ScrollRect>();
        }

        /// <summary>
        /// 拖动开始
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
    //        Tone.Games.AppGlobal.LuaScriptMgr.CallLuaFunction("M2.Events.emit", "RoleModify", "BeginMove", false);
        }

        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
    //        Tone.Games.AppGlobal.LuaScriptMgr.CallLuaFunction("M2.Events.emit", "RoleModify", "EndMove", true);

        }

    }
}

