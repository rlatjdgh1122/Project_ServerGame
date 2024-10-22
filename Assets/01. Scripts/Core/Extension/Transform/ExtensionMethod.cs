
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethod.List
{
    public static partial class ExtensionMethod
    {

        public static List<T> GetComponentInChildrenAsFirst<T>(this Transform trm)
        {
            List<T> result = new();

            for (int i = 0; i < trm.childCount; i++)
            {
                if (trm.GetChild(i).TryGetComponent(out T compo))
                {
                    result.Add(compo);

                } //end if
            } //end for

            return result;
        }
    }
}

