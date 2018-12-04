function class(parent)
	local metatable = {}
	metatable.__index = parent
	local child = setmetatable({},metatable)
	child.parent = parent
    child:ctor()
	return child
end

function requireLuaFile( name )
    local file = MF.luaFileList[name]
    if file then
        package.loaded[file] = nil 
        local layer = require(file)
        return layer
    end
end

--一个用以打印table的函数
local function tableToString(t, name, indent)   
    local tableList = {}   
    function table_r (t, name, indent, full)   
        local id = not full and name or type(name)~="number" and tostring(name) or '['..name..']'   
        local tag = indent .. id .. ' = '   
        local out = {}  -- result   
        if type(t) == "table" then   
            if tableList[t] ~= nil then   
                table.insert(out, tag .. '{} -- ' .. tableList[t] .. ' (self reference)')   
            else  
                tableList[t]= full and (full .. '.' .. id) or id  
                if next(t) then -- Table not empty   
                    table.insert(out, tag .. '{')   
                    for key,value in pairs(t) do   
                        table.insert(out,table_r(value,key,indent .. '   ',tableList[t]))   
                    end   
                    table.insert(out,indent .. '}')   
                else table.insert(out,tag .. '{}') end   
            end   
        else  
            local val = type(t)~="number" and type(t)~="boolean" and '"'..tostring(t)..'"' or tostring(t)   
            table.insert(out, tag .. val)   
        end   
        return table.concat(out, '\n')   
    end   
    return table_r(t,name or 'Value',indent or '')   
end  

function print_t( t, name)   
	print(tableToString(t, name))
end

function calcCodeExecTime( func )
    -- 记录开始时间
    local starttime = os.clock()                      
    print_t(string.format("开始时间: %.4f", starttime))
     
    -- 进行耗时操作
    func()
     
    -- 记录结束时间
    local endtime = os.clock()                         
    print_t(string.format("结束时间: %.4f", endtime))
    print_t(string.format("花费时间: %.4f", endtime - starttime))
end

function RemoveTableItem( tableList , func , removeAll )
    local i = 1
    while i <= #tableList do
        if func(tableList[i]) then
            table.remove(tableList, i)
            if removeAll == false then
                return
            end
        else
            i = i + 1
        end
    end
end