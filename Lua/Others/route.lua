local Route = {}

local nodes = {}

--调用页面生命周期
function Route.callLifeCycle( layer , parentID , childID , ...)
	if layer then		
		layer:spawnPrefab(parentID,childID)
    	layer:prepareData(...)
    	layer:bindDataForUI(...)
	end
end

function Route.push( name , ... )
	local layer = requireLuaFile(name)
	if layer then
		Route.callLifeCycle(layer , 0 ,0, ...)
		table.insert(nodes,layer)
		return layer
	end
end

function Route.pop( num )
	num = num and num or 1
	for i=1,num do
		local node = nodes[#nodes]
		table.remove(nodes)
		node:onExit()
	end
end

function  Route.notify( sender , e , data)
	for i,v in ipairs(nodes) do
		nodes[i]:onNotify(sender , e , data)
	end
end

return Route