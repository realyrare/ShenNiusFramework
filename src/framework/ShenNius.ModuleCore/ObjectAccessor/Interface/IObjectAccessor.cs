namespace ShenNius.ModuleCore.ObjectAccessor.Interface
{
    public interface IObjectAccessor<TType>
    {
        TType Value { get; set; }
    }
}
