local MessageBox = class(MF.page)

MessageBox.config = {
	name = 'MessageBox',
	ab = 'gui/comm.ab',
	prefab = 'MessageBox',
}

local leftBtn = nil
local rightBtn = nil
local tipText = nil
local leftBtnText = nil
local rightBtnText = nil

function MessageBox:Start( ... )
	leftBtn = self:getHieChild('MessageBox/Btns/leftButton')
	rightBtn = self:getHieChild('MessageBox/Btns/rightButton')
	tipText = self:getHieChild('MessageBox/tip'):GetComponent(TextType)
	leftBtnText = self:getHieChild('MessageBox/Btns/leftButton/Text'):GetComponent(TextType)
	rightBtnText = self:getHieChild('MessageBox/Btns/rightButton/Text'):GetComponent(TextType)
end

function MessageBox:popOK( tip , callback , btnText )
	self:SetActive(true)
	leftBtn:SetActive(false)
	rightBtn:SetActive(true)
	rightBtnText.text = btnText
	tipText.text = tip
	local rightCallBack = function (  )
		if callback then
			callback()
		end
		self:SetActive(false)
	end
	self:registerBtnClick(rightBtn,rightCallBack)
end

function MessageBox:PopYesNo( tip , leftCallback, rightCallback , tleftBtnText ,trightBtnText)
	self:SetActive(true)
	leftBtnText.text = tleftBtnText
    rightBtnText.text = trightBtnText
    tipText.text = tip
    leftBtn:SetActive(true)
    rightBtn:SetActive(true)
	
	local leftCallBack = function (  )
		if leftCallback then
			leftCallback()
		end
		self:SetActive(false)
	end
	self:registerBtnClick(rightBtn,leftCallBack)

	local rightCallBack = function (  )
		if rightCallback then
			rightCallback()
		end
		self:SetActive(false)
	end
	self:registerBtnClick(rightBtn,rightCallBack)
end

return MessageBox