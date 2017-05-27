local Page = class ('M2Page')

local clickTime = os.clock()
local function checkClickTime( ... )
	local now = os.clock()
	local diff = now - clickTime
	if diff > 0.15 then
		clickTime = now
		return true
	else
		return false
	end
end



local function parseEvents( page, go, t )
	assert(go ~= nil, M2.Localize.error.ip)

	if t.type == 'Button' then
		assert(t.events ~= nil and type(t.events) == 'table', M2.Localize.error.ip)
		if t.events.PointerClick then
			local btn = go:GetComponent('Button')
			if btn then
				btn.onClick:AddListener(function()
					if checkClickTime() and checkNetConnect(page.name) then 
						
						-- 强制点击后，需要隐藏遮罩
						-- 需要在执行函数之前,因为事件触发例如skip会直接执行下一步内容
						M2.Lessons.onClickButton(go.name)

						t.events.PointerClick(page, go) 						
					end
				end)
			else
				assert(false, go.name..' is not a button.')
			end
		else
			for k,v in pairs(t.events) do
				--print(k,v)
				local f = type(v) == "function" and function(o, e) v(page, go, o, e) end or function(o, e) M2.Route.notify(o, e, page, go) end
				M2.Events.add(go, page.name, k, f)
				Utils.EnableEventListener(go, k)
			end
		end

	elseif t.type == 'Toggle' then
		local f = t.events.onValueChanged
		if f and type(f) == 'function' then
			--page:print(t.path)
			go:GetComponent('Toggle').onValueChanged:AddListener(function(isOn )
				if isOn then
					M2.Lessons.onClickToggle(go.name)--toggle也需要响应
				end
				f(page,isOn,go)
			end)
		elseif t.events.event then
			go:GetComponent('Toggle').onValueChanged:AddListener(function(isOn)
				if isOn then
					M2.Lessons.onClickToggle(go.name)--toggle也需要响应
				end
				page:emit(t.events.event, {object=go, isOn=isOn, data=t.events.data})
			end)
		elseif t.events.bubbling then
			go:GetComponent('Toggle').onValueChanged:AddListener(function( isOn )
				if isOn then
					M2.Lessons.onClickToggle(go.name)--toggle也需要响应
				end
				page:bubbling(page, t.events.bubbling, {object=go, isOn=isOn, data=t.events.data})
			end)
		else
			--page:print(t.path..t.events.notify)
			go:GetComponent('Toggle').onValueChanged:AddListener(function(isOn) 
				--page:print(t.path..t.events.notify)
				if isOn then
					M2.Lessons.onClickToggle(go.name)--toggle也需要响应
				end
				M2.Route.notify(go, t.events.notify or 'Toggle.onValueChanged', {name=t.name or t.path, data=t.events.data, isOn=isOn}) 
			end)
		end

	elseif type(go) == 'table' then
		for k,v in pairs(t.events) do
			go:listen(k, v)
		end
		
	else
		page:print("No implement type: ", t.type or t.child)
	end
end

function Page:getName(  )
	return root and root.name or self.showName or self.__cname
end

function Page:print( ... )
	M2.log(llv, self.name, ' -> ', ...)
end

function Page:close(  )
	M2.Route.close(self)
	-- if M2.Route.current() == self then
	-- 	M2.Route.pop(1)
	-- end
end

function Page:exit( exit )
	if not M2.scene.pages or not M2.scene.pages[self.poda.name] then
		M2.Route.pop(1)
	else
		M2.Route.exit(M2.scene.pages, self.poda.name, exit)
	end
end

function Page:ctor()
	self.allowUpdate = true
	self.children = {}

	self:print('Page:ctor()')
end


--用于检测是否有需要自动打开的界面(这些界面比较特殊，需要传参数)
function Page:checkCustomOpenPage( ... )
	-- body
end

function Page:onEnter()
	self:print("Page:onEnter")

	if self.poda ~= nil then
		if self.poda.allowUpdate ~= nil then
			self.allowUpdate = self.poda.allowUpdate
		end

		if self.poda.texture ~= nil then
			local textures = string.split(self.poda.texture, ',')
			for i,v in ipairs(textures) do
				Tone.Assets.ResourceDepot.LoadAll(v, self.id)
			end
		end
		
		if self.poda.prefab ~= nil and self.root == nil then
			Tone.Assets.ResourceDepot.LoadAB(self.poda.ab)
		
			self.root = Utils.LoadUI(self.rootParent or (M2.scene.gameScene and M2.scene.gameScene.uiRoot), self.poda.ab, self.poda.prefab, self.id)
			-- assert(self.root ~= nil and type(self.root) == 'userdata', "LoadUI failed.")
			if self.root == nil then
				M2.log(llv, 'LoadUI failed.')
				self:close()
				return
			end
		end

		--加入半透明底
		if self.poda.block == true then
			-- local block = Utils.InsertBase(self.root, true, self.poda.block)
			-- if self.poda.block then
			-- 	M2.Events.add(block, self.name, 'PointerClick', function (  )
			-- 		if checkNetConnect() then--确保联网才可以点击
			-- 			M2.Route.pop()
			-- 		end
			-- 	end)
			-- end
			local blackself = self:addChild(self.root, 'Black', 'Black',self.poda.block)
			if blackself then
				table.remove(self.children)
				blackself.root.transform:SetAsFirstSibling()
				if self.poda.block then
					local x = blackself.root:AddComponent(EventTriggerListener.GetClassType())
					x.onClick = function (  )
						if checkNetConnect() then--确保联网才可以点击
							blackself.parent:close()
						end
					end
				end
			end
		end

		if self.poda.controls ~= nil then
			assert(type(self.poda.controls) == 'table', M2.Localize.error.ip)

			for i,v in ipairs(self.poda.controls) do
				assert(v ~= nil and type(v) == 'table', M2.Localize.error.ip)
				assert(v.path ~= nil and type(v.path) == 'string', M2.Localize.error.ip)

				local ctrl --controls下的对象
				if v.path == "." then
					ctrl = self.root
				else
					ctrl = self.root.transform:Find(v.path)
					assert(ctrl ~= nil and type(ctrl) == 'userdata', self.name.." -> ".. v.path .. " is not found.")
					ctrl = ctrl.gameObject
				end



				if v.events then
					--assert(v.type and type(v.type) == 'string', v.path .. " : type is error")
					parseEvents(self, ctrl, v)
				end
			end
		end
		self.isLoaded = true
	end


	-- self:print(' Page:onEnter() over')

	self:beginEnterTransition()

	self:onUpdateUI(false)

	--以下代码已经在Route中调用，不要去掉注释
	--个别情况不能保证孩子节点的onEnter全部执行完了，只是说父节点自己的onEnter执行完了
	--但是如果孩子节点的ab或者texture都已经加载过了，那么会立即执行孩子节点的onEnter方法,不会等到协程返回才执行onEnter方法
	--从目前来看基本上孩子节点的onEnter是会立马执行的（因为ab被父节点加载过了），所以在onEnterOver中可以执行self:onUpdateUI(true)
	-- if self.onEnterOver and type(self.onEnterOver) == 'function' then
	-- 	self:onEnterOver()
	-- 	self.onEnterOver = nil
	-- end

	if M2.Lessons.CheckInLesson() and self.parent == nil then
		clickTime = os.clock() + 0.2
	end
end

function Page:beginEnterTransition(  )
	-- 进入场景动画
	if self.poda.transition then
		local comp = self.root:GetComponent('UITransition')
		assert(comp)
		if self.poda.transition.enter then
			comp.enterType = self.poda.transition.enter
			comp:EnterBegin()
		end
	end

	if self.TweensEnter then
		for k, v in pairs(self.TweensEnter) do
			if v~=nil then v:Reset() end
		end
		self.TweensEnter = nil
	end
	
	local closeList = self.poda.closeOthers
	if closeList ~= nil and #closeList > 0 then
		M2.Route.CloseOthers(closeList,false,self.poda.forbidCamera)
	end
end

local function isAllLoaded(self)
	if not self.isLoaded then return false end
	for i,v in ipairs(self.children) do
		if not isAllLoaded(v) then return false end
	end
	return true
end

function Page:beginExitTransition(  )
	--注意：不能用fori，setParent会改变children
	local closeList = self.poda.closeOthers
	if closeList ~= nil and #closeList > 0 then
		M2.Route.CloseOthers(closeList,true,self.poda.forbidCamera)
	end

	if self.TweensExit then
		for k, v in pairs(self.TweensExit) do
			if v ~= nil then 
				v:Reset()
			end
		end
		self.TweensExit = nil
	end

	if self.poda.transition and self.poda.transition.exit then
		local comp = self.root:GetComponent('UITransition')
		assert(comp)
		comp.exitType = self.poda.transition.exit
		comp:ExitBegin()
		M2.Events.add(self.root, self.name, 'ExitTransitionOver', function( )
			self:onExit()
		end)
	else
		self:onExit()
	end
end

function Page:onExit()
	-- 只有界面关闭才需要调用gc，子控件不需要
	local callGC = self.parent == nil

	self.needTexture = 0

	if self.callback then
		for k, v in pairs(self.callback) do
			coroutine.start(function ()
				coroutine.wait(1)
				v()
			end)
		end
		self.callback = nil
	end

	local children = self.children
	self.children = {}
	for _,v in ipairs(children) do
		v:onExit()
	end
	children = nil

	-- self:print("Page:onExit")
	M2.Events.removeWithTag(self.name)
	self.onEvents = nil

	self.onUpdate = nil
	self.controls = {}
	Object.Destroy(self.root)
	self.root = nil
	self.isLoaded = nil

	-- self.children = {}
	self:setParent(nil)
	self.data = nil
	-- self.class = nil

	Tone.Assets.ResourceDepot.UnloadResource(self.id)	
	-- self:print("Page:onExit")
	-- local name = self.__cname
	-- for k,v in pairs(self) do
	-- 	self[k] = nil
	-- end

	-- 计数超过100才需要调用gc
	gc_count = gc_count + 1
	if callGC and gc_count > 100 then
		gc_count = 0
		LuaGC()
	end
end

function Page:onUpdateUI(force)
	if not self.isLoaded then return end

	if self.allowUpdate or force == true then
		-- self:print("onUpdateUI:",force)
		if self.onUpdate then
			local flag = true
			local msg = nil
			for k,v in pairs(self.onUpdate) do
				flag, msg = xpcall(v, traceback, self, k)
				if not flag then Utils.Error(self.name..k.name..msg) end
			end
		end
	end

end

function Page:updateTable(go, rows, prefab, data, page, option)
	assert(not rows or type(rows) == 'number', M2.Localize.error.ip)
	data = data or {}
	local count = #data 
	if not rows or rows == 0 then rows = math.max(count, go.transform.childCount) end
	if not page or rows >= count then page = 0 end
	local begin = rows * page

	local item
	for i=1,rows do 
		if begin + i <= count then 
			if i <= go.transform.childCount then
				item = self:findChild(go.transform:GetChild(i-1).gameObject)
				if item then item:SetActive(true) end
			else
				item = self:addChild(go, nil, prefab, option) 
			end
			if item then item:setCell(i, data[i]) end
		else
			if i <= go.transform.childCount then
				item = self:findChild(go.transform:GetChild(i-1).gameObject)
				if item then item:SetActive(false) end
				-- item:setCell(nil, nil)
			end
		end
		if item ~= nil and item.root~= nil then item.root.name = item.showName..i end
	end
end

function Page:SetActive( isActive )
	if self.root then
		self.root:SetActive(isActive)
	end
	--Utils.SetActive(self.root, isActive)
end

function Page:setCell( index, data )
	if self.showName and self.root then self.root.name = self.showName..index end
	self.index = index
	self:setData(data) 
end

function Page:setData( data )
	assert(false, 'No setData function.')
end

----	Event Functions	----

function Page:emit( e, data )
	if self.onEvents and self.onEvents[e] then
		for i,v in ipairs(self.onEvents[e]) do
			v(self, e, data)
		end
	end
end

function Page:listen( e, f )
	assert(e and type(e) == 'string', M2.Localize.error.ip)
	assert(f and type(f) == 'function', M2.Localize.error.ip)

	if not self.onEvents then self.onEvents = {} end
	if self.onEvents[e] then
		table.insert(self.onEvents[e], f)
	else
		self.onEvents[e] = {f}
	end
end

function Page:deaf( e, f )
	if self.onEvents then
		M2.removeItem(self.onEvents[e], f)
	end
end

function Page:onNotify( sender, e, data )
	--self:print('(Page)',e)
	if self.onEvents and self.onEvents[e] then
		for i,v in ipairs(self.onEvents[e]) do
			--self:print("onNotify:"..e)
			v(sender, e, data)
		end
	end

	for i,v in ipairs(self.children) do
		--v:print("children.onNotify")
		v:onNotify(sender, e, data)
	end
end

function Page:notify( e, data )
	M2.Route.notify(self, e, data)
end

function Page:bubbling( sender, e, data )
	if self.onEvents and self.onEvents[e] then
		for i,v in ipairs(self.onEvents[e]) do
			if v(sender, e, data) == true then
				return true
			end
		end
	end

	if self.parent then
		if self.parent:bubbling(sender, e, data) == true then
			return true
		end
	end
	return false
end

----	Child Functions	----

--由于名称可能重复，不能使用字典模式，只能使用数组模式

function Page:setParent( page, rootParent )
	if page then
		--增加到子页面列表
		if self.parent then	self:setParent() end
		table.insert(page.children, self)
		self.parent = page
		if self.root and rootParent then
			self.root.transform:SetParent(rootParent.transform)
		end
	else
		--从子页面列表删除
		if self.parent then
			for i,v in ipairs(self.parent.children) do
				if v == self then table.remove(self.parent.children,i) break end
			end
			self.parent = nil
			self.rootParent = nil
			if self.root then
				self.root.transform:SetParent(nil)
			end
		end
	end
end

function Page:addChild( parent, name, alias, ... )
	-- print('addChild '..alias)
	local child = M2.Route.addChild(alias, ...)
	assert(child, M2.Localize.error.ip)
	
	child:setParent(self)
	child.rootParent = parent or self.root
	if name then child.showName=name end

	local function onLoaded( self )
		self:onEnter()
		if self.onEnterOver then self:onEnterOver() end
	end
	
	if child.needTexture == 0 then
		onLoaded(child)
	else
		child.onLoaded = onLoaded
	end

	return child
end

function Page:bindChild( go, name, alias, ... )
	local child = M2.Route.addChild(alias, ...)
	assert(child, M2.Localize.error.ip)
	
	child:setParent(self)
	child.root = go
	if name then child.showName=name end

	local function onLoaded( self )
		self:onEnter()
		if self.onEnterOver then self:onEnterOver() end
	end
	
	if child.needTexture == 0 then
		onLoaded(child)
	else
		child.onLoaded = onLoaded
	end

	return child
end

function Page:findChild( child )
	--print('----findChild----'..child.name)
	for i,v in ipairs(self.children) do
		if v.root == child then	return v end
	end
end

return Page