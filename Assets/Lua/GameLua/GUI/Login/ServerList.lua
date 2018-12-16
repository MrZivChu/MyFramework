local ServerList = class(MF.page)

ServerList.config = {
	name = 'ServerList',
	ab = 'login.ab',
	prefab = 'ServerList',
}

function ServerList:Awake( )
	self.serverListParent = self:getHieChild('ServerList/Scroll View/Viewport/Content')
	self.preBtn = self:getHieChild('ServerList/preBtn')
	self.nextBtn = self:getHieChild('ServerList/nextBtn')
end

function ServerList:OnEnable( )

end

function ServerList:Start( )
	local data = { { serverName = '电信1区'},{ serverName = '电信2区'}}
	self:spawnCellForTable('ServerListItem',self.serverListParent,data,ServerList.InitData)

	self:registerBtnClick(self.preBtn,self.PrePage)
	self:registerBtnClick(self.nextBtn,self.NextPage)
end

function ServerList:InitData( page , obj , data , index  )
	page:SetData(data)
end


function ServerList:onNotify( sender , e , data )
	
end

function ServerList:NextPage( )
	local data = { { serverName = '电信3区'}}
	self:spawnCellForTable('ServerListItem',self.serverListParent,data,ServerList.InitData)
end

function ServerList:PrePage( )
	local data = { { serverName = '电信4区'},{ serverName = '电信5区'},{ serverName = '电信6区'}}
	self:spawnCellForTable('ServerListItem',self.serverListParent,data,ServerList.InitData)
end

return ServerList