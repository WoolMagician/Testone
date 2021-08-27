public interface IDataContainer<out T> where T : IData
{
    T Data { get; }
}