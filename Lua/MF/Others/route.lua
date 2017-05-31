local Route = {}

local nodes = {}

function Route.push( name , ... )
	local file = ''
	local layer = require(file)
	layer:onInitGameObject()
	table.insert(nodes,layer)
	self:onEnter()

end

return Route