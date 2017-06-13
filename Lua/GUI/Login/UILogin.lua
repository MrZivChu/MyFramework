local UILogin = class(MF.page)

UILogin.config = {
	name = 'UILogin',
	ab = 'gui/UILogin.ab',
	prefab = 'AB/login/UILogin',
	podaPath = 'GUI/Login/UILogin_pda'
}

function UILogin:prepareData( data )
	self:addButtonClik(self.id,self.index.btnOK,self.StartLogin )
end

function UILogin:onNotify( sender , e , data )
	for i,v in ipairs(self.children) do
		v:onNotify(sender, e, data)
	end
end

function UILogin:StartLogin( data )
	local userName = self.index.
end

return UILogin