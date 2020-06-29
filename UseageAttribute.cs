using System;

namespace Eris1.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UseageAttribute : Attribute
    {
        public UseageAttribute(string useTemplate) { }
    }
}