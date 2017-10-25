local UILogin = class(MF.page)

UILogin.config = {
	name = 'UILogin',
	ab = 'gui/UILogin.ab',
	prefab = 'AB/login/UILogin',
	podaPath = 'GUI/Login/UILogin_pda'
}

function UILogin:prepareData( data )
	self:addButtonClik(self.id,self.index.btnOK,self.StartLogin )

	print('^^^^^^^^^^^^^^^^^^^')
	print_r(ObjectsHelper.GetClassType())
end

function UILogin:onNotify( sender , e , data )
	for i,v in ipairs(self.children) do
		v:onNotify(sender, e, data)
	end
end

function UILogin:StartLogin( data )
	local userName = self:getInputField(self.id,self.index.UserName)
	local userPwd = self:getInputField(self.id,self.index.UserPassword)
	if userName == 'zwh' and userPwd == '111' then
		print('login success')
		TipBox.GetInstance().PopYesNo("是否立即进入游戏",nil,function ( ... )
			LuaUtils.LoadLevel(2)
		end,"取消","确定")		
	else
		print('login error')
		TipBox.GetInstance().popOK('用户名或者密码错误',nil,'确定')
	end
end

return UILogin