using System;

namespace UnityEngine.BindingSystem
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BindingAttribute : PropertyAttribute
    {
        public string Name { get; }

        public BindingAttribute(string name)
        {
            Name = name;

        } //end 
    }
}
