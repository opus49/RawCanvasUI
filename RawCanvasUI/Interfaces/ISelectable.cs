using RawCanvasUI.Mouse;
using System.Collections.Generic;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a container of objects where one can be selected.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISelectable : IControl, IObservable
    {
        /// <summary>
        /// The list of items belonging to the selectable.
        /// </summary>
        List<IDataItem> Items { get; }

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        int SelectedIndex { get; }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        IDataItem SelectedItem { get; }

        /// <summary>
        /// Adds a selectable item.
        /// </summary>
        /// <param name="item"></param>
        void Add(IDataItem item);

        /// <summary>
        /// Removes all items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes an item.
        /// </summary>
        /// <param name="item"></param>
        void Remove(IDataItem item);

        /// <summary>
        /// Select an item at the given cursor position.
        /// </summary>
        /// <param name="cursor"></param>
        void Select(Cursor cursor);
    }
}
