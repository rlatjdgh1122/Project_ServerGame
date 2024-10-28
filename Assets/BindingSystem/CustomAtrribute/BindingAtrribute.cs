using System;
using UnityEngine;

namespace Seongho.BindingSystem
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
