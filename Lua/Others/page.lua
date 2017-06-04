local Page = {}

function Page:spawnPrefab( parentID ,childID)
	self.childs = {}

	self.index = require (self.config.podaPath)
	
	local abName = self.config.ab
	local prefabName = self.config.prefab	
	self.id = ObjectsHelper.SpawnPage(parentID or 0,childID or 0,abName,prefabName)
end

function Page:prepareData()
	
end

function Page:bindDataForUI()
	
end

function Page:onExit()
	
end

function Page:addChild( name , parentID , ... )
	local layer = requireLuaFile(name)
	if layer then
		MF.route.callLifeCycle(layer,self.id,parentID,...)
		table.insert(self.childs,layer)
		return layer
	end
end

function Page:addButtonClik( parentID , childID , func , param )
	ObjectsHelper.AddButtonClick(parentID,childID, function() func(self,param) end)
end

function Page:addToggleClik( parentID , childID , func , param )
	ObjectsHelper.AddToggleClick(parentID,childID, function(isOn) func(self,isOn,param) end)
end

return Page