-- UGUI
import "UnityEngine.Events.UnityEventBase"
import "UnityEngine.Events.UnityEvent"
import "UnityEngine.EventSystems.UIBehaviour"
import "UnityEngine.EventSystems.BaseRaycaster"
import "UnityEngine.UI.GraphicRaycaster"
import "UnityEngine.UI.Graphic"
import "UnityEngine.UI.MaskableGraphic"
import "UnityEngine.UI.ColorBlock"
import "UnityEngine.UI.Selectable"
import "UnityEngine.UI.Button.ButtonClickedEvent"
ButtonWrap = import "UnityEngine.UI.Button"
import "UnityEngine.UI.Image"
TextWrap = import "UnityEngine.UI.Text"
import "UnityEngine.UI.Toggle.ToggleEvent"
ToggleWrap = import "UnityEngine.UI.Toggle"
import "UnityEngine.UI.ToggleGroup"
import "UnityEngine.UI.RawImage"
import "UnityEngine.UI.LayoutElement"
import "UnityEngine.UI.Scrollbar"
import "UnityEngine.UI.GridLayoutGroup"
import "UnityEngine.UI.Slider"
InputFieldWrap = import "UnityEngine.UI.InputField"
import "UnityEngine.UI.Shadow"
import "UnityEngine.UI.Outline"
import "UnityEngine.UI.HorizontalOrVerticalLayoutGroup"
import "UnityEngine.UI.BaseMeshEffect"


import "UnityEngine.MonoBehaviour"
import "UnityEngine.Behaviour"
import "UnityEngine.Component"
import "UnityEngine.Object"

import "ObjectsHelper"
CSharpUtilsForLua = import "CSharpUtilsForLua"
ChildrenHelperWrap = import "ChildrenHelper"

ButtonType = ButtonWrap.GetClassType()
InputFieldType = InputFieldWrap.GetClassType()
ChildrenHelperType = ChildrenHelperWrap.GetClassType()
TextType = TextWrap.GetClassType()
ToggleType = ToggleWrap.GetClassType()