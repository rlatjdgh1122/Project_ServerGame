using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevLab.Support
{
    public static class Support
    {
        
        public static IReadOnlyList<Transform> GetChilds(this GameObject gameObject)
        {
            return GetChilds(gameObject.transform);
        }

        public static IReadOnlyList<Transform> GetChilds(this Transform trm)
        {

            List<Transform> children = new();
            int cnt = trm.childCount;

            for(int i = 0; i < cnt; i++)
            {
                children.Add(trm.GetChild(i));
            }

            return children;

        }

    }
}
