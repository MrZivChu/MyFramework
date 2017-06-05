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

-- '/HUD,false;Maze,true'  
-- 有斜杆 / 表示界面从头开始生成，也就是说仅仅只存在写明的界面，场景中不会存在其他界面
-- 无斜杆 / 表示界面继续生成，不会pop掉已经存在的界面
function Route.batch( paths )

	local first = string.sub(paths, 1, 1)
	local index = 1
	if  first == '/' then
	    paths = string.sub(paths, 2)
	end
	local tempPath = string.split(paths, ';')

	if first == '/' then
		local flag = 0
		for i,v in ipairs(tempPath) do
			local tpath = string.split(v, ',')
			if nodes[i].config.name ~= tpath[1] then
				flag = i
			else
				ObjectsHelper.SetObjIsActive(nodes[i].id,tpath[2])
			end
			if flag ~= 0 then
				Route.pop(#nodes - flag + 1)
				index = flag
				break
			end
		end
	end

	for i=index,#tempPath do
		local tpath = string.split(tempPath[i], ',')
		local page = MF.Route.push(tpath[1])
		ObjectsHelper.SetObjIsActive(page.id,tpath[2])
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

function Route.listenPage( name, e, f )
	for _,v in ipairs(nodes) do
		if v.config.name == name then 
			v:listen(e, f) 
			return 
		end
	end
end

function Route.deafPage( name, e, f )
	for _,v in ipairs(nodes) do
		if v.config.name == name then 
			v:deaf(e) 
			return 
		end
	end
end

function Route.emitPage( name, e, data )
	for _,v in ipairs(nodes) do
		if v.config.name == name then 
			v:emit(e, data) 
			return 
		end
	end
end

return Route