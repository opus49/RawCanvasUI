namespace RawCanvasUI.Interfaces
{
    public interface IDataItemView<T> : IDataView<T>
        where T : class
    {
        int ItemIndex { get; set; }

        void NewItem(int index, T item);

        void ClearItem();
    }
}
