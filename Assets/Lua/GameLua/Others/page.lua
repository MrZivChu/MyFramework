local Page = {}

function Page:ctor()
	-- print_t('Page:ctor')
	self.children = {}
end

function Page:Awake()
	-- print_t('Page:Awake')
end

function Page:OnEnable()
	-- print_t('Page:OnEnable')
end

function Page:Start()
	-- print_t('Page:Start')
end

function Page:OnDisable()
	-- print_t('Page:OnDisable')
end

function Page:OnDestroy()
	-- print_t('Page:OnDestroy')
end

function Page:SetActive(isActive)
	if self.gameObject then
		local activeSelf = self.gameObject.activeSelf
		self.gameObject:SetActive(isActive)
		if isActive and activeSelf == false then
			self:OnEnable()
		elseif isActive == false and activeSelf == true then
			self:OnDisable()
		end
	end
end

function Page:getHieChild( path )
	local childrenHelper = self.gameObject:GetComponent(ChildrenHelperType)
	if childrenHelper ~= nil then
		return childrenHelper:GetHieChild(path)
	else
		print_t(self.gameObject.name..'此预设没有添加ChildrenHelper脚本')
	end
end

local clickTime = os.clock()
local function checkClickTime( ... )
	local now = os.clock()
	local diff = now - clickTime
	if diff > 0.2 then
		clickTime = now
		return true
	else
		return false
	end
end

function Page:registerBtnClick( gameObject , func , param )
 	local btn = gameObject:GetComponent(ButtonType)
	if btn then
		btn.onClick:RemoveAllListeners()
		btn.onClick:AddListener(function()
			if checkClickTime() then 
				func(self,param)
			end
		end)
	end
end

function Page:registerTogClick( gameObject , func , param )
	local tog = gameObject:GetComponent(ToggleType)
	if tog then
		btn.onValueChanged:RemoveAllListeners()
		tog.onValueChanged:AddListener(function(isOn)
			if checkClickTime() then 
				func(self,isOn,param)
			end
		end)	
	end
end


function Page:onNotify( sender , e , data )
	
end

function Page:spawnCellForTable( childName , parent , data , spawnOrUpdate )
	if data and #data > 0 and parent then
		local showCount = #data --实际要显示几个孩子
		local hasCount =  parent.transform.childCount --已经有几个孩子

		local fuyongCount = 0 --可以复用的数量
		local hideCount = 0 --隐藏的数量
		local spawnCount = 0 --要生成的数量

		if showCount >= hasCount then
			fuyongCount = hasCount
			hideCount = 0
			spawnCount = showCount - fuyongCount
		else
			fuyongCount = showCount
			hideCount = hasCount - showCount
			spawnCount = 0
		end
		
		if fuyongCount > 0 then
			for i=1,fuyongCount do
				local obj = parent.transform:GetChild(i-1).gameObject
				local thePage = self:findChild(obj)
				spawnOrUpdate(self,thePage,obj,data[i],i-1)
				obj:SetActive(true)
			end
		end

		if spawnCount > 0 then
			for i=1,spawnCount do
				local thePage = self:addChild(childName,parent)
				spawnOrUpdate(self, thePage , thePage.gameObject , data[fuyongCount + i] , fuyongCount + i - 1 )
			end
		end

		if hideCount > 0 then
			for i=fuyongCount,hasCount-1 do
				parent.transform:GetChild(i).gameObject:SetActive(false)
			end
		end 
	end
end

function Page:findChild( child )
	for i,v in ipairs(self.children) do
		if v.gameObject:Equals(child) then
			return v 
		end
	end
end

function Page:addChild( name , parent )
	local p = MF.route.Instance(name,parent)
	if p then
		table.insert(self.children,p)
		return p
	end
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