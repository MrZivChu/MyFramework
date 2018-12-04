local Page = {}

function Page:ctor()
	print_t('Page:ctor')
end

function Page:Awake()
	print_t('Page:Awake')
end

function Page:OnEnable()
	print_t('Page:OnEnable')
end

function Page:Start()
	print_t('Page:Start')
end

function Page:OnDisable()
	print_t('Page:OnDisable')
end

function Page:OnDestroy()
	print_t('Page:OnDestroy')
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

function Page:addChild( name , parentID , ... )
	local layer = requireLuaFile(name)
	if layer then
		MF.route.callLifeCycle(layer,self.id,parentID,...)
		table.insert(self.childs,layer)
		return layer
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