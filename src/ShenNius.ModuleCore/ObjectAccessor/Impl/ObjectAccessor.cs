using ShenNius.ModuleCore.ObjectAccessor.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ShenNius.ModuleCore.ObjectAccessor.Impl
{
    public class ObjectAccessor<TType> : IObjectAccessor<TType>
    {
        public ObjectAccessor( TType obj)
        {
            Value = obj;
        }

        public ObjectAccessor()
        {
        }

        public TType Value { get; set; }
    }
}
