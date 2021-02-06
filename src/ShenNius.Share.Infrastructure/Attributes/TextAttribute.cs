using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
