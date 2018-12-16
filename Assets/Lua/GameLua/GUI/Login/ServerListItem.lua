local ServerListItem = class(MF.page)

ServerListItem.config = {
	name = 'ServerListItem',
	ab = 'login.ab',
	prefab = 'ServerListItem',
}

function ServerListItem:Awake( )
	self.serverNameObj = self:getHieChild('ServerListItem/Image/Text')

end

function ServerListItem:OnEnable( )

end

function ServerListItem:Start( )
	
end

function ServerListItem:SetData( data )
    local serverNameText = self.serverNameObj:GetComponent(TextType)
	serverNameText.text = data.serverName
end

return ServerListItem