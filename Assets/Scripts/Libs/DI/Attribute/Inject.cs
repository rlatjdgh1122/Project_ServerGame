using System;

namespace DevLab.DI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class Inject : Attribute
    {
    }
}
