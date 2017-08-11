﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaUtilsWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaUtils), typeof(System.Object));
		L.RegFunction("LoadLevel", LoadLevel);
		L.RegFunction("GetClassType", GetClassType);
		L.RegFunction("New", _CreateLuaUtils);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaUtils(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LuaUtils obj = new LuaUtils();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LuaUtils.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadLevel(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			LuaUtils.LoadLevel(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	static Type classType = typeof(LuaUtils);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		ToLua.Push(L, classType);
		return 1;
	}
}

