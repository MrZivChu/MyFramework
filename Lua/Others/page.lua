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

function Page:onNotify( sender , e , data )
	
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


function Page:emit( e, data )
	if self.onEvents and self.onEvents[e] then
		for i,f in ipairs(self.onEvents[e]) do
			f(self, e, data)
		end
	end
end

function Page:listen( e, f )
	if not self.onEvents then 
		self.onEvents = {} 
	end
	if self.onEvents[e] then
		table.insert(self.onEvents[e], f)
	else
		self.onEvents[e] = {f}
	end
end

function Page:deaf( e )
	if self.onEvents and self.onEvents[e] then
		self.onEvents[e] = nil
	end
end

return Page