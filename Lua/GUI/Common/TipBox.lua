local TipBox = {}

local pageID = nil
local index = {}

function TipBox.GetInstance( ... )
	if pageID == nil then
		index = require 'GUI/Common/TipBox_pda'
		pageID = ObjectsHelper.SpawnPage( 0, 0,'','TipBox')
		ObjectsHelper.SetSortOrder(pageID,0,300)
		ObjectsHelper.SetObjIsActive( pageID, 0, 0)
	end
	return TipBox
end

function TipBox.popOK( tip , callback , btnText )
	ObjectsHelper.SetObjIsActive( pageID, 0, 1)
	ObjectsHelper.SetObjIsActive( pageID, index.leftButton, 0)
	ObjectsHelper.SetObjIsActive( pageID, index.rightButton, 1)
	ObjectsHelper.SetText(pageID,index.rightBtnText,btnText)
	ObjectsHelper.SetText(pageID,index.tip,tip)
	function theCallBack( ... )
		if callback then
			callback()
		end
		ObjectsHelper.SetObjIsActive( pageID, 0, 0)
	end
	ObjectsHelper.AddButtonClick(pageID,index.rightButton,theCallBack)
end

function TipBox.PopYesNo( tip , leftCallback, rightCallback , tleftBtnText ,trightBtnText)
	ObjectsHelper.SetObjIsActive( pageID, 0, 1)
	ObjectsHelper.SetText(pageID,index.leftBtnText,tleftBtnText)
	ObjectsHelper.SetText(pageID,index.rightBtnText,trightBtnText)
	ObjectsHelper.SetText(pageID,index.tip,tip)

	ObjectsHelper.SetObjIsActive( pageID, index.leftButton, 1)
	ObjectsHelper.SetObjIsActive( pageID, index.rightButton, 1)
	
	function ttleftCallBack( ... )
		if leftCallback then
			leftCallback()
		end
		ObjectsHelper.SetObjIsActive( pageID, 0, 0)
	end
	ObjectsHelper.AddButtonClick(pageID,index.leftButton,ttleftCallBack)

	function ttrightCallBack( ... )
		if rightCallback then
			rightCallback()
		end
		ObjectsHelper.SetObjIsActive( pageID, 0, 0)
	end
	ObjectsHelper.AddButtonClick(pageID,index.rightButton,ttrightCallBack)
end

return TipBox