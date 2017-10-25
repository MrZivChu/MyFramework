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
	print(123)
	if layer then
		--调用页面生命周期
		Route.callLifeCycle(layer , 0 ,0, ...)
		--设置上一个界面不好点击
		local prePage = nodes[#nodes]
		if prePage then
			ObjectsHelper.SetIsReceiveClick(prePage.id,0,0)
		end
		--设置界面的Canvas
		ObjectsHelper.SetSortOrder(layer.id,0,0)
		--执行自定义方法
		layer:checkCustomOpenPage()
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
			if nodes[i] then
				if nodes[i].config.name ~= tpath[1] then
					flag = i
				else
					local isActive = tpath[2] == 'true' and 1 or 0
					ObjectsHelper.SetObjIsActive(nodes[i].id,0,isActive)
				end
				if flag ~= 0 then
					Route.pop(#nodes - flag + 1)
					index = flag
					break
				end
			else
				break
			end
		end
	end

	for i=index,#tempPath do
		local tpath = string.split(tempPath[i], ',')
		local page = Route.push(tpath[1])
		local isActive = tpath[2] == 'true' and 1 or 0
		ObjectsHelper.SetObjIsActive(page.id,0,isActive)
	end
end

function Route.pop( target )
	local index = nil
	if type(target) == 'string' then
		for i=#nodes,1,-1 do
			if nodes[i].config.name == target then
				index = i
				break
			end
		end
	elseif type(target) == 'number' then
		for i=1,num do
			local node = nodes[#nodes]
			if node then
				table.remove(nodes)
				node:onExit()
			end
		end
		return
	elseif type(target) == 'table' then
		for i=#nodes,1,-1 do
			if nodes[i].id == target.id then
				index = i
				break
			end
		end
	end

	if index == nil then return end

	local page = nodes[index]
	table.remove(nodes, index)
	page:onExit()

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