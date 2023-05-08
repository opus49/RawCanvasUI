namespace RawCanvasUI.Interfaces
{
    public interface IDataView<T>
        where T : class
    {
        void UpdateItem(int index, T item);
    }
}
