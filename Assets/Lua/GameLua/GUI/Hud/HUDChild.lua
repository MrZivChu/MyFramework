local HUDChild = class(MF.page)

HUDChild.config = {
	name = 'HUDChild',
	ab = 'hud.ab',
	prefab = 'HUDChild'
}

function HUDChild:prepareData( data )
	print('HUDChild:ctor'..self.id)
end

return HUDChild