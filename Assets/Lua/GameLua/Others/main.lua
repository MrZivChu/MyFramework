isDebug = false
--导入需要访问的C#类型
require 'Others/wrap'
--加载lua公共方法
require 'Others/LuaUtils'
--加载关于游戏的lua公共方法
require 'Others/GameUtils'

MF = {}

--加载lua文件路径
MF.luaFileList = require 'Others/pagePath'

--加载UI界面管理模块
MF.route = require 'Others/route'
MF.page = require 'Others/page'

MF.uiRoot = GameObjectWrap.Find("Root")

--弹出登录界面
MF.route.push('UILogin')
