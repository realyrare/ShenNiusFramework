namespace ShenNius.ModuleCore
{

    public interface IObjectAccessor<TType>
    {
        TType Value { get; set; }
    }

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
