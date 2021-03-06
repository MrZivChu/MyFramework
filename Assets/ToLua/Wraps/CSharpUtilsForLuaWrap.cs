﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class CSharpUtilsForLuaWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(CSharpUtilsForLua), typeof(System.Object));
		L.RegFunction("LoadLevel", LoadLevel);
		L.RegFunction("GetClassType", GetClassType);
		L.RegFunction("New", _CreateCSharpUtilsForLua);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCSharpUtilsForLua(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				CSharpUtilsForLua obj = new CSharpUtilsForLua();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: CSharpUtilsForLua.New");
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
			CSharpUtilsForLua.LoadLevel(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	static Type classType = typeof(CSharpUtilsForLua);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		ToLua.Push(L, classType);
		return 1;
	}
}

