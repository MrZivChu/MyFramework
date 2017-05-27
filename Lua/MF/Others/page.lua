local Page = class('M2Page2')


-- 绑定事件处理函数
local function BindEvent( self )
	for k,v in pairs(self.c) do
		if type(k) == 'string' then
			local f = self['onUpdate_'..v.name]
			if f then v.onUpdate = f end
			
			f = self['onEvent_'..v.name]
			if f then 
				v.onEvent = f
				UITools.AddCtrlEvent(self._id + v.index, function(...) f(self, v.index, ...) end)
			end
		end
	end
end

-- 更新等待队列
function onUpdateWait( self )
	if self.w == nil then return end

	M2.log(llv, self.w, self.__cname..self._id..'.onUpdateWait')
	for _,v in ipairs(self.w) do
		local c = self.c[v]
		if c then
			-- M2.log(llv, c, 'control')
			if c.onUpdate then
			 	c.onUpdate(self, c.index)
			 elseif c.data then
			 	if c.data.visible ~= nil then UITools.SetVisible(self._id + c.index, c.data.visible) end
			 	if c.data.enable ~= nil then UITools.SetEnable(self._id + c.index, c.data.enable) end
			 	if c.data.text then UITools.SetText(self._id + c.index, c.data.text) end
			 	if c.data.color then UITools.SetColor(self._id + c.index, c.data.color) end
				if c.data.image then UITools.SetImage(self._id + c.index, c.data.image, c.data.tex) end
				if c.data.toggle ~= nil then UITools.SetToggle(self._id + c.index, c.data.toggle) end
				if c.data.current then UITools.SetFrameBar(self._id + c.index, c.data.current, c.data.max) end
			 	if c.data.gradient then UITools.SetGradientColor(self._id + c.index, c.data.c1, c.data.c2) end
			 	if c.data.outline then UITools.SetOutline(self._id + c.index, c.data.outline) end
			end
		else
			M2.log(1, self.__cname..'['..tostring(c)..'] is not exist.(111)')
		end
	end
	self.w = nil
end

-------------------		基本函数	---------------------------------

-- 构造函数
function Page:ctor( ... )
	self._id = uu.addPage(self)	-- 界面编号，与C#互动的标识，放中对象池中也不会改变
	-- self._index = nil		-- 如果是Item，则表示索引
	-- self._inScene = false		-- 是否已经加入场景，即调用过onEnter
	-- self.enterUpdate = nil	-- 进入场景时是否允许自动更新，缺省可以
	self.children = {}		-- 子页面
	self.onEvents = {}		-- 事件函数
	self.c = {				-- 控件列表
		-- index = nil,			-- 控件索引
		-- name = nil,			-- 控件名称
		-- onEvent = nil,		-- 响应函数
		-- onUpdate = nil,		-- 更新函数
		-- data = nil,			-- 更新数据，如果有更新函数，此项不起作用
	}				
	-- self.w = nil			-- 等待更新的控件

	M2.log(llv, self.__cname..self._id..'.ctor()')
end

-- 加载资源耗费一点时间，但是与更新控件和加载子页面的动作分开了，为分步加载留下了可能
function Page:load( page, parent )
	M2.log(llv, self.__cname..self._id..'.load()')
	uu.addPage(self)
	self.parent = page
	if page then
		if UITools.LoadPanel(self.config, self._id, page._id + (parent or 0)) < 0 then
			uu.removePage(self._id)
			return false
		end
	else
		if UITools.LoadPage(self.config, self._id) < 0 then
			uu.removePage(self._id)
			return false
		end
	end
	return true
end

-- 加载完成后，返回自动绑定的控件
function Page:onLoad( controls )
	M2.log(llv, self.__cname..self._id..'.onLoad()')
	self.c = {}
	for k,v in pairs(controls) do
		self.c[k] = v
	end
	
	-- 增加名称作为键值
	for k,v in pairs(controls) do
		v.name = k
		self.c[v.index] = v
	end

	UITools.SetName(self._id, self.__cname)
	
	-- 加载结束，调用派生类的初始化函数
	if self.onLoaded then self:onLoaded() end
end

-- 进入场景，即Start
function Page:onEnter()
	-- 给继承类使用
	if self.onEnterBegin then self:onEnterBegin() end

	M2.log(llv, self.__cname..self._id..'.onEnter()')

	-- 绑定更新函数和响应事件函数
	BindEvent(self)
	-- print_r(self, self.__cname)

	-- 更新所有可以更新的控件
	if self.enterUpdate ~= false then
		if self.onUpdateTextImage then self:onUpdateTextImage() end
		
		for i,v in ipairs(self.c) do
			if v.onUpdate then self:addUpdate(v.index) end
		end
	end

	-- 给调用者使用
	if self.onEnterOver then self:onEnterOver() end

	self._inScene = true
end

-- 仅Lua保存父子关系，因此丢入回收站时，由lua递归调用
function Page:onExit( )
	self._inScene = false

	-- 给调用者使用
	if self.onExitBegin then self:onExitBegin() end

	M2.log(llv, self.__cname..self._id..'.onExit()')
	self.w = nil

	-- 从父界面移除
	if self.parent then
		for i,v in ipairs(self.parent.children) do
			if v == self then table.remove(self.parent.children, i) break end
		end
		self.parent = nil
	end
	self._index = nil

	-- 移除子界面，先保存，否则子界面移除时会再次删除
	local children = self.children
	self.children = {}
	for i,v in ipairs(children or {}) do
		v:onExit()
	end

	-- 清除数据
	for i,v in ipairs(self.c or {}) do
		v.data = nil
	end

	-- 如果允许重载，则需要删除
	if self.allowReload == true then
		UITools.UnloadUI(self._id, true)
	else
		UITools.UnloadUI(self._id, false)
		uu.removePage(self._id)
	end

	-- 给继承类使用
	if self.onExitOver then self:onExitOver() end
end

-- 被销毁，即Destroy
function Page:onDestroy(  )
	-- 给调用者使用
	if self.onDestroyBegin then self:onDestroyBegin() end
	
	M2.log(llv, self.__cname..self._id..'.onDestroy()')
	
	self.children = nil
	self.onEvents = nil
	self.c = nil
	self.w = nil

	self.parent = nil
	self._index = nil
	self._inScene = nil

	uu.destroyPage(self._id)

	-- 给继承类使用
	if self.onDestroyOver then self:onDestroyOver() end
end

function Page:onLateUpdate(  )
	onUpdateWait(self)
end

function Page:close(  )
	if self.parent then
		self:onExit()
	else
		M2.Route.close(self._id)
	end
end

-- 开始退出，为了兼容旧版本
function Page:beginExitTransition(  )
	self:onExit()
	-- 修改下一层界面，使之可见可点击
	UITools.ClosePage(self._id)
end

-------------------		Get Set 函数	---------------------------------

function Page:getName(  )
	return self.__cname..(self._index or '')
end

function Page:setItem( index, data )
	self._index = index or 0
	UITools.SetName(self._id, self.__cname..self._index)

	if self.onUpdateItem then self:onUpdateItem(index, data) end
end

----	Control Functions	----

function Page:addUpdates( ... )
	local arg = {...}
	if self.w == nil then self.w = arg return end

	for _,k in ipairs(arg) do
		self:addUpdate(k)
	end
end

function Page:addUpdate( ctrl )
	if self.w == nil then self.w = {ctrl} return end

	for _,v in ipairs(self.w) do
		if v == ctrl then return end
	end
	table.insert(self.w, ctrl)
end

function Page:setValue( ctrl, key, value, force )
	local c = self.c[ctrl]
	if c == nil then return end

	if c.onUpdate then
		M2.log(2, c.name..' has onUpdate so that setValue will be ignore.')
		return
	end

	if c.data == nil then
		c.data = {[key]=value}
	else
		if not force and c.data[key] == value then return end
		c.data[key] = value
	end
	-- print_r(c, key)
	self:addUpdate(ctrl)
end

function Page:setVisible( ctrl, visible )
	self:setValue(ctrl, 'visible', visible == true)
end

function Page:setEnable( ctrl, enable )
	self:setValue(ctrl, 'enable', enable == true)
end

function Page:setText( ctrl, text )
	self:setValue(ctrl, 'text', text or '')
end

function Page:setImage( ctrl, image, tex )
	local c = self.c[ctrl]
	if c == nil then return end

	image = image or ''
	tex = tex or ''

	if c.data == nil then
		c.data = {image=image, tex=tex}
	else
		if c.data.image == image and c.data.tex == tex then return end
		c.data.image = image
		c.data.tex = tex
	end
	self:addUpdate(ctrl)
end

function Page:setColor( ctrl, color )
	self:setValue(ctrl, 'color', color or 0xFFFFFFFF)
end

function Page:setToggle( ctrl, isOn, force )
	self:setValue(ctrl, 'toggle', isOn == true, force)
end

function Page:setGradient( ctrl, color1, color2 )
	local c = self.c[ctrl]
	if c == nil then return end

	color1 = color1 or 0
	color2 = color2 or 0xFFFFFFFF

	if c.data == nil then
		c.data = {gradient=true, c1=color1, c2=color2}
	else
		if c.data.c1 == color1 and c.data.c2 == color2 then return end
		c.data.gradient = true
		c.data.c1 = color1
		c.data.c2 = color2
	end

	self:addUpdate(ctrl)
end

function Page:setProgress( ctrl, current, max )
	local c = self.c[ctrl]
	if c == nil then return end

	current = current or 0
	max = max or 100

	if c.data == nil then
		c.data = {current=current, max=max}
	else
		if c.data.current == current and c.data.max == max then return end
		c.data.current = current
		c.data.max = max
	end

	self:addUpdate(ctrl)
end

function Page:setOutline( ctrl, color )
	self:setValue(ctrl, 'outline', color or 0)
end

local STAR_COLOR = {
	word = {0xFFFFF9EE, 0xFFFFF9EE, 0xFF42FF00, 0xFF00BAFF, 0xFFC74FFF, 0xFFFF7200},
	line = {0xFF643B17, 0xFF643B17, 0xFF085522, 0xFF2A3796, 0xFF430967, 0xFF551A08}
}
function Page:setStarColor( ctrl, star )
	self:setValue(ctrl, 'color', STAR_COLOR.word[star] or STAR_COLOR.word[1])
	self:setValue(ctrl, 'outline', STAR_COLOR.line[star] or STAR_COLOR.line[1])
end

-------------------		Child 函数	---------------------------------

function Page:addChild( parent, alias, ... )
	local child = M2.Route.addChild(alias, ...)
	if child ~= nil then
		child:load(self, parent)

		table.insert(self.children, child)
	end
	return child
end

function Page:bindChild( parent, alias, ... )
	local child = M2.Route.addChild(alias, ...)
	if child ~= nil then
		UITools.BindPanel(self.config, self._id + parent)

		table.insert(self.children, child)
	end
	return child
end

-------------------		Event 函数	---------------------------------

-- 响应消息
function Page:onEvent( sender, e, data )
	if self.onEvents and self.onEvents[e] then
		for _,v in ipairs(self.onEvents[e]) do
			v(sender, e, data)
		end
	end
end

function Page:bindEvent( index, f )
	self.c[index].onEvent = f
	UITools.AddCtrlEvent(self._id + index, function(...) f(self, index, ...) end)
end

-- 响应消息
function Page:onNotify( sender, e, data )
	self:onEvent(sender, e, data)

	if self.children then
		for _,v in ipairs(self.children) do
			v:onNotify(sender, e, data)
		end
	end
end

-- 监听消息
function Page:listen( e, f )
	if self.onEvents == nil then self.onEvents = {} end

	if self.onEvents[e] == nil then
		self.onEvents[e] = {f}
		return 
	end

	-- 如果已经存在，则直接退出
	for i,v in ipairs(self.onEvents[e]) do
		if v == f then return end
	end

	table.insert(self.onEvents[e], f)
end

-- 取消监听
function Page:deaf( e, f )
	if self.onEvents == nil then return end
	if self.onEvents[e] == nil then return end

	for i,v in ipairs(self.onEvents[e]) do
		if v == f then table.remove(self.onEvents[e], i) end
	end
end

-- 发送消息
function Page:emit( e, data )
	self:onEvent(self, e, data)
end

-- 向父节点发送消息
function Page:bubbling( sender, e, data )
	self:onEvent(sender, e, data)

	if self.parent then
		-- print(self.parent.__cname, e)
		self.parent:bubbling(sender, e, data)
	end
end

return Page