using System;
using System.Collections.Generic;

namespace DevLab.DI
{
    public sealed class DIContainer
    {

        private readonly Dictionary<Type, object> _bindContainer = new();

        public void Bind(Type type, object o)
        {
            _bindContainer.Add(type, o);
        }

        public object Get(Type type)
        {
            _bindContainer.TryGetValue(type, out var o);
            return o;
        }
    }
}
