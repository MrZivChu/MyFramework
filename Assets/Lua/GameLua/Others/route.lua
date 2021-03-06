local Route = {}

local pages = {}

--生成UI界面
local function SpawnUI( abName , prefabName , parent)
	if abName and prefabName then		
		return ObjectsHelper.SpawnPage(abName,prefabName,parent)
	end
	return nil
end

--调用页面生命周期
local function ExecLifeCycle( page )
	if page then		
    	page:Awake()
    	page:OnEnable()
    	page:Start()
	end
end

function Route.Instance( name , parent )
	local page = requireLuaFile(name)
	if page then
		local theParent = parent and parent or MF.uiRoot
		local uiPanel = SpawnUI(page.config.ab,page.config.prefab, theParent)
		if uiPanel ~= nil then
			page.gameObject = uiPanel
			page.transform = uiPanel.transform
			page.parent = theParent
			ExecLifeCycle(page)
			return page
		end
	end
end

function Route.push( name  )
	local page = Route.Instance(name)
	if page then
		--设置上一个界面不好点击
		-- local prePage = pages[#pages]
		-- if prePage then
		-- 	ObjectsHelper.SetIsReceiveClick(prePage.id,0,0)
		-- end
		--设置界面的Canvas
		--ObjectsHelper.SetSortOrder(page.id,0,0)
		--执行自定义方法
		--page:checkCustomOpenPage()
		table.insert(pages,page)
		return page
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
			if pages[i] then
				if pages[i].config.name ~= tpath[1] then
					flag = i
				else
					local isActive = tpath[2] == 'true' and 1 or 0
					ObjectsHelper.SetObjIsActive(pages[i].id,0,isActive)
				end
				if flag ~= 0 then
					Route.pop(#pages - flag + 1)
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
		for i=#pages,1,-1 do
			if pages[i].config.name == target then
				index = i
				break
			end
		end
	elseif type(target) == 'number' then
		for i=1,num do
			local node = pages[#pages]
			if node then
				table.remove(pages)
				node:onExit()
			end
		end
		return
	elseif type(target) == 'table' then
		for i=#pages,1,-1 do
			if pages[i].id == target.id then
				index = i
				break
			end
		end
	end

	if index == nil then return end

	local page = pages[index]
	table.remove(pages, index)
	page:onExit()

end

function  Route.notify( sender , e , data)
	for i,v in ipairs(pages) do
		pages[i]:onNotify(sender , e , data)
	end
end

function Route.listenPage( name, e, f )
	for _,v in ipairs(pages) do
		if v.config.name == name then 
			v:listen(e, f) 
			return 
		end
	end
end

function Route.deafPage( name, e, f )
	for _,v in ipairs(pages) do
		if v.config.name == name then 
			v:deaf(e) 
			return 
		end
	end
end

function Route.emitPage( name, e, data )
	for _,v in ipairs(pages) do
		if v.config.name == name then 
			v:emit(e, data) 
			return 
		end
	end
end

return Route