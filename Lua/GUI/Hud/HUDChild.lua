local HUDChild = class(MF.page)

HUDChild.config = {
	name = 'HUDChild',
	ab = 'gui/hud.ab',
	prefab = 'HUDChild',
	podaPath = 'GUI/Hud/HUDChild_pda'
}

function HUDChild:prepareData( data )
	print('HUDChild:ctor'..self.id)
end

return HUDChild