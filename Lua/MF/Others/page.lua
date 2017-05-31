function class(tParent)
	local tmetatable = {}
	tmetatable.__index = tParent
	local son = setmetatable({},tmetatable)
	son.super = tParent
	return son
end

local Parent = {}

function Parent:onInitGameObject( ... )
	local abName = self.poda.ab
	local prefabName = self.poda.prefab
	self.id = Utils.LoadGameObject(abName,prefabName)
end

function Parent:onExit()
	
end

return Parent