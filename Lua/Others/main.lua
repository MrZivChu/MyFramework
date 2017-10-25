import "ObjectsHelper"
import "LuaUtils"

MF = {}

MF.luaFileList = require 'Others/pagePath'
MF.utils = require 'Others/utils'
MF.route = require 'Others/route'
MF.page = require 'Others/page'

TipBox = require 'GUI/Common/TipBox'

MF.route.push('UILogin')
