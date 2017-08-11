﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_WWWWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.WWW), typeof(System.Object));
		L.RegFunction("Dispose", Dispose);
		L.RegFunction("InitWWW", InitWWW);
		L.RegFunction("EscapeURL", EscapeURL);
		L.RegFunction("UnEscapeURL", UnEscapeURL);
		L.RegFunction("GetAudioClip", GetAudioClip);
		L.RegFunction("GetAudioClipCompressed", GetAudioClipCompressed);
		L.RegFunction("LoadImageIntoTexture", LoadImageIntoTexture);
		L.RegFunction("LoadFromCacheOrDownload", LoadFromCacheOrDownload);
		L.RegFunction("GetClassType", GetClassType);
		L.RegFunction("New", _CreateUnityEngine_WWW);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("responseHeaders", get_responseHeaders, null);
		L.RegVar("text", get_text, null);
		L.RegVar("bytes", get_bytes, null);
		L.RegVar("size", get_size, null);
		L.RegVar("error", get_error, null);
		L.RegVar("texture", get_texture, null);
		L.RegVar("textureNonReadable", get_textureNonReadable, null);
		L.RegVar("audioClip", get_audioClip, null);
		L.RegVar("isDone", get_isDone, null);
		L.RegVar("progress", get_progress, null);
		L.RegVar("uploadProgress", get_uploadProgress, null);
		L.RegVar("bytesDownloaded", get_bytesDownloaded, null);
		L.RegVar("url", get_url, null);
		L.RegVar("assetBundle", get_assetBundle, null);
		L.RegVar("threadPriority", get_threadPriority, set_threadPriority);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_WWW(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.WWW obj = new UnityEngine.WWW(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(byte[])))
			{
				string arg0 = ToLua.CheckString(L, 1);
				byte[] arg1 = ToLua.CheckByteBuffer(L, 2);
				UnityEngine.WWW obj = new UnityEngine.WWW(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(UnityEngine.WWWForm)))
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.WWWForm arg1 = (UnityEngine.WWWForm)ToLua.CheckObject(L, 2, typeof(UnityEngine.WWWForm));
				UnityEngine.WWW obj = new UnityEngine.WWW(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(byte[]), typeof(System.Collections.Generic.Dictionary<string,string>)))
			{
				string arg0 = ToLua.CheckString(L, 1);
				byte[] arg1 = ToLua.CheckByteBuffer(L, 2);
				System.Collections.Generic.Dictionary<string,string> arg2 = (System.Collections.Generic.Dictionary<string,string>)ToLua.CheckObject(L, 3, typeof(System.Collections.Generic.Dictionary<string,string>));
				UnityEngine.WWW obj = new UnityEngine.WWW(arg0, arg1, arg2);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.WWW.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.CheckObject(L, 1, typeof(UnityEngine.WWW));
			obj.Dispose();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitWWW(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.CheckObject(L, 1, typeof(UnityEngine.WWW));
			string arg0 = ToLua.CheckString(L, 2);
			byte[] arg1 = ToLua.CheckByteBuffer(L, 3);
			string[] arg2 = ToLua.CheckStringArray(L, 4);
			obj.InitWWW(arg0, arg1, arg2);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EscapeURL(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(string)))
			{
				string arg0 = ToLua.ToString(L, 1);
				string o = UnityEngine.WWW.EscapeURL(arg0);
				LuaDLL.lua_pushstring(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(System.Text.Encoding)))
			{
				string arg0 = ToLua.ToString(L, 1);
				System.Text.Encoding arg1 = (System.Text.Encoding)ToLua.ToObject(L, 2);
				string o = UnityEngine.WWW.EscapeURL(arg0, arg1);
				LuaDLL.lua_pushstring(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.WWW.EscapeURL");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnEscapeURL(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(string)))
			{
				string arg0 = ToLua.ToString(L, 1);
				string o = UnityEngine.WWW.UnEscapeURL(arg0);
				LuaDLL.lua_pushstring(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(System.Text.Encoding)))
			{
				string arg0 = ToLua.ToString(L, 1);
				System.Text.Encoding arg1 = (System.Text.Encoding)ToLua.ToObject(L, 2);
				string o = UnityEngine.WWW.UnEscapeURL(arg0, arg1);
				LuaDLL.lua_pushstring(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.WWW.UnEscapeURL");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAudioClip(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.WWW), typeof(bool)))
			{
				UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.ToObject(L, 1);
				bool arg0 = LuaDLL.lua_toboolean(L, 2);
				UnityEngine.AudioClip o = obj.GetAudioClip(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.WWW), typeof(bool), typeof(bool)))
			{
				UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.ToObject(L, 1);
				bool arg0 = LuaDLL.lua_toboolean(L, 2);
				bool arg1 = LuaDLL.lua_toboolean(L, 3);
				UnityEngine.AudioClip o = obj.GetAudioClip(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.WWW), typeof(bool), typeof(bool), typeof(UnityEngine.AudioType)))
			{
				UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.ToObject(L, 1);
				bool arg0 = LuaDLL.lua_toboolean(L, 2);
				bool arg1 = LuaDLL.lua_toboolean(L, 3);
				UnityEngine.AudioType arg2 = (UnityEngine.AudioType)ToLua.ToObject(L, 4);
				UnityEngine.AudioClip o = obj.GetAudioClip(arg0, arg1, arg2);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.WWW.GetAudioClip");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAudioClipCompressed(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.WWW)))
			{
				UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.ToObject(L, 1);
				UnityEngine.AudioClip o = obj.GetAudioClipCompressed();
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.WWW), typeof(bool)))
			{
				UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.ToObject(L, 1);
				bool arg0 = LuaDLL.lua_toboolean(L, 2);
				UnityEngine.AudioClip o = obj.GetAudioClipCompressed(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.WWW), typeof(bool), typeof(UnityEngine.AudioType)))
			{
				UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.ToObject(L, 1);
				bool arg0 = LuaDLL.lua_toboolean(L, 2);
				UnityEngine.AudioType arg1 = (UnityEngine.AudioType)ToLua.ToObject(L, 3);
				UnityEngine.AudioClip o = obj.GetAudioClipCompressed(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.WWW.GetAudioClipCompressed");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadImageIntoTexture(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.WWW obj = (UnityEngine.WWW)ToLua.CheckObject(L, 1, typeof(UnityEngine.WWW));
			UnityEngine.Texture2D arg0 = (UnityEngine.Texture2D)ToLua.CheckUnityObject(L, 2, typeof(UnityEngine.Texture2D));
			obj.LoadImageIntoTexture(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadFromCacheOrDownload(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(UnityEngine.Hash128)))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.Hash128 arg1 = (UnityEngine.Hash128)ToLua.ToObject(L, 2);
				UnityEngine.WWW o = UnityEngine.WWW.LoadFromCacheOrDownload(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(int)))
			{
				string arg0 = ToLua.ToString(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				UnityEngine.WWW o = UnityEngine.WWW.LoadFromCacheOrDownload(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(UnityEngine.Hash128), typeof(uint)))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.Hash128 arg1 = (UnityEngine.Hash128)ToLua.ToObject(L, 2);
				uint arg2 = (uint)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.WWW o = UnityEngine.WWW.LoadFromCacheOrDownload(arg0, arg1, arg2);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(string), typeof(int), typeof(uint)))
			{
				string arg0 = ToLua.ToString(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				uint arg2 = (uint)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.WWW o = UnityEngine.WWW.LoadFromCacheOrDownload(arg0, arg1, arg2);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.WWW.LoadFromCacheOrDownload");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	static Type classType = typeof(UnityEngine.WWW);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		ToLua.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_responseHeaders(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			System.Collections.Generic.Dictionary<string,string> ret = obj.responseHeaders;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index responseHeaders on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_text(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			string ret = obj.text;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index text on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bytes(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			byte[] ret = obj.bytes;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index bytes on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_size(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			int ret = obj.size;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index size on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_error(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			string ret = obj.error;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index error on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_texture(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			UnityEngine.Texture2D ret = obj.texture;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index texture on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_textureNonReadable(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			UnityEngine.Texture2D ret = obj.textureNonReadable;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index textureNonReadable on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_audioClip(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			UnityEngine.AudioClip ret = obj.audioClip;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index audioClip on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isDone(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			bool ret = obj.isDone;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index isDone on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_progress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			float ret = obj.progress;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index progress on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_uploadProgress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			float ret = obj.uploadProgress;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index uploadProgress on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bytesDownloaded(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			int ret = obj.bytesDownloaded;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index bytesDownloaded on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_url(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			string ret = obj.url;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index url on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_assetBundle(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			UnityEngine.AssetBundle ret = obj.assetBundle;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index assetBundle on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_threadPriority(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			UnityEngine.ThreadPriority ret = obj.threadPriority;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index threadPriority on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_threadPriority(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.WWW obj = (UnityEngine.WWW)o;
			UnityEngine.ThreadPriority arg0 = (UnityEngine.ThreadPriority)ToLua.CheckObject(L, 2, typeof(UnityEngine.ThreadPriority));
			obj.threadPriority = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index threadPriority on a nil value" : e.Message);
		}
	}
}

