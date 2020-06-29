using System;

namespace Eris1.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string categoryName) { }
    }
}