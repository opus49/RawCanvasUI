namespace RawCanvasUI.Interfaces
{
    public interface IDataView<T>
        where T : class
    {
        /// <summary>
        /// Called when a new item is added to the data model.
        /// </summary>
        /// <param name="index">The index of the new item.</param>
        /// <param name="item">The new item</param>
        void NewItem(int index, T item);

        /// <summary>
        /// Called when the data model is reset.
        /// </summary>
        void Reset();

        /// <summary>
        /// Called when an item within the data model is updated.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="item">The item.</param>
        void UpdateItem(int index, T item);
    }
}
