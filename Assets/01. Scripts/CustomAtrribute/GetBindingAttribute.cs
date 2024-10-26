using System;
using System.Reflection;

namespace UnityEngine.BindingSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetBindingAttribute : Attribute
    {
        public string Name { get; }

        public GetBindingAttribute(string name)
        {
            Name = name;

        } //end 
    }
}