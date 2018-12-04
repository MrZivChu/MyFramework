local HUD = class(MF.page)

HUD.config = {
	name = 'HUD',
	ab = 'gui/hud.ab',
	prefab = 'HUD',
	podaPath = 'GUI/Hud/HUD_pda'
}

function HUD:prepareData( data )
	print('HUD:ctor'..self.id)
	self:addButtonClik(self.id,self.index.E,self.Click ,'hahada')
	self:addToggleClik(self.id,self.index.Toggle,self.Toggle ,'its Toggle Click')
end

function HUD:onNotify( sender , e , data )
	for i,v in ipairs(self.children) do
		v:onNotify(sender, e, data)
	end
end

function HUD:Click( data )
	print_r(data)
	self:addChild('HUDChild',self.index.A)
end


function HUD:Toggle( isOn , data )
	print(tostring(isOn))
	print_r(data)
end



return HUD