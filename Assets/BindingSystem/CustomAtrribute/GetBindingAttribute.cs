using System;

namespace Seongho.BindingSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetBindingAttribute : Attribute
    {
        public string Name { get; }
        public Type DataType { get; }

        public GetBindingAttribute(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;

        } //end 
    }
}