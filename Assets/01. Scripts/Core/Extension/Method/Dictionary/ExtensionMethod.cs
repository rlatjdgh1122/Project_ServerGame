using System;
using System.Collections.Generic;

namespace ExtensionMethod.Dictionary
{
    public static partial class ExtensionMethod
    {
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
