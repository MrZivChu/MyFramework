local UILogin = class(MF.page)

UILogin.config = {
	name = 'UILogin',
	ab = 'login.ab',
	prefab = 'UILogin',
}

function UILogin:Awake( )
end

function UILogin:OnEnable( )

end

function UILogin:Start( )
	local loginBtn = self:getHieChild('UILogin/Background/btnOK')
	self:registerBtnClick(loginBtn,self.StartLogin ,"hello")
end

function UILogin:onNotify( sender , e , data )
	for i,v in ipairs(self.children) do
		v:onNotify(sender, e, data)
	end
end

function UILogin:StartLogin( data )
	local userName = (self:getHieChild('UILogin/Background/UserName'):GetComponent(InputFieldType)).text
	local userPwd = (self:getHieChild('UILogin/Background/UserPassword'):GetComponent(InputFieldType)).text
	if userName == 'zwh' and userPwd == '111' then
		print_t('login success')
		MessageBox.PopYesNo("是否立即进入游戏",nil,function ( )
			CSharpUtilsForLua.LoadLevel(2)
		end,"取消","确定")		
	else
		print_t('login error')
		MessageBox.popOK('用户名或者密码错误',nil,'确定')
	end
end

return UILogin