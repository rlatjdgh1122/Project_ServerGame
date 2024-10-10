using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtensionMethod.Dictionary
{
	public static partial class ExtensionMethod
	{

		public static void TryRemove<Key, Value>(this Dictionary<Key, Value> dic, Key key, Action<Value> action = null)
		{
			action?.Invoke(dic.GetValue(key));
			dic.Remove(key);
		}

		public static void TryClear<key, value>(this Dictionary<key, value> dic, Action<value> action = null)
		{
			if (action != null)
			{
				foreach (var item in dic)
				{
					action?.Invoke(item.Value);
				}

			}//end if


			if (dic.Count > 0) dic.Clear();
		}

		public static void TryClear<key, value>(this Dictionary<key, value> dic, Action<key, value> action)
		{
			if (action != null)
			{
				foreach (var item in dic)
				{
					action?.Invoke(item.Key, item.Value);
				}

			}//end if


			if (dic.Count > 0) dic.Clear();
		}

		public static void KeyExcept<Key, Value>(this Dictionary<Key, Value> dic, Key key, Action<Value> thisAction = null, Action<Value> otherAction = null) where Key : IEquatable<Key>
		{
			foreach(var item in dic)
			{
				if (item.Key.Equals(key))
				{
					thisAction?.Invoke(item.Value);
				}
				else
				{
					otherAction?.Invoke(item.Value);
				}

			} //end foreach
		}

		public static value GetValue<key, value>(this Dictionary<key, value> dic, key myKey)
		{
			if (dic.TryGetValue(myKey, out value myValue))
			{
				return myValue;
			}

			Debug_S.LogError($"{myKey}키와 연결된 벨류를 찾을 수 없습니다");

			return default;
		}
	}
}
