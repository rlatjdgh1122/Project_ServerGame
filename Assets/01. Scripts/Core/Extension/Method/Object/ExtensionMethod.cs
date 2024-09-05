
using System;
using System.Collections.Generic;

namespace ExtensionMethod.Object
{
    public static partial class ExtensionMethod
    {
        public static T Cast<T>(this object target)
        {
            return (T)target;
        }
    }
}

