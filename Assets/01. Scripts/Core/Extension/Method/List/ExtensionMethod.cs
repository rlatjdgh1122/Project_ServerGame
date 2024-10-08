
using System;
using System.Collections.Generic;

namespace ExtensionMethod.List
{
	public static partial class ExtensionMethod
	{
		public enum ApplyToCountType
		{
			Front,
			Back,
		}

		/// <summary>
		/// 리스트의 앞부분 또는 뒷부분에서 지정된 개수의 요소에 대해 주어진 액션을 적용
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="count"></param>
		/// <param name="action"></param>
		/// <param name="type"></param>
		public static void ApplyToCount<T>(this List<T> list, int count, Action<T> action = null, ApplyToCountType type = ApplyToCountType.Front)
		{
			if (type == ApplyToCountType.Front)
			{
				for (int i = 0; i < count; ++i)
				{
					action?.Invoke(list[i]);
				}
			}

			else if (type == ApplyToCountType.Back)
			{
				for (int i = list.Count - count; i > list.Count; ++i)
				{
					action?.Invoke(list[i]);
				}
			}
		}

		/// <summary>
		/// 나와 그 나머지를 나눠서 동작
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index"> 몇번재를 나로 설정할건지</param>
		/// <param name="thisAction"> 나의 액션</param>
		/// <param name="otherAction"> 나머지의 액션</param>
		public static void IdxExcept<T>(this List<T> list, int index,
			Action<T> thisAction = null, Action<T> otherAction = null)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				if (i == index)
				{
					thisAction?.Invoke(list[i]);
				}
				else
				{
					otherAction?.Invoke(list[i]);
				}
			}
		}

		/// <summary>
		/// 나와 그 나머지를 나눠서 동작
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="obj"> 오브젝트를 나로 설정</param>
		/// <param name="thisAction"> 나의 액션</param>
		/// <param name="otherAction"> 나머지의 액션</param>
		public static void ObjExcept<T>(this List<T> list, T obj,
			Action<T> thisAction, Action<T> otherAction)
		{
			foreach (var i in list)
			{
				if (i.Equals(obj))
				{
					thisAction?.Invoke(i);
				}
				else
				{
					otherAction?.Invoke(i);
				}
			}
		}
		/// <summary>
		/// 나를 제외한 나머지만 동작
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="obj"> 오브젝트를 나로 설정</param>
		/// <param name="otherAction"> 나머지의 액션</param>
		public static void ObjExcept<T>(this List<T> list, T obj, Action<T> otherAction)
		{
			foreach (var i in list)
			{
				if (i.Equals(obj))
				{

				}
				else
				{
					otherAction?.Invoke(i);
				}
			}
		}

		public static void RemoveList<T>(this List<T> thisList, List<T> otherList)
		{
			foreach (var item in otherList)
			{
				thisList.Remove(item);
			}
		}


		public static void TryClear<T>(this List<T> list, Action<T> action = null)
		{
			if (action != null)
			{
				foreach (var item in list)
				{
					action?.Invoke(item);
				}
			} //end if

			if (list.Count > 0) list.Clear();
		}

		public static bool TryFind<T>(this List<T> list, Predicate<T> predicate, out T result)
		{
			foreach (var item in list)
			{
				if (predicate(item))
				{
					result = item;
					return true;

				} //end if

			} //end foreach

			result = default;
			return false;
		}

		public static List<TResult> Convert<TSource, TResult>(this List<TSource> source, Func<TSource, TResult> selector)
		{
			List<TResult> result = new List<TResult>();
			foreach (var item in source)
			{
				result.Add(selector(item));
			}
			return result;
		}
	}
}

