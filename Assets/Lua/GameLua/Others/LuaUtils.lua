function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

function class(super)    
    local  cls = {}
    cls = super--clone(super)
    cls.super = super
    cls.__index = cls

    function cls.New(...)
        --元表cls为子类和父类的成员集合
        local instance = setmetatable({}, cls)
        instance:ctor(...)
        return instance
    end
    return cls
end

function requireLuaFile( name )
    local file = MF.luaFileList[name]
    if file then
        package.loaded[file] = nil 
        local layer = require(file)
        layer = layer.New()
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

-- 判断c#对象是否为空
function IsNil( uobj )
    return uobj == nil or uobj:Equals(nil)
end


--判断字符串中 汉字、字母、数字、其他字符 的个数
function getCharCount (content)
    local chineseCount = 0
    local englishCount = 0
    local numberCount = 0
    local otherCount = 0
    local contentArray = string.gmatch(content, ".[\128-\191]*")
    for w in contentArray do   
        local ascii = string.byte(w)
        if (ascii >= 65 and ascii <= 90) or (ascii>=97 and ascii <=122) then
            englishCount = englishCount + 1
        elseif ascii >= 48 and ascii <= 57 then
            numberCount = numberCount + 1
        elseif (ascii >= 0 and ascii <= 47) or (ascii >= 58 and ascii <= 64) or 
            (ascii >= 91 and ascii <= 96) or (ascii >= 123 and ascii <= 127) then
            otherCount = otherCount + 1
        else
            --ios输入法上可以输入系统表情，而此表情的ascii码值正好在这个区间，所以要用字节数来判断是否是中文
            --226 227 239 240 为ios系统表情的ascii码值
            if string.len(w) == 3 and ascii ~= 226 and ascii ~= 227 and ascii ~= 239 and ascii ~= 240  then
                chineseCount = chineseCount + 1
            else
                otherCount = otherCount + 1
            end
        end
    end
    return chineseCount, englishCount, numberCount, otherCount
end