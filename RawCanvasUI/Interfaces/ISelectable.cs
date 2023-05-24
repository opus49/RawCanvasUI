using RawCanvasUI.Mouse;

namespace RawCanvasUI.Interfaces
{
    public interface ISelectable : IControl, IObservable
    {
        /// <summary>
        /// Gets the selected index.
        /// </summary>
        int SelectedIndex { get; }

        /// <summary>
        /// Select an item at the given cursor position.
        /// </summary>
        /// <param name="cursor"></param>
        void Select(Cursor cursor);
    }

    /// <summary>
    /// Represents a container of objects where one can be selected.
    /// </summary>
    /// <typeparam name="T">The type of selectable item.</typeparam>
    public interface ISelectable<T> : ISelectable
    {
        /// <summary>
        /// Gets the selected item.
        /// </summary>
        T SelectedItem { get; set; }
    }
}
