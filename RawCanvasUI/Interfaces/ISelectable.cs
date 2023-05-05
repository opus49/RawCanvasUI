using System.Collections.Generic;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a container of objects where one can be selected.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISelectable<T> : IClickable
    {
        /// <summary>
        /// The list of items belonging to the selectable.
        /// </summary>
        List<T> Items { get; }

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        int SelectedIndex { get; }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        T SelectedItem { get; }

        /// <summary>
        /// Adds a selectable item.
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Removes all items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes an item.
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);
    }
}
