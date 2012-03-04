using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MediaVF.Services.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumStringAttribute : Attribute
    {
        public string Description { get; private set; }

        public string Abbreviation { get; private set; }

        public EnumStringAttribute(string description, string abbreviation)
        {
            Description = description;
            Abbreviation = abbreviation;
        }
    }
}
